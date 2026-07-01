// pages/admin/Assistants/AdminAssistants.tsx

import React, { useState, useEffect, useCallback } from 'react';
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
  BuildingIcon,
} from '../../../components/icons/Icons';
import assistantService, {
  Assistant,
} from '../../../services/assistantService';
import './AdminAssistants.css';
import anounmousProfilePic from '../../../assets/images/anonymous-profile.jpg';

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const AssistantCard: React.FC<{
  assistant: Assistant;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
  onView: (id: string) => void;
}> = ({ assistant, onEdit, onDelete, onView }) => {
  return (
    <div className="assistant-card">
      <div className="assistant-card-header">
        <div className="assistant-avatar">
          {assistant.profilePicture ? (
            <img src={assistant.profilePicture} alt={assistant.name} />
          ) : (
            <img src={anounmousProfilePic} alt="Anonymous" />
          )}
        </div>
        <div className="assistant-status-badge">
          <span
            className={`status-dot ${assistant.isActive ? 'active' : 'inactive'}`}
          />
          {assistant.isActive ? 'Active' : 'Inactive'}
        </div>
      </div>

      <h3 className="assistant-card-name">{assistant.name}</h3>
      <p className="assistant-card-email">{assistant.email}</p>
      <p className="assistant-card-username">@{assistant.userName}</p>

      <div className="assistant-card-details">
        <div className="assistant-detail-item">
          <UserIcon width={14} height={14} />
          <span>{assistant.gender}</span>
        </div>
        <div className="assistant-detail-item">
          <BuildingIcon width={14} height={14} />
          <span>{assistant.departmentName}</span>
        </div>
        {assistant.phoneNumber && (
          <div className="assistant-detail-item">
            <PhoneIcon width={14} height={14} />
            <span>{assistant.phoneNumber}</span>
          </div>
        )}
      </div>

      <div className="assistant-card-actions">
        <button
          className="assistant-action-btn view"
          onClick={() => onView(assistant.id)}
        >
          <EyeIcon width={16} height={16} />
          View
        </button>
        <button
          className="assistant-action-btn edit"
          onClick={() => onEdit(assistant.id)}
        >
          <EditIcon width={16} height={16} />
          Edit
        </button>
        <button
          className="assistant-action-btn delete"
          onClick={() => onDelete(assistant.id)}
        >
          <TrashIcon width={16} height={16} />
          Delete
        </button>
      </div>
    </div>
  );
};

const AssistantModal: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onSave: (data: any) => void;
  initialData?: Assistant | null;
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
    departmentId: 0,
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
        departmentId: initialData.departmentId || 0,
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
        departmentId: 0,
        password: '',
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
    if (!isEditing && !formData.password.trim())
      newErrors.password = 'Password is required';
    if (!formData.departmentId)
      newErrors.departmentId = 'Department is required';

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    const submitData = { ...formData };
    if (isEditing) {
      const { password, ...updateData } = submitData;
      onSave(updateData);
    } else {
      onSave(submitData);
    }
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEditing ? 'Edit Assistant' : 'Create Assistant'}</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="assistant-name">Full Name *</label>
              <input
                id="assistant-name"
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
              <label htmlFor="assistant-email">Email *</label>
              <input
                id="assistant-email"
                type="email"
                value={formData.email}
                onChange={e =>
                  setFormData({ ...formData, email: e.target.value })
                }
                placeholder="assistant@example.com"
                className={errors.email ? 'error' : ''}
                disabled={isEditing}
              />
              {errors.email && (
                <span className="form-error">{errors.email}</span>
              )}
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="assistant-username">Username *</label>
              <input
                id="assistant-username"
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
              <label htmlFor="assistant-gender">Gender</label>
              <select
                id="assistant-gender"
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
              <label htmlFor="assistant-phone">Phone Number</label>
              <input
                id="assistant-phone"
                type="tel"
                value={formData.phoneNumber}
                onChange={e =>
                  setFormData({ ...formData, phoneNumber: e.target.value })
                }
                placeholder="+1234567890"
              />
            </div>

            <div className="form-group">
              <label htmlFor="assistant-address">Address</label>
              <input
                id="assistant-address"
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
            <label htmlFor="assistant-department">Department *</label>
            <input
              id="assistant-department"
              type="number"
              value={formData.departmentId}
              onChange={e =>
                setFormData({
                  ...formData,
                  departmentId: parseInt(e.target.value) || 0,
                })
              }
              placeholder="Enter department ID"
              className={errors.departmentId ? 'error' : ''}
              min={1}
            />
            {errors.departmentId && (
              <span className="form-error">{errors.departmentId}</span>
            )}
          </div>

          {!isEditing && (
            <div className="form-group">
              <label htmlFor="assistant-password">Password *</label>
              <input
                id="assistant-password"
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
              {isEditing ? 'Update' : 'Create'} Assistant
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
  assistantName: string;
}> = ({ isOpen, onClose, onConfirm, assistantName }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        className="modal-content confirm-dialog"
        onClick={e => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>Delete Assistant</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <div className="confirm-body">
          <p>
            Are you sure you want to delete <strong>"{assistantName}"</strong>?
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

export default function AdminAssistants() {
  const navigate = useNavigate();
  const [assistants, setAssistants] = useState<Assistant[]>([]);
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
  const [editingAssistant, setEditingAssistant] = useState<Assistant | null>(
    null
  );
  const [isEditing, setIsEditing] = useState(false);

  // Confirm dialog
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [deletingAssistantId, setDeletingAssistantId] = useState<string | null>(
    null
  );
  const [deletingAssistantName, setDeletingAssistantName] = useState('');

  // ────────────────────────────────────────────────────────────────────────────
  // DATA FETCHING
  // ────────────────────────────────────────────────────────────────────────────

  const loadAssistants = useCallback(async () => {
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

      const response = await assistantService.getAllWithFilters(params);
      const data = response.data || [];
      setAssistants(data);
      setTotalCount(response.pagination?.totalCount || data.length);
    } catch (err) {
      setError('Failed to load assistants. Please try again.');
      console.error('Error loading assistants:', err);
    } finally {
      setLoading(false);
    }
  }, [currentPage, pageSize, searchTerm, filterGender, filterStatus]);

  useEffect(() => {
    loadAssistants();
  }, [loadAssistants]);

  // ────────────────────────────────────────────────────────────────────────────
  // CRUD OPERATIONS
  // ────────────────────────────────────────────────────────────────────────────

  const handleCreate = async (data: any) => {
    try {
      await assistantService.create(data);
      await loadAssistants();
    } catch (err) {
      console.error('Error creating assistant:', err);
      alert('Failed to create assistant. Please try again.');
    }
  };

  const handleUpdate = async (data: any) => {
    if (!editingAssistant) return;
    try {
      await assistantService.update(editingAssistant.id, data);
      await loadAssistants();
      setEditingAssistant(null);
    } catch (err) {
      console.error('Error updating assistant:', err);
      alert('Failed to update assistant. Please try again.');
    }
  };

  const handleDelete = async () => {
    if (!deletingAssistantId) return;
    try {
      await assistantService.delete(deletingAssistantId);
      await loadAssistants();
      setIsConfirmOpen(false);
      setDeletingAssistantId(null);
      setDeletingAssistantName('');
    } catch (err) {
      console.error('Error deleting assistant:', err);
      alert('Failed to delete assistant. Please try again.');
    }
  };

  const openCreateModal = () => {
    setEditingAssistant(null);
    setIsEditing(false);
    setIsModalOpen(true);
  };

  const openEditModal = (id: string) => {
    const assistant = assistants.find(a => a.id === id);
    if (assistant) {
      setEditingAssistant(assistant);
      setIsEditing(true);
      setIsModalOpen(true);
    }
  };

  const openDeleteConfirm = (id: string) => {
    const assistant = assistants.find(a => a.id === id);
    if (assistant) {
      setDeletingAssistantId(id);
      setDeletingAssistantName(assistant.name);
      setIsConfirmOpen(true);
    }
  };

  const handleViewAssistant = (id: string) => {
    navigate(`/admin/assistants/${id}`);
  };

  // ────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ────────────────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="assistants-loading">
        <div className="spinner"></div>
        <p>Loading assistants...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="assistants-error">
        <p>{error}</p>
        <button className="btn-primary" onClick={loadAssistants}>
          Retry
        </button>
      </div>
    );
  }

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <div className="assistants-page">
      {/* Header */}
      <div className="assistants-header">
        <div className="assistants-header-text">
          <h1>Assistants</h1>
          <p>Manage teaching assistants</p>
        </div>
        <div className="assistants-header-actions">
          <div className="search-box">
            <SearchIcon width={18} height={18} />
            <input
              type="text"
              placeholder="Search assistants..."
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="btn-primary" onClick={openCreateModal}>
            <PlusIcon width={18} height={18} />
            Add Assistant
          </button>
        </div>
      </div>

      {/* Filters */}
      <div className="assistants-filters">
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
        <div className="assistants-stats">
          <span>Total: {totalCount}</span>
          <span>Showing: {assistants.length}</span>
        </div>
      </div>

      {/* Assistant Grid */}
      {assistants.length === 0 ? (
        <div className="assistants-empty">
          <p>No assistants found</p>
          {searchTerm && <p>Try adjusting your search or filters</p>}
        </div>
      ) : (
        <>
          <div className="assistants-grid">
            {assistants.map(assistant => (
              <AssistantCard
                key={assistant.id}
                assistant={assistant}
                onEdit={openEditModal}
                onDelete={openDeleteConfirm}
                onView={handleViewAssistant}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="assistants-pagination">
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
      <AssistantModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setEditingAssistant(null);
        }}
        onSave={isEditing ? handleUpdate : handleCreate}
        initialData={editingAssistant}
        isEditing={isEditing}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => {
          setIsConfirmOpen(false);
          setDeletingAssistantId(null);
          setDeletingAssistantName('');
        }}
        onConfirm={handleDelete}
        assistantName={deletingAssistantName}
      />
    </div>
  );
}
