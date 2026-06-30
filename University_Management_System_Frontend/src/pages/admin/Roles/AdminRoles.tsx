// pages/admin/Roles/AdminRoles.tsx

import React, { useState, useEffect, useCallback } from 'react';
import {
  PlusIcon,
  SearchIcon,
  EditIcon,
  TrashIcon,
  ChevronLeftIcon,
  ChevronRightIcon,
  XIcon,
  ShieldIcon,
} from '../../../components/icons/Icons';
import rolesService, { Role } from '../../../services/rolesService';
import './AdminRoles.css';

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const RoleCard: React.FC<{
  role: Role;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
}> = ({ role, onEdit, onDelete }) => {
  return (
    <div className="role-card">
      <div className="role-card-header">
        <div className="role-icon-wrapper">
          <ShieldIcon width={24} height={24} />
        </div>
        <div className="role-actions">
          <button
            className="role-action-btn edit"
            onClick={() => onEdit(role.id)}
          >
            <EditIcon width={16} height={16} />
          </button>
          <button
            className="role-action-btn delete"
            onClick={() => onDelete(role.id)}
          >
            <TrashIcon width={16} height={16} />
          </button>
        </div>
      </div>
      <h3 className="role-card-name">{role.name}</h3>
      <p className="role-card-id">ID: {role.id}</p>
    </div>
  );
};

const RoleModal: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onSave: (data: any) => void;
  initialData?: Role | null;
  isEditing: boolean;
}> = ({ isOpen, onClose, onSave, initialData, isEditing }) => {
  const [roleName, setRoleName] = useState('');
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (initialData && isEditing) {
      setRoleName(initialData.name || '');
    } else {
      setRoleName('');
    }
    setErrors({});
  }, [initialData, isEditing, isOpen]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!roleName.trim()) {
      setErrors({ roleName: 'Role name is required' });
      return;
    }

    if (isEditing) {
      onSave({ newRoleName: roleName });
    } else {
      onSave({ roleName });
    }
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEditing ? 'Edit Role' : 'Create Role'}</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="role-name">Role Name *</label>
            <input
              id="role-name"
              type="text"
              value={roleName}
              onChange={e => {
                setRoleName(e.target.value);
                setErrors({});
              }}
              placeholder="Enter role name (e.g., Admin, Student)"
              className={errors.roleName ? 'error' : ''}
            />
            {errors.roleName && (
              <span className="form-error">{errors.roleName}</span>
            )}
          </div>

          <div className="modal-actions">
            <button type="button" className="btn-secondary" onClick={onClose}>
              Cancel
            </button>
            <button type="submit" className="btn-primary">
              {isEditing ? 'Update' : 'Create'} Role
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

const ConfirmDialog: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onConfirm: () => void;
  roleName: string;
}> = ({ isOpen, onClose, onConfirm, roleName }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        className="modal-content confirm-dialog"
        onClick={e => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>Delete Role</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <div className="confirm-body">
          <p>
            Are you sure you want to delete role <strong>"{roleName}"</strong>?
          </p>
          <p className="confirm-warning">This action cannot be undone.</p>
        </div>
        <div className="modal-actions">
          <button type="button" className="btn-secondary" onClick={onClose}>
            Cancel
          </button>
          <button type="button" className="btn-danger" onClick={onConfirm}>
            Delete
          </button>
        </div>
      </div>
    </div>
  );
};

// ──────────────────────────────────────────────────────────────────────────────
// MAIN PAGE
// ──────────────────────────────────────────────────────────────────────────────

export default function AdminRoles() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [filteredRoles, setFilteredRoles] = useState<Role[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Pagination
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(12);

  // Search
  const [searchTerm, setSearchTerm] = useState('');

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingRole, setEditingRole] = useState<Role | null>(null);
  const [isEditing, setIsEditing] = useState(false);

  // Confirm dialog
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [deletingRoleId, setDeletingRoleId] = useState<string | null>(null);
  const [deletingRoleName, setDeletingRoleName] = useState('');

  // ────────────────────────────────────────────────────────────────────────────
  // DATA FETCHING
  // ────────────────────────────────────────────────────────────────────────────

  const loadRoles = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await rolesService.getAll();
      const data = response.data || [];
      setRoles(data);
      setFilteredRoles(data);
    } catch (err) {
      setError('Failed to load roles. Please try again.');
      console.error('Error loading roles:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadRoles();
  }, [loadRoles]);

  // ────────────────────────────────────────────────────────────────────────────
  // FILTERING & PAGINATION
  // ────────────────────────────────────────────────────────────────────────────

  useEffect(() => {
    if (searchTerm.trim() === '') {
      setFilteredRoles(roles);
    } else {
      const term = searchTerm.toLowerCase();
      const filtered = roles.filter(role =>
        role.name.toLowerCase().includes(term)
      );
      setFilteredRoles(filtered);
    }
    setCurrentPage(1);
  }, [searchTerm, roles]);

  const paginatedRoles = React.useMemo(() => {
    const start = (currentPage - 1) * pageSize;
    const end = start + pageSize;
    return filteredRoles.slice(start, end);
  }, [filteredRoles, currentPage, pageSize]);

  const totalPages = Math.ceil(filteredRoles.length / pageSize);

  // ────────────────────────────────────────────────────────────────────────────
  // CRUD OPERATIONS
  // ────────────────────────────────────────────────────────────────────────────

  const handleCreate = async (data: any) => {
    try {
      await rolesService.create(data);
      await loadRoles();
    } catch (err) {
      console.error('Error creating role:', err);
      alert('Failed to create role. Please try again.');
    }
  };

  const handleUpdate = async (data: any) => {
    if (!editingRole) return;
    try {
      await rolesService.update(editingRole.id, data);
      await loadRoles();
      setEditingRole(null);
    } catch (err) {
      console.error('Error updating role:', err);
      alert('Failed to update role. Please try again.');
    }
  };

  const handleDelete = async () => {
    if (!deletingRoleId) return;
    try {
      await rolesService.delete(deletingRoleId);
      await loadRoles();
      setIsConfirmOpen(false);
      setDeletingRoleId(null);
      setDeletingRoleName('');
    } catch (err) {
      console.error('Error deleting role:', err);
      alert('Failed to delete role. Please try again.');
    }
  };

  const openCreateModal = () => {
    setEditingRole(null);
    setIsEditing(false);
    setIsModalOpen(true);
  };

  const openEditModal = (id: string) => {
    const role = roles.find(r => r.id === id);
    if (role) {
      setEditingRole(role);
      setIsEditing(true);
      setIsModalOpen(true);
    }
  };

  const openDeleteConfirm = (id: string) => {
    const role = roles.find(r => r.id === id);
    if (role) {
      setDeletingRoleId(id);
      setDeletingRoleName(role.name);
      setIsConfirmOpen(true);
    }
  };

  // ────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ────────────────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="roles-loading">
        <div className="spinner"></div>
        <p>Loading roles...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="roles-error">
        <p>{error}</p>
        <button className="btn-primary" onClick={loadRoles}>
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className="roles-page">
      {/* Header */}
      <div className="roles-header">
        <div className="roles-header-text">
          <h1>Roles</h1>
          <p>Manage system roles and permissions</p>
        </div>
        <div className="roles-header-actions">
          <div className="search-box">
            <SearchIcon width={18} height={18} />
            <input
              type="text"
              placeholder="Search roles..."
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="btn-primary" onClick={openCreateModal}>
            <PlusIcon width={18} height={18} />
            Add Role
          </button>
        </div>
      </div>

      {/* Stats */}
      <div className="roles-stats">
        <div className="stat-item">
          <span className="stat-label">Total Roles</span>
          <span className="stat-value">{roles.length}</span>
        </div>
        <div className="stat-item">
          <span className="stat-label">Showing</span>
          <span className="stat-value">{filteredRoles.length}</span>
        </div>
      </div>

      {/* Role Grid */}
      {filteredRoles.length === 0 ? (
        <div className="roles-empty">
          <p>No roles found</p>
          {searchTerm && <p>Try adjusting your search</p>}
        </div>
      ) : (
        <>
          <div className="roles-grid">
            {paginatedRoles.map(role => (
              <RoleCard
                key={role.id}
                role={role}
                onEdit={openEditModal}
                onDelete={openDeleteConfirm}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="roles-pagination">
              <button
                className="pagination-btn"
                onClick={() => setCurrentPage(p => Math.max(1, p - 1))}
                disabled={currentPage === 1}
              >
                <ChevronLeftIcon width={16} height={16} />
                Previous
              </button>
              <span className="pagination-info">
                Page {currentPage} of {totalPages}
              </span>
              <button
                className="pagination-btn"
                onClick={() => setCurrentPage(p => Math.min(totalPages, p + 1))}
                disabled={currentPage === totalPages}
              >
                Next
                <ChevronRightIcon width={16} height={16} />
              </button>
            </div>
          )}
        </>
      )}

      {/* Modals */}
      <RoleModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setEditingRole(null);
        }}
        onSave={isEditing ? handleUpdate : handleCreate}
        initialData={editingRole}
        isEditing={isEditing}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => {
          setIsConfirmOpen(false);
          setDeletingRoleId(null);
          setDeletingRoleName('');
        }}
        onConfirm={handleDelete}
        roleName={deletingRoleName}
      />
    </div>
  );
}
