// pages/admin/Admins/AdminAdmins.tsx

import React, { useState, useEffect, useMemo, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  PlusIcon,
  SearchIcon,
  EditIcon,
  TrashIcon,
  EyeIcon,
  ChevronLeftIcon,
  ChevronRightIcon,
  XIcon,
  UserIcon,
  MailIcon,
  PhoneIcon,
  MapPinIcon,
  ShieldIcon,
} from '../../../components/icons/Icons';
import adminService, { Admin } from '../../../services/adminService';
import './AdminAdmins.css';
import anounmousProfilePic from '../../../assets/images/anonymous-profile.jpg';

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const AdminCard: React.FC<{
  admin: Admin;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
  onView: (id: string) => void;
}> = ({ admin, onEdit, onDelete, onView }) => {
  return (
    <div className="admin-card">
      <div className="admin-card-header">
        <div className="admin-avatar">
          {admin.profilePicture ? (
            <img src={admin.profilePicture} alt={admin.name} />
          ) : (
            <img src={anounmousProfilePic} alt="Anonymous" />
          )}
        </div>
        <div className="admin-status-badge">
          <span
            className={`status-dot ${admin.isActive ? 'active' : 'inactive'}`}
          />
          {admin.isActive ? 'Active' : 'Inactive'}
        </div>
      </div>

      <h3 className="admin-card-name">{admin.name}</h3>
      <p className="admin-card-email">{admin.email}</p>
      <p className="admin-card-username">@{admin.userName}</p>

      <div className="admin-card-details">
        <div className="admin-detail-item">
          <UserIcon width={14} height={14} />
          <span>{admin.gender}</span>
        </div>
        {admin.phoneNumber && (
          <div className="admin-detail-item">
            <PhoneIcon width={14} height={14} />
            <span>{admin.phoneNumber}</span>
          </div>
        )}
        {admin.address && (
          <div className="admin-detail-item">
            <MapPinIcon width={14} height={14} />
            <span>{admin.address}</span>
          </div>
        )}
      </div>

      <div className="admin-card-actions">
        <button
          className="admin-action-btn view"
          onClick={() => onView(admin.id)}
        >
          <EyeIcon width={16} height={16} />
          View
        </button>
        <button
          className="admin-action-btn edit"
          onClick={() => onEdit(admin.id)}
        >
          <EditIcon width={16} height={16} />
          Edit
        </button>
        <button
          className="admin-action-btn delete"
          onClick={() => onDelete(admin.id)}
        >
          <TrashIcon width={16} height={16} />
          Delete
        </button>
      </div>
    </div>
  );
};

const AdminModal: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onSave: (data: any) => void;
  initialData?: Admin | null;
  isEditing: boolean;
}> = ({ isOpen, onClose, onSave, initialData, isEditing }) => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    userName: '',
    phoneNumber: '',
    address: '',
    gender: 'Male' as 'Male' | 'Female',
    isActive: true,
    password: '',
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (initialData && isEditing) {
      setFormData({
        name: initialData.name || '',
        email: initialData.email || '',
        userName: initialData.userName || '',
        phoneNumber: initialData.phoneNumber || '',
        address: initialData.address || '',
        gender: initialData.gender || 'Male',
        isActive: initialData.isActive ?? true,
        password: '',
      });
    } else {
      setFormData({
        name: '',
        email: '',
        userName: '',
        phoneNumber: '',
        address: '',
        gender: 'Male',
        isActive: true,
        password: '',
      });
    }
    setErrors({});
  }, [initialData, isEditing, isOpen]);

  // Option 1: Destructuring (Recommended)
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const newErrors: Record<string, string> = {};
    if (!formData.name.trim()) newErrors.name = 'Name is required';
    if (!formData.email.trim()) newErrors.email = 'Email is required';
    if (!formData.userName.trim()) newErrors.userName = 'Username is required';
    if (!isEditing && !formData.password.trim())
      newErrors.password = 'Password is required';

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    // Destructure to remove password for updates
    const { password, ...submitData } = formData;

    if (isEditing) {
      // Password excluded via destructuring
      onSave(submitData);
    } else {
      // Include password for creation
      onSave(formData);
    }
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEditing ? 'Edit Admin' : 'Create Admin'}</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="admin-name">Full Name *</label>
              <input
                id="admin-name"
                type="text"
                value={formData.name}
                onChange={e =>
                  setFormData({ ...formData, name: e.target.value })
                }
                placeholder="John Doe"
                className={errors.name ? 'error' : ''}
              />
              {errors.name && <span className="form-error">{errors.name}</span>}
            </div>

            <div className="form-group">
              <label htmlFor="admin-email">Email *</label>
              <input
                id="admin-email"
                type="email"
                value={formData.email}
                onChange={e =>
                  setFormData({ ...formData, email: e.target.value })
                }
                placeholder="admin@example.com"
                className={errors.email ? 'error' : ''}
              />
              {errors.email && (
                <span className="form-error">{errors.email}</span>
              )}
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="admin-username">Username *</label>
              <input
                id="admin-username"
                type="text"
                value={formData.userName}
                onChange={e =>
                  setFormData({ ...formData, userName: e.target.value })
                }
                placeholder="johndoe"
                className={errors.userName ? 'error' : ''}
              />
              {errors.userName && (
                <span className="form-error">{errors.userName}</span>
              )}
            </div>

            <div className="form-group">
              <label htmlFor="admin-gender">Gender</label>
              <select
                id="admin-gender"
                value={formData.gender}
                onChange={e =>
                  setFormData({
                    ...formData,
                    gender: e.target.value as 'Male' | 'Female',
                  })
                }
              >
                <option value="Male">Male</option>
                <option value="Female">Female</option>
              </select>
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="admin-phone">Phone Number</label>
              <input
                id="admin-phone"
                type="tel"
                value={formData.phoneNumber}
                onChange={e =>
                  setFormData({ ...formData, phoneNumber: e.target.value })
                }
                placeholder="+1234567890"
              />
            </div>

            <div className="form-group">
              <label htmlFor="admin-address">Address</label>
              <input
                id="admin-address"
                type="text"
                value={formData.address}
                onChange={e =>
                  setFormData({ ...formData, address: e.target.value })
                }
                placeholder="123 Main St, City"
              />
            </div>
          </div>

          {!isEditing && (
            <div className="form-group">
              <label htmlFor="admin-password">Password *</label>
              <input
                id="admin-password"
                type="password"
                value={formData.password}
                onChange={e =>
                  setFormData({ ...formData, password: e.target.value })
                }
                placeholder="Enter password"
                className={errors.password ? 'error' : ''}
              />
              {errors.password && (
                <span className="form-error">{errors.password}</span>
              )}
            </div>
          )}

          <div className="form-group">
            <label className="checkbox-label">
              <input
                type="checkbox"
                checked={formData.isActive}
                onChange={e =>
                  setFormData({ ...formData, isActive: e.target.checked })
                }
              />
              <span>Active</span>
            </label>
          </div>

          <div className="modal-actions">
            <button type="button" className="btn-secondary" onClick={onClose}>
              Cancel
            </button>
            <button type="submit" className="btn-primary">
              {isEditing ? 'Update' : 'Create'} Admin
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
  adminName: string;
}> = ({ isOpen, onClose, onConfirm, adminName }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        className="modal-content confirm-dialog"
        onClick={e => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>Delete Admin</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <div className="confirm-body">
          <p>
            Are you sure you want to delete <strong>"{adminName}"</strong>?
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

export default function AdminAdmins() {
  const navigate = useNavigate();
  const [admins, setAdmins] = useState<Admin[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Pagination
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(12);
  const [totalCount, setTotalCount] = useState(0);

  // Search & Filter
  const [searchTerm, setSearchTerm] = useState('');
  const [filterGender, setFilterGender] = useState<'All' | 'Male' | 'Female'>(
    'All'
  );
  const [filterStatus, setFilterStatus] = useState<
    'All' | 'Active' | 'Inactive'
  >('All');

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingAdmin, setEditingAdmin] = useState<Admin | null>(null);
  const [isEditing, setIsEditing] = useState(false);

  // Confirm dialog
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [deletingAdminId, setDeletingAdminId] = useState<string | null>(null);
  const [deletingAdminName, setDeletingAdminName] = useState('');

  // ────────────────────────────────────────────────────────────────────────────
  // DATA FETCHING
  // ────────────────────────────────────────────────────────────────────────────

  const loadAdmins = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);

      const params: any = {
        PageNumber: currentPage,
        PageSize: pageSize,
      };

      if (searchTerm) params.SearchTerm = searchTerm;
      if (filterGender !== 'All') params.Gender = filterGender;
      if (filterStatus !== 'All') params.IsActive = filterStatus === 'Active';

      const response = await adminService.getAllWithFilters(params);
      const data = response.data || [];
      setAdmins(data);
      setTotalCount(response.pagination?.totalCount || data.length);
    } catch (err) {
      setError('Failed to load admins. Please try again.');
      console.error('Error loading admins:', err);
    } finally {
      setLoading(false);
    }
  }, [currentPage, pageSize, searchTerm, filterGender, filterStatus]);

  useEffect(() => {
    loadAdmins();
  }, [loadAdmins]);

  // ────────────────────────────────────────────────────────────────────────────
  // CRUD OPERATIONS
  // ────────────────────────────────────────────────────────────────────────────

  const handleCreate = async (data: any) => {
    try {
      await adminService.create(data);
      await loadAdmins();
    } catch (err) {
      console.error('Error creating admin:', err);
      alert('Failed to create admin. Please try again.');
    }
  };

  const handleUpdate = async (data: any) => {
    if (!editingAdmin) return;
    try {
      await adminService.update(editingAdmin.id, data);
      await loadAdmins();
      setEditingAdmin(null);
    } catch (err) {
      console.error('Error updating admin:', err);
      alert('Failed to update admin. Please try again.');
    }
  };

  const handleDelete = async () => {
    if (!deletingAdminId) return;
    try {
      await adminService.delete(deletingAdminId);
      await loadAdmins();
      setIsConfirmOpen(false);
      setDeletingAdminId(null);
      setDeletingAdminName('');
    } catch (err) {
      console.error('Error deleting admin:', err);
      alert('Failed to delete admin. Please try again.');
    }
  };

  const openCreateModal = () => {
    setEditingAdmin(null);
    setIsEditing(false);
    setIsModalOpen(true);
  };

  const openEditModal = (id: string) => {
    const admin = admins.find(a => a.id === id);
    if (admin) {
      setEditingAdmin(admin);
      setIsEditing(true);
      setIsModalOpen(true);
    }
  };

  const openDeleteConfirm = (id: string) => {
    const admin = admins.find(a => a.id === id);
    if (admin) {
      setDeletingAdminId(id);
      setDeletingAdminName(admin.name);
      setIsConfirmOpen(true);
    }
  };

  const handleViewAdmin = (id: string) => {
    navigate(`/admin/admins/${id}`);
  };

  // ────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ────────────────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="admins-loading">
        <div className="spinner"></div>
        <p>Loading admins...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="admins-error">
        <p>{error}</p>
        <button className="btn-primary" onClick={loadAdmins}>
          Retry
        </button>
      </div>
    );
  }

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <div className="admins-page">
      {/* Header */}
      <div className="admins-header">
        <div className="admins-header-text">
          <h1>Admins</h1>
          <p>Manage system administrators</p>
        </div>
        <div className="admins-header-actions">
          <div className="search-box">
            <SearchIcon width={18} height={18} />
            <input
              type="text"
              placeholder="Search admins..."
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="btn-primary" onClick={openCreateModal}>
            <PlusIcon width={18} height={18} />
            Add Admin
          </button>
        </div>
      </div>

      {/* Filters */}
      <div className="admins-filters">
        <div className="filter-group">
          <label>Gender:</label>
          <select
            value={filterGender}
            onChange={e => setFilterGender(e.target.value as any)}
          >
            <option value="All">All</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
          </select>
        </div>
        <div className="filter-group">
          <label>Status:</label>
          <select
            value={filterStatus}
            onChange={e => setFilterStatus(e.target.value as any)}
          >
            <option value="All">All</option>
            <option value="Active">Active</option>
            <option value="Inactive">Inactive</option>
          </select>
        </div>
        <div className="admins-stats">
          <span>Total: {totalCount}</span>
          <span>Showing: {admins.length}</span>
        </div>
      </div>

      {/* Admin Grid */}
      {admins.length === 0 ? (
        <div className="admins-empty">
          <p>No admins found</p>
          {searchTerm && <p>Try adjusting your search or filters</p>}
        </div>
      ) : (
        <>
          <div className="admins-grid">
            {admins.map(admin => (
              <AdminCard
                key={admin.id}
                admin={admin}
                onEdit={openEditModal}
                onDelete={openDeleteConfirm}
                onView={handleViewAdmin}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="admins-pagination">
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
      <AdminModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setEditingAdmin(null);
        }}
        onSave={isEditing ? handleUpdate : handleCreate}
        initialData={editingAdmin}
        isEditing={isEditing}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => {
          setIsConfirmOpen(false);
          setDeletingAdminId(null);
          setDeletingAdminName('');
        }}
        onConfirm={handleDelete}
        adminName={deletingAdminName}
      />
    </div>
  );
}
