// components/common/RoleDashboardRedirect.tsx

import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { ROUTES, USER_ROLES } from '../../constants';

/**
 * Renders at ROUTES.DASHBOARD and forwards the user to their role-specific
 * dashboard. Always uses full absolute paths from ROUTES — never builds a
 * path by concatenating the role name, since that produces a path relative
 * to the current route (e.g. '/dashboard' + 'admin' -> '/dashboard/admin')
 * instead of the intended '/admin/dashboard'.
 */
const RoleDashboardRedirect: React.FC = () => {
  const { primaryRole } = useAuth();

  switch (primaryRole) {
    case USER_ROLES.ADMIN:
      return <Navigate to={ROUTES.ADMIN.DASHBOARD} replace />;
    case USER_ROLES.STUDENT:
      return <Navigate to={ROUTES.STUDENT.DASHBOARD} replace />;
    case USER_ROLES.INSTRUCTOR:
      return <Navigate to={ROUTES.INSTRUCTOR.DASHBOARD} replace />;
    case USER_ROLES.ASSISTANT:
      return <Navigate to={ROUTES.ASSISTANT.DASHBOARD} replace />;
    default:
      // No recognized role — bounce to login rather than looping back
      // to /dashboard, which would re-render this component and could
      // cause a redirect loop.
      return <Navigate to={ROUTES.LOGIN} replace />;
  }
};

export default RoleDashboardRedirect;