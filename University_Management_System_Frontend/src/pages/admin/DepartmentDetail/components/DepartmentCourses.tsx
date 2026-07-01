// pages/admin/DepartmentDetail/components/DepartmentCourses.tsx

import React, { useState, useEffect, useMemo, useCallback } from 'react';
import {
  PlusIcon,
  SearchIcon,
  EditIcon,
  TrashIcon,
  EyeIcon,
  XIcon,
  BookIcon,
  ChevronLeftIcon,
  ChevronRightIcon,
  FilterIcon,
  ChevronDownIcon,
  ChevronUpIcon,
  CheckCircleIcon,
  XCircleIcon,
  PlusCircleIcon,
} from '../../../../components/icons/Icons';
import departmentCourseService from '../../../../services/departmentCourseService';
import coursePrerequisiteService from '../../../../services/coursePrerequisiteService';
import './DepartmentCourses.css';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface DepartmentCourse {
  id: number;
  departmentId: number;
  departmentName: string;
  departmentCode: string;
  courseId: number;
  courseCode: string;
  courseName: string;
  credits: number;
  role: 'Major' | 'Minor' | 'Elective';
  createdAt: string;
  updatedAt: string;
}

interface PrerequisiteCourse {
  id: number;
  code: string;
  name: string;
  description: string;
  credits: number;
  status: 'Opened' | 'Closed';
  departmentId: number;
  departmentName: string;
  createdAt: string;
  updatedAt: string;
  prerequisitesCount: number;
  dependenciesCount: number;
}

interface DepartmentCoursesProps {
  departmentId: number;
  departmentName: string;
  loading?: boolean;
  onAddCourse: () => void;
  onEditCourse: (id: number) => void;
  onDeleteCourse: (id: number) => void;
  onViewCourse: (id: number) => void;
  onAddPrerequisite: (courseId: number) => void;
  onAddDependency: (courseId: number) => void;
}

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const CourseCard: React.FC<{
  departmentCourse: DepartmentCourse;
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
  onView: (id: number) => void;
  onAddPrerequisite: (id: number) => void;
  onAddDependency: (id: number) => void;
}> = ({
  departmentCourse,
  onEdit,
  onDelete,
  onView,
  onAddPrerequisite,
  onAddDependency,
}) => {
  const [showPrerequisites, setShowPrerequisites] = useState(false);
  const [showDependencies, setShowDependencies] = useState(false);
  const [prerequisites, setPrerequisites] = useState<PrerequisiteCourse[]>([]);
  const [dependencies, setDependencies] = useState<PrerequisiteCourse[]>([]);
  const [loadingPrereqs, setLoadingPrereqs] = useState(false);
  const [loadingDeps, setLoadingDeps] = useState(false);
  const [prereqCount, setPrereqCount] = useState(0);
  const [depCount, setDepCount] = useState(0);

  // Load prerequisites
  const loadPrerequisites = useCallback(async () => {
    if (!showPrerequisites) return;
    try {
      setLoadingPrereqs(true);
      const response = await coursePrerequisiteService.getPrerequisites(
        departmentCourse.courseId
      );
      const data = response.data || [];
      setPrerequisites(data);
      setPrereqCount(data.length);
    } catch (error) {
      console.error('Error loading prerequisites:', error);
    } finally {
      setLoadingPrereqs(false);
    }
  }, [departmentCourse.courseId, showPrerequisites]);

  // Load dependencies
  const loadDependencies = useCallback(async () => {
    if (!showDependencies) return;
    try {
      setLoadingDeps(true);
      const response = await coursePrerequisiteService.getDependencies(
        departmentCourse.courseId
      );
      const data = response.data || [];
      setDependencies(data);
      setDepCount(data.length);
    } catch (error) {
      console.error('Error loading dependencies:', error);
    } finally {
      setLoadingDeps(false);
    }
  }, [departmentCourse.courseId, showDependencies]);

  useEffect(() => {
    if (showPrerequisites) {
      loadPrerequisites();
    }
  }, [showPrerequisites, loadPrerequisites]);

  useEffect(() => {
    if (showDependencies) {
      loadDependencies();
    }
  }, [showDependencies, loadDependencies]);

  const getRoleBadgeClass = (role: string) => {
    switch (role) {
      case 'Major':
        return 'role-major';
      case 'Minor':
        return 'role-minor';
      case 'Elective':
        return 'role-elective';
      default:
        return '';
    }
  };

  return (
    <div className="dept-course-card">
      <div className="dept-course-card-header">
        <div className="dept-course-code-wrapper">
          <span className="dept-course-code">
            {departmentCourse.courseCode}
          </span>
          <span className="dept-course-credits-badge">
            {departmentCourse.credits} CR
          </span>
          <span
            className={`dept-course-role ${getRoleBadgeClass(departmentCourse.role)}`}
          >
            {departmentCourse.role}
          </span>
        </div>
      </div>

      <h4 className="dept-course-name">{departmentCourse.courseName}</h4>

      <div className="dept-course-stats">
        <div className="dept-course-stat">
          <span className="stat-label">Prerequisites</span>
          <span className="stat-value">{prereqCount}</span>
        </div>
        <div className="dept-course-stat">
          <span className="stat-label">Dependencies</span>
          <span className="stat-value">{depCount}</span>
        </div>
        <div className="dept-course-stat">
          <span className="stat-label">Role</span>
          <span className="stat-value">{departmentCourse.role}</span>
        </div>
      </div>

      {/* Prerequisites Section */}
      <div className="dept-course-prerequisites-section">
        <button
          className="dept-course-prereq-toggle"
          onClick={() => setShowPrerequisites(!showPrerequisites)}
        >
          {showPrerequisites ? (
            <ChevronUpIcon width={14} height={14} />
          ) : (
            <ChevronDownIcon width={14} height={14} />
          )}
          {showPrerequisites ? 'Hide' : 'View'} Prerequisites
          {prereqCount > 0 && (
            <span className="prereq-count">{prereqCount}</span>
          )}
        </button>

        {showPrerequisites && (
          <div className="dept-course-prereq-list">
            {loadingPrereqs ? (
              <div className="prereq-loading">Loading prerequisites...</div>
            ) : prerequisites.length > 0 ? (
              prerequisites.map(prereq => (
                <div key={prereq.id} className="dept-course-prereq-item">
                  <span className="prereq-code">{prereq.code}</span>
                  <span className="prereq-name">{prereq.name}</span>
                  <span className="prereq-status">
                    {prereq.status === 'Opened' ? (
                      <CheckCircleIcon width={12} height={12} />
                    ) : (
                      <XCircleIcon width={12} height={12} />
                    )}
                  </span>
                  <button
                    className="prereq-remove-btn"
                    onClick={() =>
                      coursePrerequisiteService.removePrerequisite(
                        departmentCourse.courseId,
                        prereq.id
                      )
                    }
                    title="Remove prerequisite"
                  >
                    <XIcon width={12} height={12} />
                  </button>
                </div>
              ))
            ) : (
              <div className="prereq-empty">No prerequisites found</div>
            )}
            <button
              className="prereq-add-btn"
              onClick={() => onAddPrerequisite(departmentCourse.courseId)}
            >
              <PlusCircleIcon width={14} height={14} />
              Add Prerequisite
            </button>
          </div>
        )}
      </div>

      {/* Dependencies Section */}
      <div className="dept-course-prerequisites-section">
        <button
          className="dept-course-prereq-toggle"
          onClick={() => setShowDependencies(!showDependencies)}
        >
          {showDependencies ? (
            <ChevronUpIcon width={14} height={14} />
          ) : (
            <ChevronDownIcon width={14} height={14} />
          )}
          {showDependencies ? 'Hide' : 'View'} Dependencies
          {depCount > 0 && <span className="prereq-count">{depCount}</span>}
        </button>

        {showDependencies && (
          <div className="dept-course-prereq-list">
            {loadingDeps ? (
              <div className="prereq-loading">Loading dependencies...</div>
            ) : dependencies.length > 0 ? (
              dependencies.map(dep => (
                <div key={dep.id} className="dept-course-prereq-item">
                  <span className="prereq-code">{dep.code}</span>
                  <span className="prereq-name">{dep.name}</span>
                  <span className="prereq-status">
                    {dep.status === 'Opened' ? (
                      <CheckCircleIcon width={12} height={12} />
                    ) : (
                      <XCircleIcon width={12} height={12} />
                    )}
                  </span>
                  <button
                    className="prereq-remove-btn"
                    onClick={() =>
                      coursePrerequisiteService.removeDependency(
                        departmentCourse.courseId,
                        dep.id
                      )
                    }
                    title="Remove dependency"
                  >
                    <XIcon width={12} height={12} />
                  </button>
                </div>
              ))
            ) : (
              <div className="prereq-empty">No dependencies found</div>
            )}
            <button
              className="prereq-add-btn"
              onClick={() => onAddDependency(departmentCourse.courseId)}
            >
              <PlusCircleIcon width={14} height={14} />
              Add Dependency
            </button>
          </div>
        )}
      </div>

      {/* Actions */}
      <div className="dept-course-actions">
        <button
          className="dept-course-action-btn view"
          onClick={() => onView(departmentCourse.id)}
        >
          <EyeIcon width={15} height={15} />
          View
        </button>
        <button
          className="dept-course-action-btn edit"
          onClick={() => onEdit(departmentCourse.id)}
        >
          <EditIcon width={15} height={15} />
          Edit
        </button>
        <button
          className="dept-course-action-btn delete"
          onClick={() => onDelete(departmentCourse.id)}
        >
          <TrashIcon width={15} height={15} />
          Delete
        </button>
      </div>
    </div>
  );
};

const CourseCardSkeleton: React.FC = () => {
  return (
    <div className="dept-course-card skeleton">
      <div className="dept-course-card-header">
        <div className="skeleton-code"></div>
        <div className="skeleton-badge"></div>
      </div>
      <div className="skeleton-title"></div>
      <div className="dept-course-stats">
        <div className="skeleton-stat"></div>
        <div className="skeleton-stat"></div>
        <div className="skeleton-stat"></div>
      </div>
      <div className="skeleton-prereq-toggle"></div>
      <div className="dept-course-actions">
        <div className="skeleton-action"></div>
        <div className="skeleton-action"></div>
        <div className="skeleton-action"></div>
      </div>
    </div>
  );
};

// ──────────────────────────────────────────────────────────────────────────────
// MAIN COMPONENT
// ──────────────────────────────────────────────────────────────────────────────

export default function DepartmentCourses({
  departmentId,
  departmentName,
  loading = false,
  onAddCourse,
  onEditCourse,
  onDeleteCourse,
  onViewCourse,
  onAddPrerequisite,
  onAddDependency,
}: DepartmentCoursesProps) {
  const [allCourses, setAllCourses] = useState<DepartmentCourse[]>([]);
  const [displayCourses, setDisplayCourses] = useState<DepartmentCourse[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterRole, setFilterRole] = useState<
    'All' | 'Major' | 'Minor' | 'Elective'
  >('All');
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);
  const [isLoading, setIsLoading] = useState(loading);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(1);

  // ─── Load Department Courses ──────────────────────────────────────────────

  const loadDepartmentCourses = useCallback(async () => {
    if (!departmentId) return;
    try {
      setIsLoading(true);

      // Fetch all courses with a large page size to get everything
      const response = await departmentCourseService.getByDepartmentWithFilters(
        departmentId,
        {
          PageSize: 1000, // Get all courses at once
          PageNumber: 1,
        },
        1,
        1000
      );

      const data = response.data || [];
      setAllCourses(data);
      setDisplayCourses(data);
      setTotalCount(response.pagination?.totalCount || data.length);
      setTotalPages(
        Math.ceil(
          (response.pagination?.totalCount || data.length) / itemsPerPage
        )
      );
    } catch (error) {
      console.error('Error loading department courses:', error);
    } finally {
      setIsLoading(false);
    }
  }, [departmentId, itemsPerPage]);

  useEffect(() => {
    loadDepartmentCourses();
  }, [loadDepartmentCourses]);

  // ─── Filtering ─────────────────────────────────────────────────────────────

  useEffect(() => {
    let result = allCourses;

    if (searchTerm) {
      const term = searchTerm.toLowerCase();
      result = result.filter(
        c =>
          c.courseName.toLowerCase().includes(term) ||
          c.courseCode.toLowerCase().includes(term)
      );
    }

    if (filterRole !== 'All') {
      result = result.filter(c => c.role === filterRole);
    }

    setDisplayCourses(result);
    setTotalCount(result.length);
    setTotalPages(Math.ceil(result.length / itemsPerPage));
    setCurrentPage(1);
  }, [allCourses, searchTerm, filterRole, itemsPerPage]);

  // ─── Pagination ────────────────────────────────────────────────────────────

  const paginatedCourses = useMemo(() => {
    const start = (currentPage - 1) * itemsPerPage;
    return displayCourses.slice(start, start + itemsPerPage);
  }, [displayCourses, currentPage, itemsPerPage]);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  // ─── Statistics ────────────────────────────────────────────────────────────

  const stats = {
    total: allCourses.length,
    major: allCourses.filter(c => c.role === 'Major').length,
    minor: allCourses.filter(c => c.role === 'Minor').length,
    elective: allCourses.filter(c => c.role === 'Elective').length,
  };

  // ─── Render ────────────────────────────────────────────────────────────────

  if (isLoading) {
    return (
      <div className="dept-courses">
        <div className="dept-courses-header">
          <div className="dept-courses-title">
            <BookIcon width={22} height={22} />
            <h3>Department Courses</h3>
            <span className="dept-courses-count">Loading...</span>
          </div>
          <div className="dept-courses-toolbar">
            <div className="search-box skeleton-search"></div>
            <div className="skeleton-add-btn"></div>
          </div>
        </div>
        <div className="dept-courses-grid">
          {[1, 2, 3, 4, 5, 6].map(i => (
            <CourseCardSkeleton key={i} />
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className="dept-courses">
      {/* Header */}
      <div className="dept-courses-header">
        <div className="dept-courses-title">
          <BookIcon width={22} height={22} />
          <h3>Department Courses</h3>
          <span className="dept-courses-count">{allCourses.length} total</span>
        </div>
        <div className="dept-courses-toolbar">
          <div className="search-box">
            <SearchIcon width={16} height={16} />
            <input
              type="text"
              placeholder="Search courses..."
              value={searchTerm}
              onChange={e => {
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
          <div className="filter-group">
            <FilterIcon width={16} height={16} />
            <select
              value={filterRole}
              onChange={e => {
                setFilterRole(e.target.value as any);
                setCurrentPage(1);
              }}
            >
              <option value="All">All Roles</option>
              <option value="Major">Major</option>
              <option value="Minor">Minor</option>
              <option value="Elective">Elective</option>
            </select>
          </div>
          <button className="btn-primary add-btn" onClick={onAddCourse}>
            <PlusIcon width={16} height={16} />
            Assign Course
          </button>
        </div>
      </div>

      {/* Stats Bar */}
      <div className="dept-courses-stats-bar">
        <div className="stat-item">
          <span className="stat-label">Total</span>
          <span className="stat-number">{stats.total}</span>
        </div>
        <div className="stat-divider"></div>
        <div className="stat-item">
          <span className="stat-label major">Major</span>
          <span className="stat-number">{stats.major}</span>
        </div>
        <div className="stat-divider"></div>
        <div className="stat-item">
          <span className="stat-label minor">Minor</span>
          <span className="stat-number">{stats.minor}</span>
        </div>
        <div className="stat-divider"></div>
        <div className="stat-item">
          <span className="stat-label elective">Elective</span>
          <span className="stat-number">{stats.elective}</span>
        </div>
        <div className="stat-divider"></div>
        <div className="stat-item">
          <span className="stat-label">Showing</span>
          <span className="stat-number">{displayCourses.length}</span>
        </div>
      </div>

      {/* Courses Grid */}
      {paginatedCourses.length === 0 ? (
        <div className="dept-courses-empty">
          <BookIcon width={56} height={56} />
          <h4>No courses found</h4>
          <p>
            {searchTerm || filterRole !== 'All'
              ? 'Try adjusting your search or filters'
              : `This department has no courses assigned yet. Click "Assign Course" to add one.`}
          </p>
        </div>
      ) : (
        <>
          <div className="dept-courses-grid">
            {paginatedCourses.map(deptCourse => (
              <CourseCard
                key={deptCourse.id}
                departmentCourse={deptCourse}
                onEdit={onEditCourse}
                onDelete={onDeleteCourse}
                onView={onViewCourse}
                onAddPrerequisite={onAddPrerequisite}
                onAddDependency={onAddDependency}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="dept-courses-pagination">
              <button
                className="pagination-btn"
                onClick={() => handlePageChange(currentPage - 1)}
                disabled={currentPage === 1}
              >
                <ChevronLeftIcon width={16} height={16} />
                Previous
              </button>
              <div className="pagination-pages">
                {Array.from({ length: totalPages }, (_, i) => i + 1).map(
                  page => (
                    <button
                      key={page}
                      className={`page-btn ${page === currentPage ? 'active' : ''}`}
                      onClick={() => handlePageChange(page)}
                    >
                      {page}
                    </button>
                  )
                )}
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
