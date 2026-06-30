// components/common/RoleDashboardRedirect.tsx

import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { ROUTES } from '../../constants';
import { getInitialDashboardRoute } from '../../utils/roleRouting';

/**
 * Renders at "/" (the index route) when the user is authenticated.
 * - Single role → that role's specific dashboard
 * - Multiple roles → composite dashboard (/dashboard/me)
 * - No roles → login (shouldn't happen but safe fallback)
 */
const RoleDashboardRedirect: React.FC = () => {
  const { primaryRole, roles } = useAuth();
  const target = getInitialDashboardRoute(primaryRole, roles);
  return <Navigate to={target} replace />;
};

export default RoleDashboardRedirect;
