// routes/adminRoutes.tsx

import { ROUTES, USER_ROLES } from '../constants';
import { AppRouteConfig } from './routesTypes';
import AdminDashboard from '../pages/admin/Dashboard/AdminDashboard';
import AdminDepartments from '../pages/admin/Departments/AdminDepartments';

export const adminRoutes: AppRouteConfig[] = [
  {
    path: ROUTES.ADMIN.DASHBOARD,
    element: <AdminDashboard />,
    roles: [USER_ROLES.ADMIN],
  },
  {
    path: ROUTES.ADMIN.DEPARTMENTS,
    element: <AdminDepartments />,
    roles: [USER_ROLES.ADMIN],
  },
  // Courses, Students, Users, StudyYears, Roles, PromoteStudents
  // get added here as those pages exist — keep using ROUTES.ADMIN.* as the
  // single source of truth instead of hardcoded path strings.
];