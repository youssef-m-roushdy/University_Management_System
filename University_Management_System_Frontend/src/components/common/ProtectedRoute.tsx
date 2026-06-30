// components/common/ProtectedRoute.tsx

import React from 'react';
import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';
import { useAuth } from '../../contexts/AuthContext';
import { ROUTES, UserRole } from '../../constants';
import { getDashboardRoute } from '../../utils/roleRouting';

interface ProtectedRouteProps {
  children?: React.ReactNode;
  roles?: UserRole[];
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, roles }) => {
  const { isAuthenticated, hasAnyRole, primaryRole, roles: userRoles } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    return <Navigate to={ROUTES.LOGIN} state={{ from: location }} replace />;
  }

  const isAuthorized = !roles || roles.length === 0 || hasAnyRole(roles);

  if (!isAuthorized) {
    toast.error("You don't have permission to access this page.");
    // Send them to *their own* dashboard, not a hardcoded admin one —
    // otherwise a student blocked from an admin page gets redirected
    // somewhere they also can't access.
    return <Navigate to={getDashboardRoute(primaryRole, userRoles)} replace />;
  }

  return <>{children ?? <Outlet />}</>;
};

export default ProtectedRoute;