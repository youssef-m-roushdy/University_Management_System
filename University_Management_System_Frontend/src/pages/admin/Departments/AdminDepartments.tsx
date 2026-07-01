// pages/Admin/Departments/Departments.tsx

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
} from '../../../components/icons/Icons';
import departmentService, {
  Department,
} from '../../../services/departmentService';
import './AdminDepartments.css';

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const DepartmentCard: React.FC<{
  department: Department;
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
  onView: (id: number) => void;
}> = ({ department, onEdit, onDelete, onView }) => {
  return (
    <div className="dept-card">
      <div className="dept-card-header">
        <div className="dept-card-code">{department.code}</div>
        <div className="dept-card-actions">
          <button
            className="dept-action-btn view"
            onClick={() => onView(department.id)}
            title="View Details"
          >
            <EyeIcon width={16} height={16} />
          </button>
          <button
            className="dept-action-btn edit"
            onClick={() => onEdit(department.id)}
            title="Edit Department"
          >
            <EditIcon width={16} height={16} />
          </button>
          <button
            className="dept-action-btn delete"
            onClick={() => onDelete(department.id)}
            title="Delete Department"
          >
            <TrashIcon width={16} height={16} />
          </button>
        </div>
      </div>
      <h3 className="dept-card-name">{department.name}</h3>
      <p className="dept-card-description">
        {department.description || 'No description available'}
      </p>
      <div className="dept-card-footer">
        <span className="dept-card-meta">ID: #{department.id}</span>
        {department.createdAt && (
          <span className="dept-card-meta">
            Created: {new Date(department.createdAt).toLocaleDateString()}
          </span>
        )}
      </div>
    </div>
  );
};

const DepartmentModal: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onSave: (data: { name: string; code: string; description: string }) => void;
  initialData?: Department | null;
  isEditing: boolean;
}> = ({ isOpen, onClose, onSave, initialData, isEditing }) => {
  const [formData, setFormData] = useState({
    name: '',
    code: '',
    description: '',
  });
  const [errors, setErrors] = useState<{ name?: string; code?: string }>({});

  useEffect(() => {
    if (initialData && isEditing) {
      setFormData({
        name: initialData.name || '',
        code: initialData.code || '',
        description: initialData.description || '',
      });
    } else {
      setFormData({ name: '', code: '', description: '' });
    }
    setErrors({});
  }, [initialData, isEditing, isOpen]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    // Validation
    const newErrors: { name?: string; code?: string } = {};
    if (!formData.name.trim()) newErrors.name = 'Department name is required';
    if (!formData.code.trim()) newErrors.code = 'Department code is required';

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
          <h2>{isEditing ? 'Edit Department' : 'Create Department'}</h2>
          <button className="modal-close-btn" onClick={onClose}>
            ×
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="dept-name">Department Name *</label>
            <input
              id="dept-name"
              type="text"
              value={formData.name}
              onChange={e => setFormData({ ...formData, name: e.target.value })}
              placeholder="e.g., Computer Science"
              className={errors.name ? 'error' : ''}
            />
            {errors.name && <span className="form-error">{errors.name}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="dept-code">Department Code *</label>
            <input
              id="dept-code"
              type="text"
              value={formData.code}
              onChange={e =>
                setFormData({ ...formData, code: e.target.value.toUpperCase() })
              }
              placeholder="e.g., CS"
              className={errors.code ? 'error' : ''}
              maxLength={10}
            />
            {errors.code && <span className="form-error">{errors.code}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="dept-description">Description</label>
            <textarea
              id="dept-description"
              value={formData.description}
              onChange={e =>
                setFormData({ ...formData, description: e.target.value })
              }
              placeholder="Brief description of the department..."
              rows={4}
            />
          </div>

          <div className="modal-actions">
            <button type="button" className="btn-secondary" onClick={onClose}>
              Cancel
            </button>
            <button type="submit" className="btn-primary">
              {isEditing ? 'Update' : 'Create'} Department
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
  departmentName: string;
}> = ({ isOpen, onClose, onConfirm, departmentName }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        className="modal-content confirm-dialog"
        onClick={e => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>Delete Department</h2>
          <button className="modal-close-btn" onClick={onClose}>
            ×
          </button>
        </div>
        <div className="confirm-body">
          <p>
            Are you sure you want to delete <strong>"{departmentName}"</strong>?
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

export default function Departments() {
  const navigate = useNavigate();
  const [departments, setDepartments] = useState<Department[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Pagination
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(9);
  const [totalCount, setTotalCount] = useState(0);

  // Search
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredDepartments, setFilteredDepartments] = useState<Department[]>(
    []
  );

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingDepartment, setEditingDepartment] = useState<Department | null>(
    null
  );
  const [isEditing, setIsEditing] = useState(false);

  // Confirm dialog
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [deletingDepartmentId, setDeletingDepartmentId] = useState<
    number | null
  >(null);
  const [deletingDepartmentName, setDeletingDepartmentName] = useState('');

  // ────────────────────────────────────────────────────────────────────────────
  // DATA FETCHING
  // ────────────────────────────────────────────────────────────────────────────

  const loadDepartments = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await departmentService.getAll();
      const depts = response.data || [];
      setDepartments(depts);
      setTotalCount(depts.length);
      setFilteredDepartments(depts);
    } catch (err) {
      setError('Failed to load departments. Please try again.');
      console.error('Error loading departments:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadDepartments();
  }, [loadDepartments]);

  // ────────────────────────────────────────────────────────────────────────────
  // FILTERING & PAGINATION
  // ────────────────────────────────────────────────────────────────────────────

  useEffect(() => {
    if (searchTerm.trim() === '') {
      setFilteredDepartments(departments);
    } else {
      const term = searchTerm.toLowerCase();
      const filtered = departments.filter(
        dept =>
          dept.name.toLowerCase().includes(term) ||
          dept.code.toLowerCase().includes(term) ||
          (dept.description && dept.description.toLowerCase().includes(term))
      );
      setFilteredDepartments(filtered);
    }
    setCurrentPage(1);
  }, [searchTerm, departments]);

  const paginatedDepartments = useMemo(() => {
    const start = (currentPage - 1) * pageSize;
    const end = start + pageSize;
    return filteredDepartments.slice(start, end);
  }, [filteredDepartments, currentPage, pageSize]);

  const totalPages = Math.ceil(filteredDepartments.length / pageSize);

  // ────────────────────────────────────────────────────────────────────────────
  // CRUD OPERATIONS
  // ────────────────────────────────────────────────────────────────────────────

  const handleCreate = async (data: {
    name: string;
    code: string;
    description: string;
  }) => {
    try {
      await departmentService.create(data);
      await loadDepartments();
    } catch (err) {
      console.error('Error creating department:', err);
      alert('Failed to create department. Please try again.');
    }
  };

  const handleUpdate = async (data: {
    name: string;
    code: string;
    description: string;
  }) => {
    if (!editingDepartment) return;
    try {
      await departmentService.update(editingDepartment.id, data);
      await loadDepartments();
      setEditingDepartment(null);
    } catch (err) {
      console.error('Error updating department:', err);
      alert('Failed to update department. Please try again.');
    }
  };

  const handleDelete = async () => {
    if (!deletingDepartmentId) return;
    try {
      await departmentService.delete(deletingDepartmentId);
      await loadDepartments();
      setIsConfirmOpen(false);
      setDeletingDepartmentId(null);
      setDeletingDepartmentName('');
    } catch (err) {
      console.error('Error deleting department:', err);
      alert('Failed to delete department. Please try again.');
    }
  };

  const openCreateModal = () => {
    setEditingDepartment(null);
    setIsEditing(false);
    setIsModalOpen(true);
  };

  const openEditModal = (id: number) => {
    const dept = departments.find(d => d.id === id);
    if (dept) {
      setEditingDepartment(dept);
      setIsEditing(true);
      setIsModalOpen(true);
    }
  };

  const openDeleteConfirm = (id: number) => {
    const dept = departments.find(d => d.id === id);
    if (dept) {
      setDeletingDepartmentId(id);
      setDeletingDepartmentName(dept.name);
      setIsConfirmOpen(true);
    }
  };

  const handleViewDepartment = (id: number) => {
    navigate(`/admin/departments/${id}`);
  };

  // ────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ────────────────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="departments-loading">
        <div className="spinner"></div>
        <p>Loading departments...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="departments-error">
        <p>{error}</p>
        <button className="btn-primary" onClick={loadDepartments}>
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className="departments-page">
      {/* Header */}
      <div className="departments-header">
        <div className="departments-header-text">
          <h1>Departments</h1>
          <p>Manage your university departments</p>
        </div>
        <div className="departments-header-actions">
          <div className="search-box">
            <SearchIcon width={18} height={18} />
            <input
              type="text"
              placeholder="Search departments..."
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="btn-primary" onClick={openCreateModal}>
            <PlusIcon width={18} height={18} />
            Add Department
          </button>
        </div>
      </div>

      {/* Stats */}
      <div className="departments-stats">
        <div className="stat-item">
          <span className="stat-label">Total Departments</span>
          <span className="stat-value">{totalCount}</span>
        </div>
        <div className="stat-item">
          <span className="stat-label">Showing</span>
          <span className="stat-value">{filteredDepartments.length}</span>
        </div>
        <div className="stat-item">
          <span className="stat-label">Page</span>
          <span className="stat-value">
            {currentPage} of {totalPages || 1}
          </span>
        </div>
      </div>

      {/* Department Grid */}
      {filteredDepartments.length === 0 ? (
        <div className="departments-empty">
          <p>No departments found</p>
          {searchTerm && <p>Try adjusting your search term</p>}
        </div>
      ) : (
        <>
          <div className="departments-grid">
            {paginatedDepartments.map(department => (
              <DepartmentCard
                key={department.id}
                department={department}
                onEdit={openEditModal}
                onDelete={openDeleteConfirm}
                onView={handleViewDepartment}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="departments-pagination">
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
      <DepartmentModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setEditingDepartment(null);
        }}
        onSave={isEditing ? handleUpdate : handleCreate}
        initialData={editingDepartment}
        isEditing={isEditing}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => {
          setIsConfirmOpen(false);
          setDeletingDepartmentId(null);
          setDeletingDepartmentName('');
        }}
        onConfirm={handleDelete}
        departmentName={deletingDepartmentName}
      />
    </div>
  );
}
