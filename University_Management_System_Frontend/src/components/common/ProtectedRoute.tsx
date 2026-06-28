import React from 'react';
import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';
import { useAuth } from '../../contexts/AuthContext';
import { ROUTES, UserRole } from '../../constants';

interface ProtectedRouteProps {
  /**
   * Rendered when explicitly passed (matches your existing usage,
   * e.g. <ProtectedRoute roles={['Admin']}><Departments /></ProtectedRoute>).
   * When omitted, renders <Outlet /> instead, so this same component can
   * also guard a parent layout route with nested children.
   */
  children?: React.ReactNode;
  /** Roles permitted to view this route. Omit to allow any authenticated user. */
  roles?: UserRole[];
}

/**
 * Central route guard.
 *  - Redirects unauthenticated users to /login, preserving the location
 *    they were trying to reach (via `state.from`) so Login can send them
 *    back after a successful sign-in.
 *  - Blocks authenticated users whose roles don't include any allowed role
 *    and sends them to the dashboard with an explanatory toast.
 *
 * NOTE: AuthContext hydrates `user`/`isAuthenticated` synchronously from
 * authService on mount (see initialState), so there's no separate loading
 * phase to wait on here.
 */
const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, roles }) => {
  const { isAuthenticated, hasAnyRole } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    return <Navigate to={ROUTES.LOGIN} state={{ from: location }} replace />;
  }

  const isAuthorized = !roles || roles.length === 0 || hasAnyRole(roles);

  if (!isAuthorized) {
    toast.error("You don't have permission to access this page.");
    return <Navigate to={ROUTES.DASHBOARD} replace />;
  }

  return <>{children ?? <Outlet />}</>;
};

export default ProtectedRoute;
