// routes/studentRoutes.tsx

import { ROUTES, USER_ROLES } from '../constants';
import { AppRouteConfig } from './routesTypes';
import StudentDashboard from '../pages/student/Dashboard/StudentDashboard';

export const studentRoutes: AppRouteConfig[] = [
  {
    path: ROUTES.STUDENT.DASHBOARD,
    element: <StudentDashboard />,
    roles: [USER_ROLES.STUDENT],
  },
];