// pages/admin/DepartmentDetail/components/DepartmentSpecializations.tsx

import React, { useState, useMemo } from 'react';
import {
  PlusIcon,
  SearchIcon,
  EditIcon,
  TrashIcon,
  EyeIcon,
  XIcon,
  GraduationCapIcon,
  UsersIcon,
  BookIcon,
  ChevronLeftIcon,
  ChevronRightIcon,
} from '../../../../components/icons/Icons';
import './DepartmentSpecializations.css';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface Specialization {
  id: number;
  name: string;
  description: string;
  departmentId: number;
  departmentName: string;
  departmentCode: string;
  studentCount: number;
  courseCount: number;
  createdAt: string;
  updatedAt: string;
}

interface DepartmentSpecializationsProps {
  departmentId: number;
  departmentName: string;
  specializations: Specialization[];
  loading?: boolean;
  onAddSpecialization: () => void;
  onEditSpecialization: (id: number) => void;
  onDeleteSpecialization: (id: number) => void;
  onViewSpecialization: (id: number) => void;
}

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const SpecializationCard: React.FC<{
  specialization: Specialization;
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
  onView: (id: number) => void;
}> = ({ specialization, onEdit, onDelete, onView }) => {
  const getInitials = (name: string) => {
    return name
      .split(' ')
      .map(word => word.charAt(0))
      .join('')
      .toUpperCase()
      .slice(0, 2);
  };

  return (
    <div className="dept-specialization-card">
      <div className="dept-specialization-card-header">
        <div className="dept-specialization-avatar">
          <span>{getInitials(specialization.name)}</span>
        </div>
        <div className="dept-specialization-actions">
          <button 
            className="dept-specialization-action-btn view" 
            onClick={() => onView(specialization.id)}
            title="View Specialization"
          >
            <EyeIcon width={15} height={15} />
          </button>
          <button 
            className="dept-specialization-action-btn edit" 
            onClick={() => onEdit(specialization.id)}
            title="Edit Specialization"
          >
            <EditIcon width={15} height={15} />
          </button>
          <button 
            className="dept-specialization-action-btn delete" 
            onClick={() => onDelete(specialization.id)}
            title="Delete Specialization"
          >
            <TrashIcon width={15} height={15} />
          </button>
        </div>
      </div>

      <h4 className="dept-specialization-name">{specialization.name}</h4>
      <p className="dept-specialization-description">
        {specialization.description || 'No description available'}
      </p>

      <div className="dept-specialization-stats">
        <div className="dept-specialization-stat">
          <UsersIcon width={14} height={14} />
          <div className="stat-info">
            <span className="stat-label">Students</span>
            <span className="stat-value">{specialization.studentCount}</span>
          </div>
        </div>
        <div className="dept-specialization-stat">
          <BookIcon width={14} height={14} />
          <div className="stat-info">
            <span className="stat-label">Courses</span>
            <span className="stat-value">{specialization.courseCount}</span>
          </div>
        </div>
        <div className="dept-specialization-stat">
          <GraduationCapIcon width={14} height={14} />
          <div className="stat-info">
            <span className="stat-label">Created</span>
            <span className="stat-value">
              {new Date(specialization.createdAt).toLocaleDateString()}
            </span>
          </div>
        </div>
      </div>

      <div className="dept-specialization-progress">
        <div className="progress-bar">
          <div 
            className="progress-fill" 
            style={{ width: `${Math.min((specialization.courseCount / 10) * 100, 100)}%` }}
          ></div>
        </div>
        <span className="progress-label">
          {specialization.courseCount} courses
        </span>
      </div>
    </div>
  );
};

const SpecializationCardSkeleton: React.FC = () => {
  return (
    <div className="dept-specialization-card skeleton">
      <div className="dept-specialization-card-header">
        <div className="skeleton-avatar"></div>
        <div className="skeleton-actions">
          <div className="skeleton-action-btn"></div>
          <div className="skeleton-action-btn"></div>
          <div className="skeleton-action-btn"></div>
        </div>
      </div>
      <div className="skeleton-title"></div>
      <div className="skeleton-description"></div>
      <div className="skeleton-stats">
        <div className="skeleton-stat"></div>
        <div className="skeleton-stat"></div>
        <div className="skeleton-stat"></div>
      </div>
      <div className="skeleton-progress">
        <div className="skeleton-progress-bar"></div>
      </div>
    </div>
  );
};

// ──────────────────────────────────────────────────────────────────────────────
// MAIN COMPONENT
// ──────────────────────────────────────────────────────────────────────────────

export default function DepartmentSpecializations({
  departmentId,
  departmentName,
  specializations,
  loading = false,
  onAddSpecialization,
  onEditSpecialization,
  onDeleteSpecialization,
  onViewSpecialization,
}: DepartmentSpecializationsProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(6);

  // ─── Filtering ─────────────────────────────────────────────────────────────

  const filteredSpecializations = useMemo(() => {
    let result = specializations;

    if (searchTerm) {
      const term = searchTerm.toLowerCase();
      result = result.filter(
        (s) =>
          s.name.toLowerCase().includes(term) ||
          (s.description && s.description.toLowerCase().includes(term))
      );
    }

    return result;
  }, [specializations, searchTerm]);

  // ─── Pagination ────────────────────────────────────────────────────────────

  const totalPages = Math.ceil(filteredSpecializations.length / itemsPerPage);
  const paginatedSpecializations = useMemo(() => {
    const start = (currentPage - 1) * itemsPerPage;
    return filteredSpecializations.slice(start, start + itemsPerPage);
  }, [filteredSpecializations, currentPage, itemsPerPage]);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  // ─── Statistics ────────────────────────────────────────────────────────────

  const totalStudents = specializations.reduce((sum, s) => sum + s.studentCount, 0);
  const totalCourses = specializations.reduce((sum, s) => sum + s.courseCount, 0);
  const avgStudents = specializations.length > 0 
    ? Math.round(totalStudents / specializations.length) 
    : 0;

  // ─── Render ────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="dept-specializations">
        <div className="dept-specializations-header">
          <div className="dept-specializations-title">
            <GraduationCapIcon width={22} height={22} />
            <h3>Specializations</h3>
            <span className="dept-specializations-count">Loading...</span>
          </div>
          <div className="dept-specializations-toolbar">
            <div className="search-box skeleton-search"></div>
            <div className="skeleton-add-btn"></div>
          </div>
        </div>
        <div className="dept-specializations-grid">
          {[1, 2, 3, 4, 5, 6].map((i) => (
            <SpecializationCardSkeleton key={i} />
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className="dept-specializations">
      {/* Header */}
      <div className="dept-specializations-header">
        <div className="dept-specializations-title">
          <GraduationCapIcon width={22} height={22} />
          <h3>Specializations</h3>
          <span className="dept-specializations-count">
            {specializations.length} total
          </span>
        </div>
        <div className="dept-specializations-toolbar">
          <div className="search-box">
            <SearchIcon width={16} height={16} />
            <input
              type="text"
              placeholder="Search specializations..."
              value={searchTerm}
              onChange={(e) => {
                setSearchTerm(e.target.value);
                setCurrentPage(1);
              }}
            />
            {searchTerm && (
              <button 
                className="search-clear"
                onClick={() => setSearchTerm('')}
              >
                <XIcon width={14} height={14} />
              </button>
            )}
          </div>
          <button className="btn-primary add-btn" onClick={onAddSpecialization}>
            <PlusIcon width={16} height={16} />
            Add Specialization
          </button>
        </div>
      </div>

      {/* Stats Bar */}
      <div className="dept-specializations-stats-bar">
        <div className="stat-item">
          <span className="stat-label">Total</span>
          <span className="stat-number">{specializations.length}</span>
        </div>
        <div className="stat-divider"></div>
        <div className="stat-item">
          <span className="stat-label">Students</span>
          <span className="stat-number">{totalStudents}</span>
        </div>
        <div className="stat-divider"></div>
        <div className="stat-item">
          <span className="stat-label">Courses</span>
          <span className="stat-number">{totalCourses}</span>
        </div>
        <div className="stat-divider"></div>
        <div className="stat-item">
          <span className="stat-label">Avg. Students</span>
          <span className="stat-number">{avgStudents}</span>
        </div>
        <div className="stat-divider"></div>
        <div className="stat-item">
          <span className="stat-label">Showing</span>
          <span className="stat-number">{filteredSpecializations.length}</span>
        </div>
      </div>

      {/* Specializations Grid */}
      {filteredSpecializations.length === 0 ? (
        <div className="dept-specializations-empty">
          <GraduationCapIcon width={56} height={56} />
          <h4>No specializations found</h4>
          <p>
            {searchTerm
              ? 'Try adjusting your search term'
              : `This department has no specializations yet. Click "Add Specialization" to create one.`}
          </p>
        </div>
      ) : (
        <>
          <div className="dept-specializations-grid">
            {paginatedSpecializations.map((specialization) => (
              <SpecializationCard
                key={specialization.id}
                specialization={specialization}
                onEdit={onEditSpecialization}
                onDelete={onDeleteSpecialization}
                onView={onViewSpecialization}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="dept-specializations-pagination">
              <button
                className="pagination-btn"
                onClick={() => handlePageChange(currentPage - 1)}
                disabled={currentPage === 1}
              >
                <ChevronLeftIcon width={16} height={16} />
                Previous
              </button>
              <div className="pagination-pages">
                {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
                  <button
                    key={page}
                    className={`page-btn ${page === currentPage ? 'active' : ''}`}
                    onClick={() => handlePageChange(page)}
                  >
                    {page}
                  </button>
                ))}
              </div>
              <button
                className="pagination-btn"
                onClick={() => handlePageChange(currentPage + 1)}
                disabled={currentPage === totalPages}
              >
                Next
                <ChevronRightIcon width={16} height={16} />
              </button>
            </div>
          )}
        </>
      )}
    </div>
  );
}