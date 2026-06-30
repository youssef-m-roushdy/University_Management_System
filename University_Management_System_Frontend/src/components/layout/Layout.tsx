// components/layout/Layout.tsx

import React, { useState, useMemo, useCallback } from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { ROUTES, USER_ROLES, UserRole } from '../../constants';
import { getDashboardRoute } from '../../utils/roleRouting';
import RoleNavSection from './RoleNavSection';
import {
  FiHome,
  FiBook,
  FiUsers,
  FiCalendar,
  FiGrid,
  FiMenu,
  FiX,
  FiUser,
  FiShield,
  FiLayers,
  FiChevronDown,
  FiChevronRight,
  FiLock,
  FiUserPlus,
  FiSettings,
  FiSearch,
  FiBell,
  FiMail,
  FiLogOut,
} from 'react-icons/fi';
import {
  AlertIcon,
  GraduationCapIcon,
  StudentIcon,
  ClipboardListIcon,
  CpuIcon,
  BookOpenIcon,
} from '../icons/Icons';
import './Layout.css';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface NavItem {
  to: string;
  icon: React.ReactNode;
  label: string;
}

// ──────────────────────────────────────────────────────────────────────────────
// ROLE SECTION METADATA — display name + icon for each role group in sidebar
// ──────────────────────────────────────────────────────────────────────────────

const ROLE_SECTION_META: Record<
  UserRole,
  { label: string; icon: React.ReactNode }
> = {
  [USER_ROLES.STUDENT]: { label: 'Student', icon: <StudentIcon /> },
  [USER_ROLES.ADMIN]: { label: 'Admin', icon: <CpuIcon /> },
  [USER_ROLES.INSTRUCTOR]: { label: 'Instructor', icon: <BookOpenIcon /> },
  [USER_ROLES.ASSISTANT]: { label: 'Assistant', icon: <ClipboardListIcon /> },
};

// ──────────────────────────────────────────────────────────────────────────────
// NAV ITEMS — keyed by role, not in separate arrays
// Each role owns its nav items; no cross-role filtering needed.
// ──────────────────────────────────────────────────────────────────────────────

const ROLE_NAV_ITEMS: Record<UserRole, NavItem[]> = {
  // ─── Admin Navigation ──────────────────────────────────────────────────────
  // Structured: Dashboard → User Management → Academic → System
  [USER_ROLES.ADMIN]: [
    // Dashboard
    { to: ROUTES.ADMIN.DASHBOARD, icon: <FiHome />, label: 'Dashboard' },

    // User Management
    { to: ROUTES.ADMIN.USERS, icon: <FiUserPlus />, label: 'Users' },
    { to: ROUTES.ADMIN.ADMINS, icon: <FiUser />, label: 'Admins' },
    { to: ROUTES.ADMIN.STUDENTS, icon: <FiUsers />, label: 'Students' },
    { to: ROUTES.ADMIN.INSTRUCTORS, icon: <FiUser />, label: 'Instructors' },
    { to: ROUTES.ADMIN.ASSISTANTS, icon: <FiUser />, label: 'Assistants' },

    // Academic Structure
    { to: ROUTES.ADMIN.DEPARTMENTS, icon: <FiGrid />, label: 'Departments' },
    {
      to: ROUTES.ADMIN.STUDY_YEARS,
      icon: <FiCalendar />,
      label: 'Study Years',
    },

    // System
    { to: ROUTES.ADMIN.ROLES, icon: <FiShield />, label: 'Roles' },
  ],

  // ─── Student Navigation ────────────────────────────────────────────────────
  [USER_ROLES.STUDENT]: [
    { to: ROUTES.STUDENT.DASHBOARD, icon: <FiHome />, label: 'Dashboard' },
    { to: ROUTES.STUDENT.MY_COURSES, icon: <FiBook />, label: 'My Courses' },
    { to: ROUTES.STUDENT.MY_TIMELINE, icon: <FiLayers />, label: 'Timeline' },
    {
      to: ROUTES.STUDENT.MY_STUDY_YEARS,
      icon: <FiCalendar />,
      label: 'Study Years',
    },
    {
      to: ROUTES.STUDENT.DEPARTMENT_COURSES,
      icon: <FiGrid />,
      label: 'Department Courses',
    },
    { to: ROUTES.STUDENT.PROFILE, icon: <FiUser />, label: 'Profile' },
    {
      to: ROUTES.STUDENT.CHANGE_PASSWORD,
      icon: <FiLock />,
      label: 'Change Password',
    },
  ],

  // ─── Instructor Navigation ─────────────────────────────────────────────────
  [USER_ROLES.INSTRUCTOR]: [
    { to: ROUTES.INSTRUCTOR.DASHBOARD, icon: <FiHome />, label: 'Dashboard' },
    { to: ROUTES.INSTRUCTOR.COURSES, icon: <FiBook />, label: 'My Courses' },
    { to: ROUTES.INSTRUCTOR.STUDENTS, icon: <FiUsers />, label: 'Students' },
  ],

  // ─── Assistant Navigation ──────────────────────────────────────────────────
  [USER_ROLES.ASSISTANT]: [
    { to: ROUTES.ASSISTANT.DASHBOARD, icon: <FiHome />, label: 'Dashboard' },
    { to: ROUTES.ASSISTANT.COURSES, icon: <FiBook />, label: 'Courses' },
    { to: ROUTES.ASSISTANT.STUDENTS, icon: <FiUsers />, label: 'Students' },
    { to: ROUTES.ASSISTANT.GRADING, icon: <FiShield />, label: 'Grading' },
  ],
};

// ──────────────────────────────────────────────────────────────────────────────
// ROLE PRIORITY — order of sections in sidebar when user has multiple roles
// Student first (most common), then upward in responsibility
// ──────────────────────────────────────────────────────────────────────────────

const ROLE_PRIORITY: UserRole[] = [
  USER_ROLES.STUDENT,
  USER_ROLES.ASSISTANT,
  USER_ROLES.INSTRUCTOR,
  USER_ROLES.ADMIN,
];

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENT
// ──────────────────────────────────────────────────────────────────────────────

export default function Layout() {
  const { user, logout, roles, primaryRole, hasRole, isAdmin, isStudent } =
    useAuth();

  const navigate = useNavigate();
  const [sidebarOpen, setSidebarOpen] = useState<boolean>(true);
  const [profileOpen, setProfileOpen] = useState<boolean>(false);

  // ─── Handlers ──────────────────────────────────────────────────────────────

  const handleLogout = useCallback(() => {
    logout();
    navigate(ROUTES.LOGIN);
  }, [logout, navigate]);

  const toggleSidebar = useCallback(() => {
    setSidebarOpen(prev => !prev);
  }, []);

  const toggleProfile = useCallback(() => {
    setProfileOpen(prev => !prev);
  }, []);

  const closeProfile = useCallback(() => {
    setProfileOpen(false);
  }, []);

  // ─── Build role sections for sidebar ──────────────────────────────────────

  const roleSections = useMemo(() => {
    if (!roles || roles.length === 0) return [];

    const sorted = [...roles].sort((a, b) => {
      const aIdx = ROLE_PRIORITY.indexOf(a);
      const bIdx = ROLE_PRIORITY.indexOf(b);
      return aIdx - bIdx;
    });

    return sorted
      .filter(role => ROLE_NAV_ITEMS[role]?.length > 0)
      .map(role => ({
        role,
        meta: ROLE_SECTION_META[role],
        items: ROLE_NAV_ITEMS[role],
      }));
  }, [roles]);

  // ─── Display role string for header ───────────────────────────────────────

  const roleDisplay = useMemo(() => {
    if (!roles || roles.length === 0) return 'User';
    return roles.map(r => ROLE_SECTION_META[r]?.label || r).join(' · ');
  }, [roles]);

  // ──────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ──────────────────────────────────────────────────────────────────────────────

  return (
    <div className={`app-layout ${sidebarOpen ? '' : 'sidebar-collapsed'}`}>
      {/* Sidebar */}
      <aside className="sidebar">
        <div className="sidebar-header">
          <Link
            to={getDashboardRoute(primaryRole, roles)}
            className="logo-link"
          >
            <div className="logo-icon">
              <GraduationCapIcon width={22} height={22} />
            </div>
            {sidebarOpen && (
              <div className="logo-text-block">
                <span className="logo-title">UniMan</span>
                <span className="logo-subtitle">University System</span>
              </div>
            )}
          </Link>
        </div>

        {/* ─── Role-Grouped Navigation ─── */}
        <nav className="sidebar-nav">
          {roleSections.map(section => (
            <RoleNavSection
              key={section.role}
              roleName={section.meta.label}
              roleIcon={section.meta.icon}
              items={section.items}
              defaultOpen={section.role === primaryRole}
              sidebarOpen={sidebarOpen}
            />
          ))}
        </nav>

        <div className="sidebar-footer">
          {sidebarOpen ? (
            <div className="need-help-block">
              <div className="need-help-icon">
                <AlertIcon width={18} height={18} />
              </div>
              <div className="need-help-text">
                <span className="need-help-title">Need Help?</span>
                <a href="mailto:support@uniman.edu" className="need-help-link">
                  Contact Support
                </a>
              </div>
            </div>
          ) : (
            <div className="need-help-icon-only" title="Need Help?">
              <AlertIcon width={18} height={18} />
            </div>
          )}
        </div>
      </aside>

      {/* Main Content */}
      <div className="main-wrapper">
        <header className="top-header">
          <button
            className="toggle-btn"
            onClick={toggleSidebar}
            aria-label="Toggle sidebar"
          >
            {sidebarOpen ? <FiX /> : <FiMenu />}
          </button>

          <div className="header-search">
            <FiSearch />
            <input type="text" placeholder="Search anything..." />
            <span className="shortcut">⌘ K</span>
          </div>

          <div className="header-right">
            <div className="header-icons">
              <button className="header-icon-btn">
                <FiBell />
                <span className="icon-badge">3</span>
              </button>
              <button className="header-icon-btn">
                <FiMail />
                <span className="icon-badge">5</span>
              </button>
            </div>

            <div
              className="profile-dropdown"
              onClick={toggleProfile}
              role="button"
              tabIndex={0}
              onKeyDown={e => {
                if (e.key === 'Enter' || e.key === ' ') toggleProfile();
              }}
            >
              <div className="avatar">
                {user?.profilePicture ? (
                  <img
                    src={user.profilePicture}
                    alt={`${user.name}'s avatar`}
                  />
                ) : (
                  <span>
                    {user?.name?.charAt(0) ||
                      user?.displayName?.charAt(0) ||
                      'U'}
                  </span>
                )}
              </div>

              {sidebarOpen && (
                <div className="profile-info">
                  <span className="profile-name">
                    {user?.displayName || user?.name}
                  </span>
                  <span className="profile-role">{roleDisplay}</span>
                </div>
              )}

              <FiChevronDown />

              {/* Profile Dropdown */}
              {profileOpen && (
                <div className="dropdown-menu">
                  <div className="dropdown-header">
                    <strong>{user?.displayName || user?.name}</strong>
                    <small>{user?.email}</small>
                    <div className="user-roles">
                      {roles?.map(role => (
                        <span key={role} className="role-badge">
                          {ROLE_SECTION_META[role]?.label || role}
                        </span>
                      ))}
                    </div>
                  </div>

                  <div className="dropdown-divider" />

                  {isStudent && (
                    <Link
                      to={ROUTES.STUDENT.PROFILE}
                      className="dropdown-item"
                      onClick={closeProfile}
                    >
                      <FiUser /> Profile
                    </Link>
                  )}

                  {isAdmin && (
                    <Link
                      to={ROUTES.ADMIN.DASHBOARD}
                      className="dropdown-item"
                      onClick={closeProfile}
                    >
                      <FiSettings /> Admin Panel
                    </Link>
                  )}

                  <Link
                    to={ROUTES.STUDENT.CHANGE_PASSWORD}
                    className="dropdown-item"
                    onClick={closeProfile}
                  >
                    <FiLock /> Change Password
                  </Link>

                  <div className="dropdown-divider" />

                  <button className="dropdown-item" onClick={handleLogout}>
                    <FiLogOut /> Logout
                  </button>
                </div>
              )}
            </div>
          </div>
        </header>

        <main className="main-content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
