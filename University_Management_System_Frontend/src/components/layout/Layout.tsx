// components/layout/Layout.tsx

import React, { useState, useMemo, useCallback } from 'react';
import { Link, NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { ROUTES, USER_ROLES, UserRole } from '../../constants';
import { getDashboardRoute } from '../../utils/roleRouting';
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
  FiArrowUp,
  FiLock,
  FiUserPlus,
  FiSettings,
  FiSearch,
  FiBell,
  FiMail,
  FiLogOut,
} from 'react-icons/fi';
import { AlertIcon, GraduationCapIcon } from '../icons/Icons';
import './Layout.css';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface NavItem {
  to: string;
  icon: React.ReactNode;
  label: string;
  roles: UserRole[];
}

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENT
// ──────────────────────────────────────────────────────────────────────────────

export default function Layout() {
  const {
    user,
    logout,
    isAdmin,
    isStudent,
    isInstructor,
    isAssistant,
    roles,
    primaryRole,
    hasAnyRole,
    hasRole,
  } = useAuth();

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

  // ─── Navigation Items ──────────────────────────────────────────────────────

  const adminNav: NavItem[] = useMemo(
    () => [
      {
        to: ROUTES.ADMIN.DASHBOARD,
        icon: <FiHome />,
        label: 'Dashboard',
        roles: [USER_ROLES.ADMIN],
      },
      {
        to: ROUTES.ADMIN.DEPARTMENTS, // ← This is the link
        icon: <FiGrid />,
        label: 'Departments',
        roles: [USER_ROLES.ADMIN],
      },
      {
        to: ROUTES.ADMIN.COURSES,
        icon: <FiBook />,
        label: 'Courses',
        roles: [USER_ROLES.ADMIN],
      },
      {
        to: ROUTES.ADMIN.STUDENTS,
        icon: <FiUsers />,
        label: 'Students',
        roles: [USER_ROLES.ADMIN],
      },
      {
        to: ROUTES.ADMIN.USERS,
        icon: <FiUserPlus />,
        label: 'Users',
        roles: [USER_ROLES.ADMIN],
      },
      {
        to: ROUTES.ADMIN.STUDY_YEARS,
        icon: <FiCalendar />,
        label: 'Study Years',
        roles: [USER_ROLES.ADMIN],
      },
      {
        to: ROUTES.ADMIN.ROLES,
        icon: <FiShield />,
        label: 'Roles',
        roles: [USER_ROLES.ADMIN],
      },
      {
        to: ROUTES.ADMIN.PROMOTE_STUDENTS,
        icon: <FiArrowUp />,
        label: 'Promote Students',
        roles: [USER_ROLES.ADMIN],
      },
    ],
    []
  );

  const studentNav: NavItem[] = useMemo(
    () => [
      {
        to: ROUTES.STUDENT.DASHBOARD,
        icon: <FiHome />,
        label: 'Dashboard',
        roles: [USER_ROLES.STUDENT],
      },
      {
        to: ROUTES.STUDENT.MY_COURSES,
        icon: <FiBook />,
        label: 'My Courses',
        roles: [USER_ROLES.STUDENT],
      },
      {
        to: ROUTES.STUDENT.MY_TIMELINE,
        icon: <FiLayers />,
        label: 'Timeline',
        roles: [USER_ROLES.STUDENT],
      },
      {
        to: ROUTES.STUDENT.MY_STUDY_YEARS,
        icon: <FiCalendar />,
        label: 'Study Years',
        roles: [USER_ROLES.STUDENT],
      },
      {
        to: ROUTES.STUDENT.DEPARTMENT_COURSES,
        icon: <FiGrid />,
        label: 'Department Courses',
        roles: [USER_ROLES.STUDENT],
      },
      {
        to: ROUTES.STUDENT.PROFILE,
        icon: <FiUser />,
        label: 'Profile',
        roles: [USER_ROLES.STUDENT],
      },
      {
        to: ROUTES.STUDENT.CHANGE_PASSWORD,
        icon: <FiLock />,
        label: 'Change Password',
        roles: [USER_ROLES.STUDENT],
      },
    ],
    []
  );

  const instructorNav: NavItem[] = useMemo(
    () => [
      {
        to: ROUTES.INSTRUCTOR.DASHBOARD,
        icon: <FiHome />,
        label: 'Dashboard',
        roles: [USER_ROLES.INSTRUCTOR],
      },
      {
        to: ROUTES.INSTRUCTOR.COURSES,
        icon: <FiBook />,
        label: 'My Courses',
        roles: [USER_ROLES.INSTRUCTOR],
      },
      {
        to: ROUTES.INSTRUCTOR.STUDENTS,
        icon: <FiUsers />,
        label: 'Students',
        roles: [USER_ROLES.INSTRUCTOR],
      },
    ],
    []
  );

  const assistantNav: NavItem[] = useMemo(
    () => [
      {
        to: ROUTES.ASSISTANT.DASHBOARD,
        icon: <FiHome />,
        label: 'Dashboard',
        roles: [USER_ROLES.ASSISTANT],
      },
      {
        to: ROUTES.ASSISTANT.COURSES,
        icon: <FiBook />,
        label: 'Courses',
        roles: [USER_ROLES.ASSISTANT],
      },
      {
        to: ROUTES.ASSISTANT.STUDENTS,
        icon: <FiUsers />,
        label: 'Students',
        roles: [USER_ROLES.ASSISTANT],
      },
      {
        to: ROUTES.ASSISTANT.GRADING,
        icon: <FiShield />,
        label: 'Grading',
        roles: [USER_ROLES.ASSISTANT],
      },
    ],
    []
  );

  // ─── Combine and Filter Navigation ────────────────────────────────────────

  const navItems = useMemo(() => {
    const allItems = [
      ...adminNav,
      ...studentNav,
      ...instructorNav,
      ...assistantNav,
    ];
    return allItems.filter(item => hasAnyRole(item.roles));
  }, [adminNav, studentNav, instructorNav, assistantNav, hasAnyRole]);

  // ─── User Info ─────────────────────────────────────────────────────────────

  const primaryUserRole = useMemo(() => primaryRole || 'User', [primaryRole]);

  // ──────────────────────────────────────────────────────────────────────────────
  // RENDER
  // ──────────────────────────────────────────────────────────────────────────────

  return (
    <div className={`app-layout ${sidebarOpen ? '' : 'sidebar-collapsed'}`}>
      {/* Sidebar */}
      <aside className="sidebar">
        <div className="sidebar-header">
          <Link to={getDashboardRoute(primaryRole, roles)} className="logo-link">
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

        <nav className="sidebar-nav">
          {navItems.map(item => (
            <NavLink
              key={item.to}
              to={item.to}
              className={({ isActive }) =>
                `nav-item ${isActive ? 'active' : ''}`
              }
              onClick={closeProfile}
            >
              <span className="nav-icon">{item.icon}</span>
              {sidebarOpen && <span className="nav-label">{item.label}</span>}
            </NavLink>
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
                if (e.key === 'Enter' || e.key === ' ') {
                  toggleProfile();
                }
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
                  <span className="profile-role">{primaryUserRole}</span>
                  {roles && roles.length > 1 && (
                    <span className="profile-more-roles">
                      +{roles.length - 1} more
                    </span>
                  )}
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
                          {role}
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
                      to={ROUTES.ADMIN.DEPARTMENTS}
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
