// pages/admin/DepartmentDetail/components/DepartmentStatistics.tsx

import React from 'react';
import { 
  BookIcon, 
  UsersIcon, 
  GraduationCapIcon,
  CalendarIcon,
  TrendingUpIcon,
  UserPlusIcon,
  ClockIcon,
} from '../../../../components/icons/Icons';
import './DepartmentStatistics.css';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface StatisticsData {
  totalCourses: number;
  totalSpecializations: number;
  totalStudents: number;
  totalInstructors: number;
  activeCourses: number;
  averageCredits: number;
  totalCredits?: number;
  completionRate?: number;
}

interface DepartmentStatisticsProps {
  departmentName: string;
  departmentCode: string;
  description?: string;
  statistics: StatisticsData;
  loading?: boolean;
  onActionClick?: (action: string) => void;
}

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENT
// ──────────────────────────────────────────────────────────────────────────────

export default function DepartmentStatistics({
  departmentName,
  departmentCode,
  description,
  statistics,
  loading = false,
  onActionClick,
}: DepartmentStatisticsProps) {
  if (loading) {
    return <DepartmentStatisticsSkeleton />;
  }

  const stats = [
    {
      id: 'courses',
      label: 'Total Courses',
      value: statistics.totalCourses,
      icon: <BookIcon />,
      color: '#3B82F6',
      bgColor: 'rgba(59, 130, 246, 0.12)',
      trend: '+12%',
      trendUp: true,
    },
    {
      id: 'specializations',
      label: 'Specializations',
      value: statistics.totalSpecializations,
      icon: <GraduationCapIcon />,
      color: '#8B5CF6',
      bgColor: 'rgba(139, 92, 246, 0.12)',
      trend: '+4%',
      trendUp: true,
    },
    {
      id: 'students',
      label: 'Total Students',
      value: statistics.totalStudents,
      icon: <UsersIcon />,
      color: '#22C55E',
      bgColor: 'rgba(34, 197, 94, 0.12)',
      trend: '+8.5%',
      trendUp: true,
    },
    {
      id: 'instructors',
      label: 'Instructors',
      value: statistics.totalInstructors,
      icon: <UserPlusIcon />,
      color: '#F59E0B',
      bgColor: 'rgba(245, 158, 11, 0.12)',
      trend: '+2.3%',
      trendUp: true,
    },
    {
      id: 'active-courses',
      label: 'Active Courses',
      value: statistics.activeCourses,
      icon: <CalendarIcon />,
      color: '#06B6D4',
      bgColor: 'rgba(6, 182, 212, 0.12)',
      trend: '-3%',
      trendUp: false,
    },
    {
      id: 'avg-credits',
      label: 'Avg. Credits',
      value: statistics.averageCredits.toFixed(1),
      icon: <TrendingUpIcon />,
      color: '#EC4899',
      bgColor: 'rgba(236, 72, 153, 0.12)',
      trend: '+0.4',
      trendUp: true,
    },
  ];

  return (
    <div className="dept-statistics">
      {/* Header */}
      <div className="dept-statistics-header">
        <div className="dept-statistics-brand">
          <div className="dept-statistics-avatar">
            <span>{departmentName.charAt(0)}</span>
          </div>
          <div className="dept-statistics-title-group">
            <div className="dept-statistics-title">
              <h2>{departmentName}</h2>
              <span className="dept-statistics-code">{departmentCode}</span>
            </div>
            {description && (
              <p className="dept-statistics-description">{description}</p>
            )}
          </div>
        </div>
        <div className="dept-statistics-meta">
          <div className="dept-statistics-meta-item">
            <ClockIcon width={14} height={14} />
            <span>Updated just now</span>
          </div>
        </div>
      </div>

      {/* Stats Grid */}
      <div className="dept-statistics-grid">
        {stats.map((stat) => (
          <div key={stat.id} className="dept-stat-card">
            <div className="dept-stat-card-left">
              <div 
                className="dept-stat-icon"
                style={{ 
                  backgroundColor: stat.bgColor,
                  color: stat.color,
                }}
              >
                {stat.icon}
              </div>
              <div className="dept-stat-content">
                <span className="dept-stat-label">{stat.label}</span>
                <span className="dept-stat-value">{stat.value}</span>
              </div>
            </div>
            {stat.trend && (
              <div className={`dept-stat-trend ${stat.trendUp ? 'up' : 'down'}`}>
                <span>{stat.trend}</span>
              </div>
            )}
          </div>
        ))}
      </div>

      {/* Quick Actions */}
      <div className="dept-stat-actions">
        <button 
          className="dept-stat-action-btn primary"
          onClick={() => onActionClick?.('courses')}
        >
          <BookIcon width={16} height={16} />
          Manage Courses
        </button>
        <button 
          className="dept-stat-action-btn secondary"
          onClick={() => onActionClick?.('specializations')}
        >
          <GraduationCapIcon width={16} height={16} />
          Manage Specializations
        </button>
        <button 
          className="dept-stat-action-btn secondary"
          onClick={() => onActionClick?.('students')}
        >
          <UsersIcon width={16} height={16} />
          View Students
        </button>
        <button 
          className="dept-stat-action-btn secondary"
          onClick={() => onActionClick?.('instructors')}
        >
          <UserPlusIcon width={16} height={16} />
          View Instructors
        </button>
      </div>
    </div>
  );
}

// ──────────────────────────────────────────────────────────────────────────────
// SKELETON LOADING
// ──────────────────────────────────────────────────────────────────────────────

function DepartmentStatisticsSkeleton() {
  return (
    <div className="dept-stats-skeleton">
      <div className="skeleton-header">
        <div className="skeleton-avatar"></div>
        <div className="skeleton-header-content">
          <div className="skeleton-title-group">
            <div className="skeleton-line skeleton-title"></div>
            <div className="skeleton-line skeleton-code"></div>
          </div>
          <div className="skeleton-line skeleton-description"></div>
        </div>
      </div>
      <div className="skeleton-grid">
        {[1, 2, 3, 4, 5, 6].map((i) => (
          <div key={i} className="skeleton-card">
            <div className="skeleton-card-left">
              <div className="skeleton-icon"></div>
              <div className="skeleton-text-group">
                <div className="skeleton-text"></div>
                <div className="skeleton-value"></div>
              </div>
            </div>
            <div className="skeleton-trend"></div>
          </div>
        ))}
      </div>
      <div className="skeleton-actions">
        {[1, 2, 3, 4].map((i) => (
          <div key={i} className="skeleton-action-btn"></div>
        ))}
      </div>
    </div>
  );
}