// pages/admin/Instructors/AdminInstructors.tsx

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
import instructorService, {
  Instructor,
} from '../../../services/instructorService';
import './AdminInstructors.css';
import anounmousProfilePic from '../../../assets/images/anonymous-profile.jpg';

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const InstructorCard: React.FC<{
  instructor: Instructor;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
  onView: (id: string) => void;
}> = ({ instructor, onEdit, onDelete, onView }) => {
  return (
    <div className="instructor-card">
      <div className="instructor-card-header">
        <div className="instructor-avatar">
          {instructor.profilePicture ? (
            <img src={instructor.profilePicture} alt={instructor.name} />
          ) : (
            <img src={anounmousProfilePic} alt="Anonymous" />
          )}
        </div>
        <div className="instructor-status-badge">
          <span
            className={`status-dot ${instructor.isActive ? 'active' : 'inactive'}`}
          />
          {instructor.isActive ? 'Active' : 'Inactive'}
        </div>
      </div>

      <h3 className="instructor-card-name">{instructor.name}</h3>
      <p className="instructor-card-email">{instructor.email}</p>
      <p className="instructor-card-username">@{instructor.userName}</p>

      <div className="instructor-card-details">
        <div className="instructor-detail-item">
          <UserIcon width={14} height={14} />
          <span>{instructor.gender}</span>
        </div>
        <div className="instructor-detail-item">
          <BuildingIcon width={14} height={14} />
          <span>{instructor.departmentName}</span>
        </div>
        {instructor.phoneNumber && (
          <div className="instructor-detail-item">
            <PhoneIcon width={14} height={14} />
            <span>{instructor.phoneNumber}</span>
          </div>
        )}
      </div>

      <div className="instructor-card-actions">
        <button
          className="instructor-action-btn view"
          onClick={() => onView(instructor.id)}
        >
          <EyeIcon width={16} height={16} />
          View
        </button>
        <button
          className="instructor-action-btn edit"
          onClick={() => onEdit(instructor.id)}
        >
          <EditIcon width={16} height={16} />
          Edit
        </button>
        <button
          className="instructor-action-btn delete"
          onClick={() => onDelete(instructor.id)}
        >
          <TrashIcon width={16} height={16} />
          Delete
        </button>
      </div>
    </div>
  );
};

const InstructorModal: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onSave: (data: any) => void;
  initialData?: Instructor | null;
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
          <h2>{isEditing ? 'Edit Instructor' : 'Create Instructor'}</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="instructor-name">Full Name *</label>
              <input
                id="instructor-name"
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
              <label htmlFor="instructor-email">Email *</label>
              <input
                id="instructor-email"
                type="email"
                value={formData.email}
                onChange={e =>
                  setFormData({ ...formData, email: e.target.value })
                }
                placeholder="instructor@example.com"
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
              <label htmlFor="instructor-username">Username *</label>
              <input
                id="instructor-username"
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
              <label htmlFor="instructor-gender">Gender</label>
              <select
                id="instructor-gender"
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
              <label htmlFor="instructor-phone">Phone Number</label>
              <input
                id="instructor-phone"
                type="tel"
                value={formData.phoneNumber}
                onChange={e =>
                  setFormData({ ...formData, phoneNumber: e.target.value })
                }
                placeholder="+1234567890"
              />
            </div>

            <div className="form-group">
              <label htmlFor="instructor-address">Address</label>
              <input
                id="instructor-address"
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
            <label htmlFor="instructor-department">Department *</label>
            <input
              id="instructor-department"
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
              <label htmlFor="instructor-password">Password *</label>
              <input
                id="instructor-password"
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
              {isEditing ? 'Update' : 'Create'} Instructor
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
  instructorName: string;
}> = ({ isOpen, onClose, onConfirm, instructorName }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        className="modal-content confirm-dialog"
        onClick={e => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>Delete Instructor</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <div className="confirm-body">
          <p>
            Are you sure you want to delete <strong>"{instructorName}"</strong>?
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

export default function AdminInstructors() {
  const navigate = useNavigate();
  const [instructors, setInstructors] = useState<Instructor[]>([]);
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
  const [editingInstructor, setEditingInstructor] = useState<Instructor | null>(
    null
  );
  const [isEditing, setIsEditing] = useState(false);

  // Confirm dialog
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [deletingInstructorId, setDeletingInstructorId] = useState<
    string | null
  >(null);
  const [deletingInstructorName, setDeletingInstructorName] = useState('');

  // ────────────────────────────────────────────────────────────────────────────
  // DATA FETCHING
  // ────────────────────────────────────────────────────────────────────────────

  const loadInstructors = useCallback(async () => {
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

      const response = await instructorService.getAllWithFilters(params);
      const data = response.data || [];
      setInstructors(data);
      setTotalCount(response.pagination?.totalCount || data.length);
    } catch (err) {
      setError('Failed to load instructors. Please try again.');
      console.error('Error loading instructors:', err);
    } finally {
      setLoading(false);
    }
  }, [currentPage, pageSize, searchTerm, filterGender, filterStatus]);

  useEffect(() => {
    loadInstructors();
  }, [loadInstructors]);

  // ────────────────────────────────────────────────────────────────────────────
  // CRUD OPERATIONS
  // ────────────────────────────────────────────────────────────────────────────

  const handleCreate = async (data: any) => {
    try {
      await instructorService.create(data);
      await loadInstructors();
    } catch (err) {
      console.error('Error creating instructor:', err);
      alert('Failed to create instructor. Please try again.');
    }
  };

  const handleUpdate = async (data: any) => {
    if (!editingInstructor) return;
    try {
      await instructorService.update(editingInstructor.id, data);
      await loadInstructors();
      setEditingInstructor(null);
    } catch (err) {
      console.error('Error updating instructor:', err);
      alert('Failed to update instructor. Please try again.');
    }
  };

  const handleDelete = async () => {
    if (!deletingInstructorId) return;
    try {
      await instructorService.delete(deletingInstructorId);
      await loadInstructors();
      setIsConfirmOpen(false);
      setDeletingInstructorId(null);
      setDeletingInstructorName('');
    } catch (err) {
      console.error('Error deleting instructor:', err);
      alert('Failed to delete instructor. Please try again.');
    }
  };

  const openCreateModal = () => {
    setEditingInstructor(null);
    setIsEditing(false);
    setIsModalOpen(true);
  };

  const openEditModal = (id: string) => {
    const instructor = instructors.find(i => i.id === id);
    if (instructor) {
      setEditingInstructor(instructor);
      setIsEditing(true);
      setIsModalOpen(true);
    }
  };

  const openDeleteConfirm = (id: string) => {
    const instructor = instructors.find(i => i.id === id);
    if (instructor) {
      setDeletingInstructorId(id);
      setDeletingInstructorName(instructor.name);
      setIsConfirmOpen(true);
    }
  };

  const handleViewInstructor = (id: string) => {
    navigate(`/admin/instructors/${id}`);
  };

  // ────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ────────────────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="instructors-loading">
        <div className="spinner"></div>
        <p>Loading instructors...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="instructors-error">
        <p>{error}</p>
        <button className="btn-primary" onClick={loadInstructors}>
          Retry
        </button>
      </div>
    );
  }

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <div className="instructors-page">
      {/* Header */}
      <div className="instructors-header">
        <div className="instructors-header-text">
          <h1>Instructors</h1>
          <p>Manage faculty instructors</p>
        </div>
        <div className="instructors-header-actions">
          <div className="search-box">
            <SearchIcon width={18} height={18} />
            <input
              type="text"
              placeholder="Search instructors..."
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="btn-primary" onClick={openCreateModal}>
            <PlusIcon width={18} height={18} />
            Add Instructor
          </button>
        </div>
      </div>

      {/* Filters */}
      <div className="instructors-filters">
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
        <div className="instructors-stats">
          <span>Total: {totalCount}</span>
          <span>Showing: {instructors.length}</span>
        </div>
      </div>

      {/* Instructor Grid */}
      {instructors.length === 0 ? (
        <div className="instructors-empty">
          <p>No instructors found</p>
          {searchTerm && <p>Try adjusting your search or filters</p>}
        </div>
      ) : (
        <>
          <div className="instructors-grid">
            {instructors.map(instructor => (
              <InstructorCard
                key={instructor.id}
                instructor={instructor}
                onEdit={openEditModal}
                onDelete={openDeleteConfirm}
                onView={handleViewInstructor}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="instructors-pagination">
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
      <InstructorModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setEditingInstructor(null);
        }}
        onSave={isEditing ? handleUpdate : handleCreate}
        initialData={editingInstructor}
        isEditing={isEditing}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => {
          setIsConfirmOpen(false);
          setDeletingInstructorId(null);
          setDeletingInstructorName('');
        }}
        onConfirm={handleDelete}
        instructorName={deletingInstructorName}
      />
    </div>
  );
}
