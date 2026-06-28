import { AdminDashboard } from '@pages/admin';
import { ROUTES, USER_ROLES } from '../constants';
import { AppRouteConfig, relative } from './routesTypes';

const ADMIN_ONLY = [USER_ROLES.ADMIN];

export const adminRoutes: AppRouteConfig[] = [
  {
    path: relative(ROUTES.ADMIN.DASHBOARD),
    element: <AdminDashboard />,
    roles: ADMIN_ONLY,
  },
];
