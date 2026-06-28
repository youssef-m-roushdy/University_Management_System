import { StudentDashboard } from '@pages/student';
import { ROUTES, USER_ROLES } from '../constants';
import { AppRouteConfig, relative } from './routesTypes';

const STUDENT_ONLY = [USER_ROLES.STUDENT];

export const studentRoutes: AppRouteConfig[] = [
  {
    path: relative(ROUTES.STUDENT.DASHBOARD),
    element: <StudentDashboard />,
    roles: STUDENT_ONLY,
  },
];
