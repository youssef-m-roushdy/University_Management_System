// routes/protectedRoutes.tsx

import { AppRouteConfig } from './routesTypes';
import { ROUTES, USER_ROLES } from '../constants';
import { adminRoutes } from './adminRoutes';
import { studentRoutes } from './studentRoutes';
import { instructorRoutes } from './instructorRoutes';
import { assistantRoutes } from './assistantRoutes';
import CompositeDashboard from '../pages/dashboard/CompositeDashboard';

export const protectedRoutes: AppRouteConfig[] = [
  // ─── Composite dashboard for multi-role users ───
  // No specific role requirement — any authenticated user can reach it.
  // The CompositeDashboard itself renders sections per-role,
  // so a user with Student+Assistant sees both sections.
  {
    path: ROUTES.DASHBOARD_ME,
    element: <CompositeDashboard />,
    // intentionally NO roles — accessible to any authenticated user
  },

  ...adminRoutes,
  ...studentRoutes,
  ...instructorRoutes,
  ...assistantRoutes,
];
