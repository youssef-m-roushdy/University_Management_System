// pages/admin/Users/AdminUsers.tsx

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
import userService, { User } from '../../../services/userService';
import './AdminUsers.css';
import anounmousProfilePic from '../../../assets/images/anonymous-profile.jpg';

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const UserCard: React.FC<{
  user: User;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
  onView: (id: string) => void;
}> = ({ user, onEdit, onDelete, onView }) => {
  return (
    <div className="user-card">
      <div className="user-card-header">
        <div className="user-avatar">
          {user.profilePicture ? (
            <img src={user.profilePicture} alt={user.name} />
          ) : (
            <img src={anounmousProfilePic} alt="Anonymous" />
          )}
        </div>
        <div className="user-status-badge">
          <span
            className={`status-dot ${user.isActive ? 'active' : 'inactive'}`}
          />
          {user.isActive ? 'Active' : 'Inactive'}
        </div>
      </div>

      <h3 className="user-card-name">{user.displayName || user.name}</h3>
      <p className="user-card-email">{user.email}</p>
      <p className="user-card-username">@{user.userName}</p>

      <div className="user-roles-display">
        {user.roles?.map(role => (
          <span key={role} className="role-tag">
            {role}
          </span>
        ))}
      </div>

      <div className="user-card-details">
        <div className="user-detail-item">
          <UserIcon width={14} height={14} />
          <span>{user.gender || 'N/A'}</span>
        </div>
        {user.phoneNumber && (
          <div className="user-detail-item">
            <PhoneIcon width={14} height={14} />
            <span>{user.phoneNumber}</span>
          </div>
        )}
        {user.address && (
          <div className="user-detail-item">
            <MapPinIcon width={14} height={14} />
            <span>{user.address}</span>
          </div>
        )}
      </div>

      <div className="user-card-actions">
        <button
          className="user-action-btn view"
          onClick={() => onView(user.id)}
        >
          <EyeIcon width={16} height={16} />
          View
        </button>
        <button
          className="user-action-btn edit"
          onClick={() => onEdit(user.id)}
        >
          <EditIcon width={16} height={16} />
          Edit
        </button>
        <button
          className="user-action-btn delete"
          onClick={() => onDelete(user.id)}
        >
          <TrashIcon width={16} height={16} />
          Delete
        </button>
      </div>
    </div>
  );
};

const UserModal: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onSave: (data: any) => void;
  initialData?: User | null;
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
    roles: [] as string[],
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
        roles: initialData.roles || [],
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
        roles: [],
      });
    }
    setErrors({});
  }, [initialData, isEditing, isOpen]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const newErrors: Record<string, string> = {};
    if (!formData.name.trim()) newErrors.name = 'Name is required';
    if (!formData.email.trim()) newErrors.email = 'Email is required';
    if (!formData.userName.trim()) newErrors.userName = 'Username is required';

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    onSave(formData);
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEditing ? 'Edit User' : 'Create User'}</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="user-name">Full Name *</label>
              <input
                id="user-name"
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
              <label htmlFor="user-email">Email *</label>
              <input
                id="user-email"
                type="email"
                value={formData.email}
                onChange={e =>
                  setFormData({ ...formData, email: e.target.value })
                }
                placeholder="user@example.com"
                className={errors.email ? 'error' : ''}
              />
              {errors.email && (
                <span className="form-error">{errors.email}</span>
              )}
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="user-username">Username *</label>
              <input
                id="user-username"
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
              <label htmlFor="user-gender">Gender</label>
              <select
                id="user-gender"
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
              <label htmlFor="user-phone">Phone Number</label>
              <input
                id="user-phone"
                type="tel"
                value={formData.phoneNumber}
                onChange={e =>
                  setFormData({ ...formData, phoneNumber: e.target.value })
                }
                placeholder="+1234567890"
              />
            </div>

            <div className="form-group">
              <label htmlFor="user-address">Address</label>
              <input
                id="user-address"
                type="text"
                value={formData.address}
                onChange={e =>
                  setFormData({ ...formData, address: e.target.value })
                }
                placeholder="123 Main St, City"
              />
            </div>
          </div>

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
              {isEditing ? 'Update' : 'Create'} User
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
  userName: string;
}> = ({ isOpen, onClose, onConfirm, userName }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        className="modal-content confirm-dialog"
        onClick={e => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>Delete User</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <div className="confirm-body">
          <p>
            Are you sure you want to delete <strong>"{userName}"</strong>?
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

export default function AdminUsers() {
  const navigate = useNavigate();
  const [users, setUsers] = useState<User[]>([]);
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
  const [filterRole, setFilterRole] = useState<string>('All');

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [isEditing, setIsEditing] = useState(false);

  // Confirm dialog
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [deletingUserId, setDeletingUserId] = useState<string | null>(null);
  const [deletingUserName, setDeletingUserName] = useState('');

  // ─── Available roles for filter ────────────────────────────────────────────

  const availableRoles = useMemo(() => {
    const roles = new Set<string>();
    users.forEach(user => {
      user.roles?.forEach(role => roles.add(role));
    });
    return ['All', ...Array.from(roles)];
  }, [users]);

  // ────────────────────────────────────────────────────────────────────────────
  // DATA FETCHING
  // ────────────────────────────────────────────────────────────────────────────

  const loadUsers = useCallback(async () => {
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
      if (filterRole !== 'All') params.Role = filterRole;

      const response = await userService.getAll(params);
      const data = response.data || [];
      setUsers(data);
      setTotalCount(response.pagination?.totalCount || data.length);
    } catch (err) {
      setError('Failed to load users. Please try again.');
      console.error('Error loading users:', err);
    } finally {
      setLoading(false);
    }
  }, [
    currentPage,
    pageSize,
    searchTerm,
    filterGender,
    filterStatus,
    filterRole,
  ]);

  useEffect(() => {
    loadUsers();
  }, [loadUsers]);

  // ────────────────────────────────────────────────────────────────────────────
  // CRUD OPERATIONS
  // ────────────────────────────────────────────────────────────────────────────

  const handleCreate = async (data: any) => {
    try {
      // Note: User creation might be handled differently based on your API
      // This is a placeholder - implement based on actual API endpoints
      await userService.update(data);
      await loadUsers();
    } catch (err) {
      console.error('Error creating user:', err);
      alert('Failed to create user. Please try again.');
    }
  };

  const handleUpdate = async (data: any) => {
    if (!editingUser) return;
    try {
      await userService.update(data);
      await loadUsers();
      setEditingUser(null);
    } catch (err) {
      console.error('Error updating user:', err);
      alert('Failed to update user. Please try again.');
    }
  };

  const handleDelete = async () => {
    if (!deletingUserId) return;
    try {
      await userService.delete(deletingUserId);
      await loadUsers();
      setIsConfirmOpen(false);
      setDeletingUserId(null);
      setDeletingUserName('');
    } catch (err) {
      console.error('Error deleting user:', err);
      alert('Failed to delete user. Please try again.');
    }
  };

  const handleToggleActive = async (user: User) => {
    try {
      if (user.isActive) {
        await userService.deactivate(user.id);
      } else {
        await userService.activate(user.id);
      }
      await loadUsers();
    } catch (err) {
      console.error('Error toggling user status:', err);
      alert('Failed to update user status. Please try again.');
    }
  };

  const openCreateModal = () => {
    setEditingUser(null);
    setIsEditing(false);
    setIsModalOpen(true);
  };

  const openEditModal = (id: string) => {
    const user = users.find(u => u.id === id);
    if (user) {
      setEditingUser(user);
      setIsEditing(true);
      setIsModalOpen(true);
    }
  };

  const openDeleteConfirm = (id: string) => {
    const user = users.find(u => u.id === id);
    if (user) {
      setDeletingUserId(id);
      setDeletingUserName(user.displayName || user.name);
      setIsConfirmOpen(true);
    }
  };

  const handleViewUser = (id: string) => {
    navigate(`/admin/users/${id}`);
  };

  // ────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ────────────────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="users-loading">
        <div className="spinner"></div>
        <p>Loading users...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="users-error">
        <p>{error}</p>
        <button className="btn-primary" onClick={loadUsers}>
          Retry
        </button>
      </div>
    );
  }

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <div className="users-page">
      {/* Header */}
      <div className="users-header">
        <div className="users-header-text">
          <h1>Users</h1>
          <p>Manage system users</p>
        </div>
        <div className="users-header-actions">
          <div className="search-box">
            <SearchIcon width={18} height={18} />
            <input
              type="text"
              placeholder="Search users..."
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="btn-primary" onClick={openCreateModal}>
            <PlusIcon width={18} height={18} />
            Add User
          </button>
        </div>
      </div>

      {/* Filters */}
      <div className="users-filters">
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
        <div className="filter-group">
          <label>Role:</label>
          <select
            value={filterRole}
            onChange={e => setFilterRole(e.target.value)}
          >
            {availableRoles.map(role => (
              <option key={role} value={role}>
                {role}
              </option>
            ))}
          </select>
        </div>
        <div className="users-stats">
          <span>Total: {totalCount}</span>
          <span>Showing: {users.length}</span>
        </div>
      </div>

      {/* User Grid */}
      {users.length === 0 ? (
        <div className="users-empty">
          <p>No users found</p>
          {searchTerm && <p>Try adjusting your search or filters</p>}
        </div>
      ) : (
        <>
          <div className="users-grid">
            {users.map(user => (
              <UserCard
                key={user.id}
                user={user}
                onEdit={openEditModal}
                onDelete={openDeleteConfirm}
                onView={handleViewUser}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="users-pagination">
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
      <UserModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setEditingUser(null);
        }}
        onSave={isEditing ? handleUpdate : handleCreate}
        initialData={editingUser}
        isEditing={isEditing}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => {
          setIsConfirmOpen(false);
          setDeletingUserId(null);
          setDeletingUserName('');
        }}
        onConfirm={handleDelete}
        userName={deletingUserName}
      />
    </div>
  );
}
