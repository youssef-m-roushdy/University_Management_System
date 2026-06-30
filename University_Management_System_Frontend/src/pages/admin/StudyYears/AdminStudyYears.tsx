// pages/admin/StudyYears/AdminStudyYears.tsx

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
  CalendarIcon,
  CheckCircleIcon,
  XCircleIcon,
} from '../../../components/icons/Icons';
import studyYearService, {
  StudyYear,
} from '../../../services/studyYearService';
import './AdminStudyYears.css';

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const StudyYearCard: React.FC<{
  studyYear: StudyYear;
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
  onView: (id: number) => void;
  onSetCurrent: (id: number) => void;
}> = ({ studyYear, onEdit, onDelete, onView, onSetCurrent }) => {
  return (
    <div className="study-year-card">
      <div className="study-year-card-header">
        <div className="study-year-range">
          <CalendarIcon width={20} height={20} />
          <span className="year-range">
            {studyYear.startYear} - {studyYear.endYear}
          </span>
        </div>
        <div className="study-year-status-badge">
          {studyYear.isCurrent ? (
            <span className="status-badge current">
              <CheckCircleIcon width={14} height={14} />
              Current
            </span>
          ) : (
            <span className="status-badge not-current">
              <XCircleIcon width={14} height={14} />
              Inactive
            </span>
          )}
        </div>
      </div>

      <div className="study-year-details">
        <div className="study-year-detail-item">
          <span className="detail-label">Start Year</span>
          <span className="detail-value">{studyYear.startYear}</span>
        </div>
        <div className="study-year-detail-item">
          <span className="detail-label">End Year</span>
          <span className="detail-value">{studyYear.endYear}</span>
        </div>
        <div className="study-year-detail-item">
          <span className="detail-label">Created</span>
          <span className="detail-value">
            {new Date(studyYear.createdAt).toLocaleDateString()}
          </span>
        </div>
      </div>

      <div className="study-year-card-actions">
        {!studyYear.isCurrent && (
          <button
            className="study-year-action-btn set-current"
            onClick={() => onSetCurrent(studyYear.id)}
          >
            <CheckCircleIcon width={16} height={16} />
            Set as Current
          </button>
        )}
        <button
          className="study-year-action-btn view"
          onClick={() => onView(studyYear.id)}
        >
          <EyeIcon width={16} height={16} />
          View
        </button>
        <button
          className="study-year-action-btn edit"
          onClick={() => onEdit(studyYear.id)}
        >
          <EditIcon width={16} height={16} />
          Edit
        </button>
        <button
          className="study-year-action-btn delete"
          onClick={() => onDelete(studyYear.id)}
        >
          <TrashIcon width={16} height={16} />
          Delete
        </button>
      </div>
    </div>
  );
};

const StudyYearModal: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onSave: (data: any) => void;
  initialData?: StudyYear | null;
  isEditing: boolean;
}> = ({ isOpen, onClose, onSave, initialData, isEditing }) => {
  const [formData, setFormData] = useState({
    startYear: new Date().getFullYear(),
    endYear: new Date().getFullYear() + 1,
    isCurrent: false,
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (initialData && isEditing) {
      setFormData({
        startYear: initialData.startYear || new Date().getFullYear(),
        endYear: initialData.endYear || new Date().getFullYear() + 1,
        isCurrent: initialData.isCurrent || false,
      });
    } else {
      const currentYear = new Date().getFullYear();
      setFormData({
        startYear: currentYear,
        endYear: currentYear + 1,
        isCurrent: false,
      });
    }
    setErrors({});
  }, [initialData, isEditing, isOpen]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const newErrors: Record<string, string> = {};
    if (!formData.startYear) newErrors.startYear = 'Start year is required';
    if (!formData.endYear) newErrors.endYear = 'End year is required';
    if (formData.endYear <= formData.startYear) {
      newErrors.endYear = 'End year must be greater than start year';
    }

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
          <h2>{isEditing ? 'Edit Study Year' : 'Create Study Year'}</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="start-year">Start Year *</label>
              <input
                id="start-year"
                type="number"
                value={formData.startYear}
                onChange={e =>
                  setFormData({
                    ...formData,
                    startYear: parseInt(e.target.value) || 0,
                  })
                }
                className={errors.startYear ? 'error' : ''}
                min={2000}
                max={2100}
              />
              {errors.startYear && (
                <span className="form-error">{errors.startYear}</span>
              )}
            </div>

            <div className="form-group">
              <label htmlFor="end-year">End Year *</label>
              <input
                id="end-year"
                type="number"
                value={formData.endYear}
                onChange={e =>
                  setFormData({
                    ...formData,
                    endYear: parseInt(e.target.value) || 0,
                  })
                }
                className={errors.endYear ? 'error' : ''}
                min={2001}
                max={2100}
              />
              {errors.endYear && (
                <span className="form-error">{errors.endYear}</span>
              )}
            </div>
          </div>

          <div className="form-group">
            <label className="checkbox-label">
              <input
                type="checkbox"
                checked={formData.isCurrent}
                onChange={e =>
                  setFormData({ ...formData, isCurrent: e.target.checked })
                }
              />
              <span>Set as Current Study Year</span>
            </label>
            <p className="helper-text">
              Only one study year can be current at a time.
            </p>
          </div>

          <div className="modal-actions">
            <button type="button" className="btn-secondary" onClick={onClose}>
              Cancel
            </button>
            <button type="submit" className="btn-primary">
              {isEditing ? 'Update' : 'Create'} Study Year
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
  studyYearRange: string;
}> = ({ isOpen, onClose, onConfirm, studyYearRange }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        className="modal-content confirm-dialog"
        onClick={e => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>Delete Study Year</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <div className="confirm-body">
          <p>
            Are you sure you want to delete study year{' '}
            <strong>"{studyYearRange}"</strong>?
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

export default function AdminStudyYears() {
  const navigate = useNavigate();
  const [studyYears, setStudyYears] = useState<StudyYear[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Pagination
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(12);
  const [totalCount, setTotalCount] = useState(0);

  // Search & Filter
  const [searchTerm, setSearchTerm] = useState('');
  const [filterCurrent, setFilterCurrent] = useState<
    'All' | 'Current' | 'Inactive'
  >('All');

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingStudyYear, setEditingStudyYear] = useState<StudyYear | null>(
    null
  );
  const [isEditing, setIsEditing] = useState(false);

  // Confirm dialog
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [deletingStudyYearId, setDeletingStudyYearId] = useState<number | null>(
    null
  );
  const [deletingStudyYearRange, setDeletingStudyYearRange] = useState('');

  // ────────────────────────────────────────────────────────────────────────────
  // DATA FETCHING
  // ────────────────────────────────────────────────────────────────────────────

  const loadStudyYears = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);

      const params: any = {
        PageNumber: currentPage,
        PageSize: pageSize,
      };

      if (searchTerm) {
        // Search by year range
        const years = searchTerm.split('-');
        if (years.length === 2) {
          params.StartYear = parseInt(years[0]);
          params.EndYear = parseInt(years[1]);
        }
      }
      if (filterCurrent === 'Current') params.IsCurrent = true;
      if (filterCurrent === 'Inactive') params.IsCurrent = false;

      const response = await studyYearService.getAllWithFilters(params);
      const data = response.data || [];
      setStudyYears(data);
      setTotalCount(response.pagination?.totalCount || data.length);
    } catch (err) {
      setError('Failed to load study years. Please try again.');
      console.error('Error loading study years:', err);
    } finally {
      setLoading(false);
    }
  }, [currentPage, pageSize, searchTerm, filterCurrent]);

  useEffect(() => {
    loadStudyYears();
  }, [loadStudyYears]);

  // ────────────────────────────────────────────────────────────────────────────
  // CRUD OPERATIONS
  // ────────────────────────────────────────────────────────────────────────────

  const handleCreate = async (data: any) => {
    try {
      await studyYearService.create(data);
      await loadStudyYears();
    } catch (err) {
      console.error('Error creating study year:', err);
      alert('Failed to create study year. Please try again.');
    }
  };

  const handleUpdate = async (data: any) => {
    if (!editingStudyYear) return;
    try {
      await studyYearService.update(editingStudyYear.id, data);
      await loadStudyYears();
      setEditingStudyYear(null);
    } catch (err) {
      console.error('Error updating study year:', err);
      alert('Failed to update study year. Please try again.');
    }
  };

  const handleDelete = async () => {
    if (!deletingStudyYearId) return;
    try {
      await studyYearService.delete(deletingStudyYearId);
      await loadStudyYears();
      setIsConfirmOpen(false);
      setDeletingStudyYearId(null);
      setDeletingStudyYearRange('');
    } catch (err) {
      console.error('Error deleting study year:', err);
      alert('Failed to delete study year. Please try again.');
    }
  };

  const handleSetCurrent = async (id: number) => {
    try {
      await studyYearService.setAsCurrent(id);
      await loadStudyYears();
    } catch (err) {
      console.error('Error setting study year as current:', err);
      alert('Failed to set study year as current. Please try again.');
    }
  };

  const openCreateModal = () => {
    setEditingStudyYear(null);
    setIsEditing(false);
    setIsModalOpen(true);
  };

  const openEditModal = (id: number) => {
    const studyYear = studyYears.find(s => s.id === id);
    if (studyYear) {
      setEditingStudyYear(studyYear);
      setIsEditing(true);
      setIsModalOpen(true);
    }
  };

  const openDeleteConfirm = (id: number) => {
    const studyYear = studyYears.find(s => s.id === id);
    if (studyYear) {
      setDeletingStudyYearId(id);
      setDeletingStudyYearRange(
        `${studyYear.startYear} - ${studyYear.endYear}`
      );
      setIsConfirmOpen(true);
    }
  };

  const handleViewStudyYear = (id: number) => {
    navigate(`/admin/study-years/${id}`);
  };

  // ────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ────────────────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="study-years-loading">
        <div className="spinner"></div>
        <p>Loading study years...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="study-years-error">
        <p>{error}</p>
        <button className="btn-primary" onClick={loadStudyYears}>
          Retry
        </button>
      </div>
    );
  }

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <div className="study-years-page">
      {/* Header */}
      <div className="study-years-header">
        <div className="study-years-header-text">
          <h1>Study Years</h1>
          <p>Manage academic study years</p>
        </div>
        <div className="study-years-header-actions">
          <div className="search-box">
            <SearchIcon width={18} height={18} />
            <input
              type="text"
              placeholder="Search by year (e.g., 2024-2025)..."
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="btn-primary" onClick={openCreateModal}>
            <PlusIcon width={18} height={18} />
            Add Study Year
          </button>
        </div>
      </div>

      {/* Filters */}
      <div className="study-years-filters">
        <div className="filter-group">
          <label>Status:</label>
          <select
            value={filterCurrent}
            onChange={e => setFilterCurrent(e.target.value as any)}
          >
            <option value="All">All</option>
            <option value="Current">Current</option>
            <option value="Inactive">Inactive</option>
          </select>
        </div>
        <div className="study-years-stats">
          <span>Total: {totalCount}</span>
          <span>Showing: {studyYears.length}</span>
        </div>
      </div>

      {/* Study Year Grid */}
      {studyYears.length === 0 ? (
        <div className="study-years-empty">
          <p>No study years found</p>
          {searchTerm && <p>Try adjusting your search or filters</p>}
        </div>
      ) : (
        <>
          <div className="study-years-grid">
            {studyYears.map(studyYear => (
              <StudyYearCard
                key={studyYear.id}
                studyYear={studyYear}
                onEdit={openEditModal}
                onDelete={openDeleteConfirm}
                onView={handleViewStudyYear}
                onSetCurrent={handleSetCurrent}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="study-years-pagination">
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
      <StudyYearModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setEditingStudyYear(null);
        }}
        onSave={isEditing ? handleUpdate : handleCreate}
        initialData={editingStudyYear}
        isEditing={isEditing}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => {
          setIsConfirmOpen(false);
          setDeletingStudyYearId(null);
          setDeletingStudyYearRange('');
        }}
        onConfirm={handleDelete}
        studyYearRange={deletingStudyYearRange}
      />
    </div>
  );
}
