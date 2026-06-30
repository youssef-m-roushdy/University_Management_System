// pages/admin/Students/AdminStudents.tsx

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
  GraduationCapIcon,
  BookOpenIcon,
} from '../../../components/icons/Icons';
import studentService, {
  Student,
  StudentLevel,
  Gender,
} from '../../../services/studentService';
import './AdminStudents.css';

// ──────────────────────────────────────────────────────────────────────────────
// CONSTANTS
// ──────────────────────────────────────────────────────────────────────────────

const LEVEL_OPTIONS: StudentLevel[] = [
  'Preparatory_Year',
  'First_Year',
  'Second_Year',
  'Third_Year',
  'Fourth_Year',
  'Graduate',
];

const GENDER_OPTIONS: Gender[] = ['Male', 'Female'];

const LEVEL_COLORS: Record<StudentLevel, string> = {
  Preparatory_Year: '#8B5CF6',
  First_Year: '#3B82F6',
  Second_Year: '#22C55E',
  Third_Year: '#F59E0B',
  Fourth_Year: '#EF4444',
  Graduate: '#8B5CF6',
};

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENTS
// ──────────────────────────────────────────────────────────────────────────────

const StudentCard: React.FC<{
  student: Student;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
  onView: (id: string) => void;
}> = ({ student, onEdit, onDelete, onView }) => {
  return (
    <div className="student-card">
      <div className="student-card-header">
        <div className="student-avatar">
          {student.profilePicture ? (
            <img src={student.profilePicture} alt={student.name} />
          ) : (
            <span>{student.name.charAt(0)}</span>
          )}
        </div>
        <div
          className="student-level-badge"
          style={{
            background: LEVEL_COLORS[student.level] + '20',
            color: LEVEL_COLORS[student.level],
          }}
        >
          {student.level.replace('_', ' ')}
        </div>
      </div>

      <h3 className="student-card-name">{student.name}</h3>
      <p className="student-card-code">{student.academicCode}</p>

      <div className="student-card-details">
        <div className="student-detail-item">
          <GraduationCapIcon width={14} height={14} />
          <span>GPA: {student.totalGPA.toFixed(2)}</span>
        </div>
        <div className="student-detail-item">
          <BookOpenIcon width={14} height={14} />
          <span>{student.totalCredits} Credits</span>
        </div>
        <div className="student-detail-item">
          <UserIcon width={14} height={14} />
          <span>{student.gender}</span>
        </div>
      </div>

      <div className="student-card-footer">
        <div className="student-department">
          <span className="label">Department</span>
          <span className="value">{student.department}</span>
        </div>
        <div className="student-specialization">
          <span className="label">Specialization</span>
          <span className="value">{student.specialization || 'N/A'}</span>
        </div>
      </div>

      <div className="student-card-actions">
        <button
          className="student-action-btn view"
          onClick={() => onView(student.id)}
        >
          <EyeIcon width={16} height={16} />
          View
        </button>
        <button
          className="student-action-btn edit"
          onClick={() => onEdit(student.id)}
        >
          <EditIcon width={16} height={16} />
          Edit
        </button>
        <button
          className="student-action-btn delete"
          onClick={() => onDelete(student.id)}
        >
          <TrashIcon width={16} height={16} />
          Delete
        </button>
      </div>
    </div>
  );
};

const StudentModal: React.FC<{
  isOpen: boolean;
  onClose: () => void;
  onSave: (data: any) => void;
  initialData?: Student | null;
  isEditing: boolean;
}> = ({ isOpen, onClose, onSave, initialData, isEditing }) => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    userName: '',
    phoneNumber: '',
    address: '',
    gender: 'Male' as Gender,
    academicCode: '',
    level: 'First_Year' as StudentLevel,
    totalCredits: 0,
    allowedCredits: 18,
    totalGPA: 0,
    departmentId: 0,
    specializationId: 0,
    password: '',
    isActive: true,
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (initialData && isEditing) {
      setFormData({
        name: initialData.name || '',
        email: '',
        userName: initialData.userName || '',
        phoneNumber: initialData.phoneNumber || '',
        address: initialData.address || '',
        gender: initialData.gender || 'Male',
        academicCode: initialData.academicCode || '',
        level: initialData.level || 'First_Year',
        totalCredits: initialData.totalCredits || 0,
        allowedCredits: initialData.allowedCredits || 18,
        totalGPA: initialData.totalGPA || 0,
        departmentId: 0,
        specializationId: 0,
        password: '',
        isActive: true,
      });
    } else {
      setFormData({
        name: '',
        email: '',
        userName: '',
        phoneNumber: '',
        address: '',
        gender: 'Male',
        academicCode: '',
        level: 'First_Year',
        totalCredits: 0,
        allowedCredits: 18,
        totalGPA: 0,
        departmentId: 0,
        specializationId: 0,
        password: '',
        isActive: true,
      });
    }
    setErrors({});
  }, [initialData, isEditing, isOpen]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const newErrors: Record<string, string> = {};
    if (!formData.name.trim()) newErrors.name = 'Name is required';
    if (!isEditing && !formData.email.trim())
      newErrors.email = 'Email is required';
    if (!formData.userName.trim()) newErrors.userName = 'Username is required';
    if (!formData.academicCode.trim())
      newErrors.academicCode = 'Academic code is required';
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
      // Remove password for updates
      const { password, email, ...updateData } = submitData;
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
          <h2>{isEditing ? 'Edit Student' : 'Create Student'}</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="student-name">Full Name *</label>
              <input
                id="student-name"
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
              <label htmlFor="student-email">Email *</label>
              <input
                id="student-email"
                type="email"
                value={formData.email}
                onChange={e =>
                  setFormData({ ...formData, email: e.target.value })
                }
                placeholder="student@example.com"
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
              <label htmlFor="student-username">Username *</label>
              <input
                id="student-username"
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
              <label htmlFor="student-academic-code">Academic Code *</label>
              <input
                id="student-academic-code"
                type="text"
                value={formData.academicCode}
                onChange={e =>
                  setFormData({ ...formData, academicCode: e.target.value })
                }
                placeholder="2024-001"
                className={errors.academicCode ? 'error' : ''}
              />
              {errors.academicCode && (
                <span className="form-error">{errors.academicCode}</span>
              )}
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="student-gender">Gender</label>
              <select
                id="student-gender"
                value={formData.gender}
                onChange={e =>
                  setFormData({ ...formData, gender: e.target.value as Gender })
                }
              >
                {GENDER_OPTIONS.map(opt => (
                  <option key={opt} value={opt}>
                    {opt}
                  </option>
                ))}
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="student-level">Level *</label>
              <select
                id="student-level"
                value={formData.level}
                onChange={e =>
                  setFormData({
                    ...formData,
                    level: e.target.value as StudentLevel,
                  })
                }
              >
                {LEVEL_OPTIONS.map(opt => (
                  <option key={opt} value={opt}>
                    {opt.replace('_', ' ')}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="student-phone">Phone Number</label>
              <input
                id="student-phone"
                type="tel"
                value={formData.phoneNumber}
                onChange={e =>
                  setFormData({ ...formData, phoneNumber: e.target.value })
                }
                placeholder="+1234567890"
              />
            </div>

            <div className="form-group">
              <label htmlFor="student-address">Address</label>
              <input
                id="student-address"
                type="text"
                value={formData.address}
                onChange={e =>
                  setFormData({ ...formData, address: e.target.value })
                }
                placeholder="123 Main St, City"
              />
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="student-total-credits">Total Credits</label>
              <input
                id="student-total-credits"
                type="number"
                value={formData.totalCredits}
                onChange={e =>
                  setFormData({
                    ...formData,
                    totalCredits: parseInt(e.target.value) || 0,
                  })
                }
                min={0}
              />
            </div>

            <div className="form-group">
              <label htmlFor="student-allowed-credits">Allowed Credits</label>
              <input
                id="student-allowed-credits"
                type="number"
                value={formData.allowedCredits}
                onChange={e =>
                  setFormData({
                    ...formData,
                    allowedCredits: parseInt(e.target.value) || 0,
                  })
                }
                min={0}
              />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="student-gpa">GPA</label>
            <input
              id="student-gpa"
              type="number"
              step="0.01"
              value={formData.totalGPA}
              onChange={e =>
                setFormData({
                  ...formData,
                  totalGPA: parseFloat(e.target.value) || 0,
                })
              }
              min={0}
              max={4}
            />
          </div>

          {!isEditing && (
            <div className="form-group">
              <label htmlFor="student-password">Password *</label>
              <input
                id="student-password"
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
              {isEditing ? 'Update' : 'Create'} Student
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
  studentName: string;
}> = ({ isOpen, onClose, onConfirm, studentName }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        className="modal-content confirm-dialog"
        onClick={e => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2>Delete Student</h2>
          <button className="modal-close-btn" onClick={onClose}>
            <XIcon width={20} height={20} />
          </button>
        </div>
        <div className="confirm-body">
          <p>
            Are you sure you want to delete <strong>"{studentName}"</strong>?
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

export default function AdminStudents() {
  const navigate = useNavigate();
  const [students, setStudents] = useState<Student[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Pagination
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(12);
  const [totalCount, setTotalCount] = useState(0);

  // Search & Filter
  const [searchTerm, setSearchTerm] = useState('');
  const [filterLevel, setFilterLevel] = useState<'All' | StudentLevel>('All');
  const [filterGender, setFilterGender] = useState<'All' | Gender>('All');
  const [filterGraduated, setFilterGraduated] = useState<
    'All' | 'Graduated' | 'Active'
  >('All');

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingStudent, setEditingStudent] = useState<Student | null>(null);
  const [isEditing, setIsEditing] = useState(false);

  // Confirm dialog
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [deletingStudentId, setDeletingStudentId] = useState<string | null>(
    null
  );
  const [deletingStudentName, setDeletingStudentName] = useState('');

  // ────────────────────────────────────────────────────────────────────────────
  // DATA FETCHING
  // ────────────────────────────────────────────────────────────────────────────

  const loadStudents = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);

      const params: any = {
        PageNumber: currentPage,
        PageSize: pageSize,
      };

      if (searchTerm) params.SearchTerm = searchTerm;
      if (filterLevel !== 'All') params.Level = filterLevel;
      if (filterGender !== 'All') params.Gender = filterGender;
      if (filterGraduated === 'Graduated') params.IsGraduated = true;
      if (filterGraduated === 'Active') params.IsGraduated = false;

      const response = await studentService.getAllWithFilters(params);
      const data = response.data || [];
      setStudents(data);
      setTotalCount(response.pagination?.totalCount || data.length);
    } catch (err) {
      setError('Failed to load students. Please try again.');
      console.error('Error loading students:', err);
    } finally {
      setLoading(false);
    }
  }, [
    currentPage,
    pageSize,
    searchTerm,
    filterLevel,
    filterGender,
    filterGraduated,
  ]);

  useEffect(() => {
    loadStudents();
  }, [loadStudents]);

  // ────────────────────────────────────────────────────────────────────────────
  // CRUD OPERATIONS
  // ────────────────────────────────────────────────────────────────────────────

  const handleCreate = async (data: any) => {
    try {
      await studentService.create(data);
      await loadStudents();
    } catch (err) {
      console.error('Error creating student:', err);
      alert('Failed to create student. Please try again.');
    }
  };

  const handleUpdate = async (data: any) => {
    if (!editingStudent) return;
    try {
      await studentService.update(editingStudent.id, data);
      await loadStudents();
      setEditingStudent(null);
    } catch (err) {
      console.error('Error updating student:', err);
      alert('Failed to update student. Please try again.');
    }
  };

  const handleDelete = async () => {
    if (!deletingStudentId) return;
    try {
      await studentService.delete(deletingStudentId);
      await loadStudents();
      setIsConfirmOpen(false);
      setDeletingStudentId(null);
      setDeletingStudentName('');
    } catch (err) {
      console.error('Error deleting student:', err);
      alert('Failed to delete student. Please try again.');
    }
  };

  const openCreateModal = () => {
    setEditingStudent(null);
    setIsEditing(false);
    setIsModalOpen(true);
  };

  const openEditModal = (id: string) => {
    const student = students.find(s => s.id === id);
    if (student) {
      setEditingStudent(student);
      setIsEditing(true);
      setIsModalOpen(true);
    }
  };

  const openDeleteConfirm = (id: string) => {
    const student = students.find(s => s.id === id);
    if (student) {
      setDeletingStudentId(id);
      setDeletingStudentName(student.name);
      setIsConfirmOpen(true);
    }
  };

  const handleViewStudent = (id: string) => {
    navigate(`/admin/students/${id}`);
  };

  // ────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ────────────────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="students-loading">
        <div className="spinner"></div>
        <p>Loading students...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="students-error">
        <p>{error}</p>
        <button className="btn-primary" onClick={loadStudents}>
          Retry
        </button>
      </div>
    );
  }

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <div className="students-page">
      {/* Header */}
      <div className="students-header">
        <div className="students-header-text">
          <h1>Students</h1>
          <p>Manage student records</p>
        </div>
        <div className="students-header-actions">
          <div className="search-box">
            <SearchIcon width={18} height={18} />
            <input
              type="text"
              placeholder="Search students..."
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
            />
          </div>
          <button className="btn-primary" onClick={openCreateModal}>
            <PlusIcon width={18} height={18} />
            Add Student
          </button>
        </div>
      </div>

      {/* Filters */}
      <div className="students-filters">
        <div className="filter-group">
          <label>Level:</label>
          <select
            value={filterLevel}
            onChange={e => setFilterLevel(e.target.value as any)}
          >
            <option value="All">All</option>
            {LEVEL_OPTIONS.map(level => (
              <option key={level} value={level}>
                {level.replace('_', ' ')}
              </option>
            ))}
          </select>
        </div>
        <div className="filter-group">
          <label>Gender:</label>
          <select
            value={filterGender}
            onChange={e => setFilterGender(e.target.value as any)}
          >
            <option value="All">All</option>
            {GENDER_OPTIONS.map(gender => (
              <option key={gender} value={gender}>
                {gender}
              </option>
            ))}
          </select>
        </div>
        <div className="filter-group">
          <label>Status:</label>
          <select
            value={filterGraduated}
            onChange={e => setFilterGraduated(e.target.value as any)}
          >
            <option value="All">All</option>
            <option value="Active">Active</option>
            <option value="Graduated">Graduated</option>
          </select>
        </div>
        <div className="students-stats">
          <span>Total: {totalCount}</span>
          <span>Showing: {students.length}</span>
        </div>
      </div>

      {/* Student Grid */}
      {students.length === 0 ? (
        <div className="students-empty">
          <p>No students found</p>
          {searchTerm && <p>Try adjusting your search or filters</p>}
        </div>
      ) : (
        <>
          <div className="students-grid">
            {students.map(student => (
              <StudentCard
                key={student.id}
                student={student}
                onEdit={openEditModal}
                onDelete={openDeleteConfirm}
                onView={handleViewStudent}
              />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="students-pagination">
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
      <StudentModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setEditingStudent(null);
        }}
        onSave={isEditing ? handleUpdate : handleCreate}
        initialData={editingStudent}
        isEditing={isEditing}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => {
          setIsConfirmOpen(false);
          setDeletingStudentId(null);
          setDeletingStudentName('');
        }}
        onConfirm={handleDelete}
        studentName={deletingStudentName}
      />
    </div>
  );
}
