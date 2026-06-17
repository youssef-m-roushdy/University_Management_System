import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  FiDollarSign,
  FiArrowLeft,
  FiPlus,
  FiEdit2,
  FiTrash2,
} from 'react-icons/fi';
import { feeService } from '../../services/otherServices';
import departmentService from '../../services/departmentService';
import { LEVEL_LABELS } from '../../constants';
import { toast } from 'react-toastify';

const FEE_TYPE_LABELS = { Academic: 'Academic', Registration: 'Registration' };

export default function StudyYearFees() {
  const { studyYearId } = useParams();
  const navigate = useNavigate();
  const [fees, setFees] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modal, setModal] = useState(null); // 'create' | { fee }
  const [form, setForm] = useState({
    amount: '',
    type: 'Academic',
    level: 'First_Year',
    description: '',
    departmentId: '',
  });

  const loadFees = async () => {
    try {
      const res = await feeService.getByStudyYear(studyYearId);
      setFees(res?.data || res || []);
    } catch (e) {
      toast.error('Failed to load fees');
    }
  };

  const loadDepartments = async () => {
    try {
      const res = await departmentService.getAll();
      setDepartments(res?.data || res || []);
    } catch {
      /* ignore */
    }
  };

  useEffect(() => {
    const init = async () => {
      setLoading(true);
      await Promise.all([loadFees(), loadDepartments()]);
      setLoading(false);
    };
    init();
  }, [studyYearId]);

  const resetForm = () =>
    setForm({
      amount: '',
      type: 'Academic',
      level: 'First_Year',
      description: '',
      departmentId: '',
    });

  const openCreate = () => {
    resetForm();
    setModal('create');
  };

  const openEdit = fee => {
    setForm({
      amount: fee.amount,
      type: fee.type,
      level: fee.level,
      description: fee.description || '',
      departmentId: fee.departmentId || '',
    });
    setModal({ fee });
  };

  const handleCreate = async e => {
    e.preventDefault();
    try {
      await feeService.create({
        amount: parseFloat(form.amount),
        type: form.type,
        level: form.level,
        description: form.description,
        studyYearId: parseInt(studyYearId),
        departmentId: form.departmentId
          ? parseInt(form.departmentId)
          : undefined,
      });
      toast.success('Fee created');
      setModal(null);
      loadFees();
    } catch (err) {
      toast.error(err?.errorMessage || 'Failed to create fee');
    }
  };

  const handleUpdate = async e => {
    e.preventDefault();
    try {
      await feeService.update(modal.fee.id, {
        type: form.type,
        level: form.level,
        description: form.description,
        amount: parseFloat(form.amount),
      });
      toast.success('Fee updated');
      setModal(null);
      loadFees();
    } catch (err) {
      toast.error(err?.errorMessage || 'Failed to update fee');
    }
  };

  const handleDelete = async id => {
    if (!window.confirm('Delete this fee?')) return;
    try {
      await feeService.del(id);
      toast.success('Fee deleted');
      loadFees();
    } catch (err) {
      toast.error(err?.errorMessage || 'Failed to delete fee');
    }
  };

  // Group fees by department
  const grouped = fees.reduce((acc, f) => {
    const key = f.departmentName || 'General';
    if (!acc[key]) acc[key] = [];
    acc[key].push(f);
    return acc;
  }, {});

  if (loading)
    return (
      <div className="page-container">
        <div className="spinner" />
      </div>
    );

  return (
    <div className="page-container">
      <div
        className="page-header"
        style={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
        }}
      >
        <div>
          <button
            className="btn btn-ghost btn-sm"
            style={{ marginBottom: 12 }}
            onClick={() => navigate('/admin/study-years')}
          >
            <FiArrowLeft /> Back to Study Years
          </button>
          <h1>
            <FiDollarSign style={{ marginRight: 8 }} />
            Fees — Study Year #{studyYearId}
          </h1>
          <p>Manage fee structure for this study year</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>
          <FiPlus /> Add Fee
        </button>
      </div>

      {/* Stats */}
      <div className="stats-grid" style={{ marginBottom: 24 }}>
        <div className="card stat-card">
          <div className="stat-icon blue">
            <FiDollarSign />
          </div>
          <div className="stat-info">
            <h3>{fees.length}</h3>
            <p>Total Fees</p>
          </div>
        </div>
        <div className="card stat-card">
          <div className="stat-icon green">
            <FiDollarSign />
          </div>
          <div className="stat-info">
            <h3>
              ${fees.reduce((s, f) => s + (f.amount || 0), 0).toLocaleString()}
            </h3>
            <p>Total Amount</p>
          </div>
        </div>
      </div>

      {fees.length === 0 ? (
        <div className="card empty-state">
          <h3>No fees configured</h3>
          <p>Add fees to this study year to get started.</p>
        </div>
      ) : (
        <div style={{ display: 'grid', gap: 20 }}>
          {Object.entries(grouped).map(([deptName, deptFees]) => (
            <div className="card" key={deptName}>
              <div
                style={{
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  marginBottom: 16,
                }}
              >
                <h3>{deptName}</h3>
                <span className="badge badge-info">
                  {deptFees.length} fee{deptFees.length !== 1 ? 's' : ''}
                </span>
              </div>
              <table>
                <thead>
                  <tr>
                    <th>Type</th>
                    <th>Level</th>
                    <th>Description</th>
                    <th>Amount</th>
                    <th style={{ width: 120 }}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {deptFees.map(f => (
                    <tr key={f.id}>
                      <td>
                        <span
                          className={`badge ${
                            f.type === 'Academic'
                              ? 'badge-info'
                              : 'badge-warning'
                          }`}
                        >
                          {FEE_TYPE_LABELS[f.type] || f.type}
                        </span>
                      </td>
                      <td>{LEVEL_LABELS[f.level] || f.level}</td>
                      <td>{f.description || '—'}</td>
                      <td>
                        <strong>${f.amount?.toLocaleString()}</strong>
                      </td>
                      <td>
                        <div style={{ display: 'flex', gap: 8 }}>
                          <button
                            className="btn btn-sm btn-ghost"
                            onClick={() => openEdit(f)}
                          >
                            <FiEdit2 />
                          </button>
                          <button
                            className="btn btn-sm btn-danger"
                            onClick={() => handleDelete(f.id)}
                          >
                            <FiTrash2 />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                  <tr style={{ background: '#f7fafc' }}>
                    <td colSpan={3}>
                      <strong>Subtotal</strong>
                    </td>
                    <td>
                      <strong>
                        $
                        {deptFees
                          .reduce((s, f) => s + (f.amount || 0), 0)
                          .toLocaleString()}
                      </strong>
                    </td>
                    <td />
                  </tr>
                </tbody>
              </table>
            </div>
          ))}
        </div>
      )}

      {/* Create / Edit Modal */}
      {modal && (
        <div className="modal-overlay" onClick={() => setModal(null)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>{modal === 'create' ? 'Add Fee' : 'Edit Fee'}</h2>
            <form onSubmit={modal === 'create' ? handleCreate : handleUpdate}>
              <div className="form-row">
                <div className="form-group">
                  <label>Fee Type</label>
                  <select
                    className="form-control"
                    value={form.type}
                    onChange={e => setForm({ ...form, type: e.target.value })}
                  >
                    <option value="Academic">Academic</option>
                    <option value="Registration">Registration</option>
                  </select>
                </div>
                <div className="form-group">
                  <label>Level</label>
                  <select
                    className="form-control"
                    value={form.level}
                    onChange={e => setForm({ ...form, level: e.target.value })}
                  >
                    {Object.entries(LEVEL_LABELS).map(([val, lbl]) => (
                      <option key={val} value={val}>
                        {lbl}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
              <div className="form-group">
                <label>Amount</label>
                <input
                  type="number"
                  step="0.01"
                  min="0"
                  className="form-control"
                  value={form.amount}
                  onChange={e => setForm({ ...form, amount: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Description</label>
                <input
                  type="text"
                  className="form-control"
                  value={form.description}
                  onChange={e =>
                    setForm({ ...form, description: e.target.value })
                  }
                  placeholder="Optional description"
                />
              </div>
              {modal === 'create' && (
                <div className="form-group">
                  <label>Department</label>
                  <select
                    className="form-control"
                    value={form.departmentId}
                    onChange={e =>
                      setForm({ ...form, departmentId: e.target.value })
                    }
                    required
                  >
                    <option value="">Select department</option>
                    {departments.map(d => (
                      <option key={d.id} value={d.id}>
                        {d.name}
                      </option>
                    ))}
                  </select>
                </div>
              )}
              <div className="form-actions">
                <button type="submit" className="btn btn-primary">
                  {modal === 'create' ? 'Create' : 'Update'}
                </button>
                <button
                  type="button"
                  className="btn btn-ghost"
                  onClick={() => setModal(null)}
                >
                  Cancel
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
