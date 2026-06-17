import React, { useEffect, useState } from 'react';
import { FiPlus, FiEdit2, FiTrash2, FiGrid } from 'react-icons/fi';
import departmentService from '../../services/departmentService';
import { toast } from 'react-toastify';

export default function Departments() {
  const [departments, setDepartments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modal, setModal] = useState(null); // 'create' | 'edit'
  const [form, setForm] = useState({ name: '', code: '', description: '' });
  const [editId, setEditId] = useState(null);

  const load = async () => {
    setLoading(true);
    try {
      const res = await departmentService.getAll();
      setDepartments(res?.data || res || []);
    } catch (e) {
      toast.error('Failed to load departments');
    }
    setLoading(false);
  };

  useEffect(() => {
    load();
  }, []);

  const openCreate = () => {
    setForm({ name: '', code: '', description: '' });
    setEditId(null);
    setModal('create');
  };
  const openEdit = d => {
    setForm({ name: d.name, code: d.code, description: d.description || '' });
    setEditId(d.id);
    setModal('edit');
  };

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      if (modal === 'create') {
        await departmentService.create(form);
        toast.success('Department created');
      } else {
        await departmentService.update(editId, form);
        toast.success('Department updated');
      }
      setModal(null);
      load();
    } catch (err) {
      toast.error(err?.errorMessage || 'Operation failed');
    }
  };

  const handleDelete = async id => {
    if (!window.confirm('Delete this department?')) return;
    try {
      await departmentService.del(id);
      toast.success('Department deleted');
      load();
    } catch (e) {
      toast.error('Failed to delete');
    }
  };

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
          <h1>
            <FiGrid style={{ marginRight: 8 }} />
            Departments
          </h1>
          <p>Manage university departments</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>
          <FiPlus /> Add Department
        </button>
      </div>

      <div className="card">
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>Code</th>
                <th>Name</th>
                <th>Description</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {departments.length === 0 && (
                <tr>
                  <td colSpan={4} className="empty-state">
                    No departments found
                  </td>
                </tr>
              )}
              {departments.map(d => (
                <tr key={d.id}>
                  <td>
                    <strong>{d.code}</strong>
                  </td>
                  <td>{d.name}</td>
                  <td>{d.description || '—'}</td>
                  <td>
                    <button
                      className="btn btn-ghost btn-sm"
                      onClick={() => openEdit(d)}
                    >
                      <FiEdit2 />
                    </button>
                    <button
                      className="btn btn-danger btn-sm"
                      style={{ marginLeft: 8 }}
                      onClick={() => handleDelete(d.id)}
                    >
                      <FiTrash2 />
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {modal && (
        <div className="modal-overlay" onClick={() => setModal(null)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>{modal === 'create' ? 'Add Department' : 'Edit Department'}</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-row">
                <div className="form-group">
                  <label>Code</label>
                  <input
                    className="form-control"
                    value={form.code}
                    onChange={e => setForm({ ...form, code: e.target.value })}
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Name</label>
                  <input
                    className="form-control"
                    value={form.name}
                    onChange={e => setForm({ ...form, name: e.target.value })}
                    required
                  />
                </div>
              </div>
              <div className="form-group">
                <label>Description</label>
                <textarea
                  className="form-control"
                  rows={3}
                  value={form.description}
                  onChange={e =>
                    setForm({ ...form, description: e.target.value })
                  }
                />
              </div>
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
