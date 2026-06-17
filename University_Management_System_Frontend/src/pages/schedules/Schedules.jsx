import React, { useEffect, useState } from 'react';
import { FiFileText, FiPlus, FiTrash2, FiExternalLink } from 'react-icons/fi';
import { scheduleService } from '../../services/otherServices';
import { toast } from 'react-toastify';

export default function Schedules({ isAdmin = false }) {
  const [schedules, setSchedules] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modal, setModal] = useState(false);
  const [form, setForm] = useState({
    studyYearId: '',
    departmentId: '',
    semesterId: '',
    title: '',
    description: '',
    file: null,
  });

  const load = async () => {
    setLoading(true);
    try {
      const res = await scheduleService.getAll();
      setSchedules(res?.data || res || []);
    } catch (e) {
      toast.error('Failed to load schedules');
    }
    setLoading(false);
  };

  useEffect(() => {
    load();
  }, []);

  const handleCreate = async e => {
    e.preventDefault();
    const fd = new FormData();
    fd.append('Title', form.title);
    fd.append('Description', form.description);
    if (form.file) fd.append('File', form.file);
    try {
      await scheduleService.create(
        form.studyYearId,
        form.departmentId,
        form.semesterId,
        fd
      );
      toast.success('Schedule created');
      setModal(false);
      load();
    } catch (err) {
      toast.error(err?.errorMessage || 'Failed');
    }
  };

  const handleDelete = async id => {
    if (!window.confirm('Delete this schedule?')) return;
    try {
      await scheduleService.del(id);
      toast.success('Deleted');
      load();
    } catch (e) {
      toast.error('Failed');
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
            <FiFileText style={{ marginRight: 8 }} />
            Academic Schedules
          </h1>
          <p>{isAdmin ? 'Manage' : 'View'} schedules</p>
        </div>
        {isAdmin && (
          <button className="btn btn-primary" onClick={() => setModal(true)}>
            <FiPlus /> Upload Schedule
          </button>
        )}
      </div>

      {schedules.length === 0 ? (
        <div className="card empty-state">
          <h3>No schedules found</h3>
        </div>
      ) : (
        <div
          style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fill, minmax(320px, 1fr))',
            gap: 20,
          }}
        >
          {schedules.map(s => (
            <div className="card" key={s.id}>
              <div
                style={{
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'start',
                }}
              >
                <div>
                  <h3>{s.title}</h3>
                  {s.description && (
                    <p
                      style={{
                        color: 'var(--text-light)',
                        fontSize: '0.85rem',
                        marginTop: 4,
                      }}
                    >
                      {s.description}
                    </p>
                  )}
                  {s.createdAt && (
                    <small style={{ color: 'var(--text-light)' }}>
                      {new Date(s.createdAt).toLocaleDateString()}
                    </small>
                  )}
                </div>
                <div style={{ display: 'flex', gap: 6 }}>
                  {s.url && (
                    <a
                      href={s.url}
                      target="_blank"
                      rel="noreferrer"
                      className="btn btn-ghost btn-sm"
                    >
                      <FiExternalLink />
                    </a>
                  )}
                  {isAdmin && (
                    <button
                      className="btn btn-danger btn-sm"
                      onClick={() => handleDelete(s.id)}
                    >
                      <FiTrash2 />
                    </button>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {modal && (
        <div className="modal-overlay" onClick={() => setModal(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>Upload Academic Schedule</h2>
            <form onSubmit={handleCreate}>
              <div className="form-row">
                <div className="form-group">
                  <label>Study Year ID</label>
                  <input
                    type="number"
                    className="form-control"
                    value={form.studyYearId}
                    onChange={e =>
                      setForm({ ...form, studyYearId: e.target.value })
                    }
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Department ID</label>
                  <input
                    type="number"
                    className="form-control"
                    value={form.departmentId}
                    onChange={e =>
                      setForm({ ...form, departmentId: e.target.value })
                    }
                    required
                  />
                </div>
              </div>
              <div className="form-group">
                <label>Semester ID</label>
                <input
                  type="number"
                  className="form-control"
                  value={form.semesterId}
                  onChange={e =>
                    setForm({ ...form, semesterId: e.target.value })
                  }
                  required
                />
              </div>
              <div className="form-group">
                <label>Title</label>
                <input
                  className="form-control"
                  value={form.title}
                  onChange={e => setForm({ ...form, title: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Description</label>
                <textarea
                  className="form-control"
                  rows={2}
                  value={form.description}
                  onChange={e =>
                    setForm({ ...form, description: e.target.value })
                  }
                />
              </div>
              <div className="form-group">
                <label>File</label>
                <input
                  type="file"
                  className="form-control"
                  onChange={e => setForm({ ...form, file: e.target.files[0] })}
                  required
                />
              </div>
              <div className="form-actions">
                <button type="submit" className="btn btn-primary">
                  Upload
                </button>
                <button
                  type="button"
                  className="btn btn-ghost"
                  onClick={() => setModal(false)}
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
