import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../../contexts/AuthContext';
import {
  FiBook,
  FiUsers,
  FiCalendar,
  FiTrendingUp,
  FiAward,
  FiCheckCircle,
  FiAlertCircle,
  FiArrowRight,
  FiUser,
  FiDollarSign,
  FiClipboard,
  FiSettings,
  FiPlus,
  FiEye,
  FiEdit,
  FiTrash2,
} from 'react-icons/fi';
import { ROUTES } from '../../../constants';
import './AdminDashboard.css';

export default function AdminDashboard() {
  const { user } = useAuth();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => setLoading(false), 1000);
    return () => clearTimeout(timer);
  }, []);

  const getGreeting = () => {
    const hour = new Date().getHours();
    if (hour < 12) return 'Good Morning';
    if (hour < 17) return 'Good Afternoon';
    return 'Good Evening';
  };

  if (loading) {
    return (
      <div className="admin-dashboard">
        <div className="loading-container">
          <div className="loading-spinner"></div>
          <p>Loading dashboard...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="admin-dashboard">
      <div className="dashboard-header">
        <div className="header-left">
          <h1>
            {getGreeting()}, Admin {user?.displayName || user?.name || 'Admin'}!
            👋
          </h1>
          <p className="header-subtitle">
            Welcome to the administration panel. Manage your university system.
          </p>
        </div>
        <div className="header-right">
          <Link to={ROUTES.ADMIN.DEPARTMENTS} className="action-btn primary">
            <FiPlus /> Manage Departments
          </Link>
          <Link to={ROUTES.STUDENT.PROFILE} className="profile-btn">
            <FiUser />
            <span>Profile</span>
          </Link>
        </div>
      </div>

      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-icon students">
            <FiUsers />
          </div>
          <div className="stat-info">
            <span className="stat-value">1,284</span>
            <span className="stat-label">Total Students</span>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon courses">
            <FiBook />
          </div>
          <div className="stat-info">
            <span className="stat-value">156</span>
            <span className="stat-label">Total Courses</span>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon departments">
            <FiClipboard />
          </div>
          <div className="stat-info">
            <span className="stat-value">12</span>
            <span className="stat-label">Departments</span>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon revenue">
            <FiDollarSign />
          </div>
          <div className="stat-info">
            <span className="stat-value">$2.4M</span>
            <span className="stat-label">Total Revenue</span>
          </div>
        </div>
      </div>

      <div className="dashboard-grid">
        <div className="dashboard-left">
          <div className="dashboard-section">
            <div className="section-header">
              <h2>
                <FiTrendingUp className="section-icon" /> Recent Activity
              </h2>
              <Link to="/admin/activity" className="view-all-link">
                View All <FiArrowRight />
              </Link>
            </div>
            <div className="activity-list">
              <div className="activity-item">
                <div className="activity-icon">📝</div>
                <div className="activity-info">
                  <span className="activity-title">
                    New course added: Advanced Algorithms
                  </span>
                  <span className="activity-time">2 hours ago</span>
                </div>
              </div>
              <div className="activity-item">
                <div className="activity-icon">👤</div>
                <div className="activity-info">
                  <span className="activity-title">
                    5 new students registered
                  </span>
                  <span className="activity-time">5 hours ago</span>
                </div>
              </div>
              <div className="activity-item">
                <div className="activity-icon">📊</div>
                <div className="activity-info">
                  <span className="activity-title">
                    Semester GPA reports generated
                  </span>
                  <span className="activity-time">1 day ago</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="dashboard-right">
          <div className="dashboard-section">
            <div className="section-header">
              <h2>
                <FiSettings className="section-icon" /> Quick Actions
              </h2>
            </div>
            <div className="quick-actions">
              <Link to={ROUTES.ADMIN.STUDENTS} className="quick-action">
                <FiUsers /> Manage Students
              </Link>
              <Link to={ROUTES.ADMIN.COURSES} className="quick-action">
                <FiBook /> Manage Courses
              </Link>
              <Link to={ROUTES.ADMIN.DEPARTMENTS} className="quick-action">
                <FiClipboard /> Manage Departments
              </Link>
              <Link to={ROUTES.ADMIN.STUDY_YEARS} className="quick-action">
                <FiCalendar /> Manage Study Years
              </Link>
              <Link to={ROUTES.ADMIN.USERS} className="quick-action">
                <FiUsers /> Manage Users
              </Link>
              <Link to={ROUTES.ADMIN.PROMOTE_STUDENTS} className="quick-action">
                <FiTrendingUp /> Promote Students
              </Link>
            </div>
          </div>

          <div className="dashboard-section">
            <div className="section-header">
              <h2>
                <FiAlertCircle className="section-icon" /> Pending Tasks
              </h2>
            </div>
            <div className="pending-list">
              <div className="pending-item">
                <span className="pending-count">15</span>
                <span className="pending-label">
                  Pending student registrations
                </span>
              </div>
              <div className="pending-item">
                <span className="pending-count">8</span>
                <span className="pending-label">Course approval requests</span>
              </div>
              <div className="pending-item">
                <span className="pending-count">3</span>
                <span className="pending-label">Department budget reviews</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
