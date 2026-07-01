// pages/admin/DepartmentDetail/DepartmentDetail.tsx

import React, { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  ChevronLeftIcon,
  EditIcon,
  BookIcon,
  UsersIcon,
  TrendingUpIcon,
  GraduationCapIcon,
} from '../../../components/icons/Icons';
import {
  DepartmentStatistics,
  DepartmentCourses,
  DepartmentSpecializations,
} from './components';
import departmentService, { Department } from '../../../services/departmentService';
import specializationService from '../../../services/specializationService';
import './DepartmentDetail.css';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

type TabType = 'overview' | 'courses' | 'specializations';

interface Tab {
  id: TabType;
  label: string;
  icon: React.ReactNode;
  count?: number;
}

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENT
// ──────────────────────────────────────────────────────────────────────────────

export default function DepartmentDetail() {
  const { departmentId } = useParams<{ departmentId: string }>();
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState<TabType>('overview');
  const [department, setDepartment] = useState<Department | null>(null);
  const [specializations, setSpecializations] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  const [statistics, setStatistics] = useState({
    totalCourses: 0,
    totalSpecializations: 0,
    totalStudents: 0,
    totalInstructors: 0,
    activeCourses: 0,
    averageCredits: 0,
    totalCredits: 0,
  });

  const deptId = parseInt(departmentId || '0');

  // ────────────────────────────────────────────────────────────────────────────
  // DATA FETCHING
  // ────────────────────────────────────────────────────────────────────────────

  const loadData = useCallback(async () => {
    if (!deptId) return;
    try {
      setLoading(true);
      setError(null);

      // Load department details
      const deptResponse = await departmentService.getById(deptId);
      setDepartment(deptResponse.data || null);

      // Load specializations
      const specResponse = await specializationService.getByDepartment(deptId, {
        PageSize: 100,
      });
      const specData = specResponse.data || [];
      setSpecializations(specData);

      // Update statistics
      setStatistics({
        totalCourses: 0,
        totalSpecializations: specData.length,
        totalStudents: 0,
        totalInstructors: 0,
        activeCourses: 0,
        averageCredits: 0,
        totalCredits: 0,
      });

    } catch (err) {
      setError('Failed to load department data. Please try again.');
      console.error('Error loading department:', err);
    } finally {
      setLoading(false);
    }
  }, [deptId]);

  useEffect(() => {
    loadData();
  }, [loadData]);

  // ────────────────────────────────────────────────────────────────────────────
  // HANDLERS
  // ────────────────────────────────────────────────────────────────────────────

  const handleActionClick = (action: string) => {
    switch (action) {
      case 'courses':
        setActiveTab('courses');
        break;
      case 'specializations':
        setActiveTab('specializations');
        break;
      default:
        break;
    }
  };

  // ─── Course Handlers ──────────────────────────────────────────────────────

  const handleAddCourse = () => {
    navigate(`/admin/departments/${deptId}/courses/assign`);
  };

  const handleEditCourse = (id: number) => {
    navigate(`/admin/department-courses/${id}/edit`);
  };

  const handleDeleteCourse = async (id: number) => {
    if (!window.confirm('Are you sure you want to remove this course from the department?')) return;
    try {
      // Find the department course to get courseId
      // The delete endpoint needs departmentId and courseId
      // We'll handle this in the DepartmentCourses component
      await loadData();
    } catch (err) {
      console.error('Error deleting department course:', err);
      alert('Failed to remove course from department.');
    }
  };

  const handleViewCourse = (id: number) => {
    navigate(`/admin/department-courses/${id}`);
  };

  // ─── Prerequisite Handlers ───────────────────────────────────────────────

  const handleAddPrerequisite = (courseId: number) => {
    navigate(`/admin/courses/${courseId}/prerequisites/add`);
  };

  const handleAddDependency = (courseId: number) => {
    navigate(`/admin/courses/${courseId}/dependencies/add`);
  };

  // ─── Specialization Handlers ─────────────────────────────────────────────

  const handleAddSpecialization = () => {
    navigate(`/admin/departments/${deptId}/specializations/new`);
  };

  const handleEditSpecialization = (id: number) => {
    navigate(`/admin/specializations/${id}/edit`);
  };

  const handleDeleteSpecialization = async (id: number) => {
    if (!window.confirm('Are you sure you want to delete this specialization?')) return;
    try {
      await specializationService.delete(id);
      await loadData();
    } catch (err) {
      console.error('Error deleting specialization:', err);
      alert('Failed to delete specialization.');
    }
  };

  const handleViewSpecialization = (id: number) => {
    navigate(`/admin/specializations/${id}`);
  };

  // ────────────────────────────────────────────────────────────────────────────
  // TABS
  // ────────────────────────────────────────────────────────────────────────────

  const tabs: Tab[] = [
    {
      id: 'overview',
      label: 'Overview',
      icon: <TrendingUpIcon width={18} height={18} />,
    },
    {
      id: 'courses',
      label: 'Courses',
      icon: <BookIcon width={18} height={18} />,
    },
    {
      id: 'specializations',
      label: 'Specializations',
      icon: <GraduationCapIcon width={18} height={18} />,
      count: specializations.length,
    },
  ];

  // ────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ────────────────────────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="dept-detail-loading">
        <div className="spinner"></div>
        <p>Loading department details...</p>
      </div>
    );
  }

  if (error || !department) {
    return (
      <div className="dept-detail-error">
        <p>{error || 'Department not found'}</p>
        <button className="btn-primary" onClick={() => navigate('/admin/departments')}>
          Back to Departments
        </button>
      </div>
    );
  }

  return (
    <div className="dept-detail-page">
      {/* Header */}
      <div className="dept-detail-header">
        <div className="dept-detail-header-left">
          <button
            className="back-button"
            onClick={() => navigate('/admin/departments')}
          >
            <ChevronLeftIcon width={20} height={20} />
            Back
          </button>
          <div className="dept-detail-title">
            <h1>{department.name}</h1>
            <span className="dept-detail-code">{department.code}</span>
          </div>
        </div>
        <div className="dept-detail-actions">
          <button
            className="btn-secondary"
            onClick={() => navigate(`/admin/departments/${deptId}/edit`)}
          >
            <EditIcon width={16} height={16} />
            Edit Department
          </button>
        </div>
      </div>

      {/* Description */}
      <div className="dept-detail-description">
        <p>{department.description || 'No description available.'}</p>
      </div>

      {/* Tabs */}
      <div className="dept-detail-tabs">
        {tabs.map((tab) => (
          <button
            key={tab.id}
            className={`dept-tab-btn ${activeTab === tab.id ? 'active' : ''}`}
            onClick={() => setActiveTab(tab.id)}
          >
            {tab.icon}
            {tab.label}
            {tab.count !== undefined && tab.count > 0 && (
              <span className="dept-tab-count">{tab.count}</span>
            )}
          </button>
        ))}
      </div>

      {/* Tab Content */}
      <div className="dept-detail-content">
        {activeTab === 'overview' && (
          <DepartmentStatistics
            departmentName={department.name}
            departmentCode={department.code}
            description={department.description}
            statistics={statistics}
            onActionClick={handleActionClick}
          />
        )}

        {activeTab === 'courses' && (
          <DepartmentCourses
            departmentId={deptId}
            departmentName={department.name}
            onAddCourse={handleAddCourse}
            onEditCourse={handleEditCourse}
            onDeleteCourse={handleDeleteCourse}
            onViewCourse={handleViewCourse}
            onAddPrerequisite={handleAddPrerequisite}
            onAddDependency={handleAddDependency}
          />
        )}

        {activeTab === 'specializations' && (
          <DepartmentSpecializations
            departmentId={deptId}
            departmentName={department.name}
            specializations={specializations}
            onAddSpecialization={handleAddSpecialization}
            onEditSpecialization={handleEditSpecialization}
            onDeleteSpecialization={handleDeleteSpecialization}
            onViewSpecialization={handleViewSpecialization}
          />
        )}
      </div>
    </div>
  );
}