import React, { useEffect, useState } from 'react';
import { FiShield, FiPlus, FiEdit2, FiTrash2, FiSearch } from 'react-icons/fi';
import { roleService } from '../../services/otherServices';
import { toast } from 'react-toastify';

export default function Roles() {
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modal, setModal] = useState(null);
  const [form, setForm] = useState({ roleName: '' });
  const [editId, setEditId] = useState(null);
  const [roleSearch, setRoleSearch] = useState('');
  const [userInfo, setUserInfo] = useState(null);

  const load = async () => {
    setLoading(true);
    try {
      const res = await roleService.getAll();
      setRoles(res?.data || res || []);
    } catch (e) {
      toast.error('Failed to load roles');
    }
    setLoading(false);
  };

  useEffect(() => {
    load();
  }, []);

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      if (modal === 'create') {
        await roleService.create(form);
        toast.success('Role created');
      } else {
        await roleService.update(editId, { newRoleName: form.roleName });
        toast.success('Role updated');
      }
      setModal(null);
      load();
    } catch (err) {
      toast.error(err?.errorMessage || 'Failed');
    }
  };

  const handleDelete = async id => {
    if (!window.confirm('Delete this role?')) return;
    try {
      await roleService.del(id);
      toast.success('Role deleted');
      load();
    } catch (e) {
      toast.error('Failed');
    }
  };

  const searchUserRole = async () => {
    if (!roleSearch) return;
    try {
      const res = await roleService.getUserRoleInfo(roleSearch);
      setUserInfo(res?.data || res);
    } catch (e) {
      toast.error('User not found');
      setUserInfo(null);
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
            <FiShield style={{ marginRight: 8 }} />
            Roles Management
          </h1>
          <p>Manage user roles</p>
        </div>
        <button
          className="btn btn-primary"
          onClick={() => {
            setForm({ roleName: '' });
            setEditId(null);
            setModal('create');
          }}
        >
          <FiPlus /> Add Role
        </button>
      </div>

      <div className="card" style={{ marginBottom: 20 }}>
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>Role Name</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {roles.map(r => (
                <tr key={r.id}>
                  <td>
                    <span className="badge badge-info">{r.name}</span>
                  </td>
                  <td>
                    <button
                      className="btn btn-ghost btn-sm"
                      onClick={() => {
                        setForm({ roleName: r.name });
                        setEditId(r.id);
                        setModal('edit');
                      }}
                    >
                      <FiEdit2 />
                    </button>
                    <button
                      className="btn btn-danger btn-sm"
                      style={{ marginLeft: 8 }}
                      onClick={() => handleDelete(r.id)}
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

      <div className="card">
        <h3 style={{ marginBottom: 12 }}>Look up User Roles</h3>
        <div style={{ display: 'flex', gap: 12 }}>
          <input
            className="form-control"
            style={{ maxWidth: 300 }}
            placeholder="Academic code..."
            value={roleSearch}
            onChange={e => setRoleSearch(e.target.value)}
            onKeyDown={e => e.key === 'Enter' && searchUserRole()}
          />
          <button className="btn btn-primary" onClick={searchUserRole}>
            <FiSearch /> Search
          </button>
        </div>
        {userInfo && (
          <div style={{ marginTop: 16 }}>
            <p>
              <strong>{userInfo.userName}</strong> — {userInfo.email}
            </p>
            <div style={{ display: 'flex', gap: 8, marginTop: 8 }}>
              {userInfo.roles?.map(r => (
                <span key={r} className="badge badge-info">
                  {r}
                </span>
              ))}
            </div>
          </div>
        )}
      </div>

      {modal && (
        <div className="modal-overlay" onClick={() => setModal(null)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>{modal === 'create' ? 'Create Role' : 'Edit Role'}</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Role Name</label>
                <input
                  className="form-control"
                  value={form.roleName}
                  onChange={e => setForm({ roleName: e.target.value })}
                  required
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
