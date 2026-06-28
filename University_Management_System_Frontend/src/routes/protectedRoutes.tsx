// routes/protectedRoutes.tsx
import { USER_ROLES } from '../constants';
import { AppRouteConfig } from './routesTypes';
import AdminDashboard from '../pages/admin/Dashboard/AdminDashboard';
import StudentDashboard from '../pages/student/Dashboard/StudentDashboard';
// Dashboard imports

export const protectedRoutes: AppRouteConfig[] = [
  // ─── Admin Dashboard ────────────────────────────────────────────
  {
    path: '/dashboard/admin',
    element: <AdminDashboard />,
    roles: [USER_ROLES.ADMIN],
  },
  // ─── Student Dashboard ──────────────────────────────────────────
  {
    path: '/dashboard/student',
    element: <StudentDashboard />,
    roles: [USER_ROLES.STUDENT],
  },
];
