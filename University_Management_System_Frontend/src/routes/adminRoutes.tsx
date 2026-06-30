// routes/adminRoutes.tsx

import { ROUTES, USER_ROLES } from '../constants';
import { AppRouteConfig } from './routesTypes';
import AdminDashboard from '../pages/admin/Dashboard/AdminDashboard';
import AdminDepartments from '../pages/admin/Departments/AdminDepartments';
import Admins from '../pages/admin/Admins';
import AdminUsers from '../pages/admin/Users/AdminUsers';
import AdminStudyYears from '../pages/admin/StudyYears/AdminStudyYears';
import AdminRoles from '../pages/admin/Roles/AdminRoles';
import AdminStudents from '../pages/admin/Students/AdminStudents';
import AdminAssistants from '../pages/admin/Assistants/AdminAssistants';
import AdminInstructors from '../pages/admin/Instructors/AdminInstructors';

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
  {
    path: ROUTES.ADMIN.ADMINS,
    element: <Admins />,
    roles: [USER_ROLES.ADMIN],
  },
  {
    path: ROUTES.ADMIN.USERS,
    element: <AdminUsers />,
    roles: [USER_ROLES.ADMIN],
  },
  {
    path: ROUTES.ADMIN.STUDY_YEARS,
    element: <AdminStudyYears />,
    roles: [USER_ROLES.ADMIN],
  },
  {
    path: ROUTES.ADMIN.ROLES,
    element: <AdminRoles />,
    roles: [USER_ROLES.ADMIN],
  },
  {
    path: ROUTES.ADMIN.STUDENTS,
    element: <AdminStudents />,
    roles: [USER_ROLES.ADMIN],
  },
  {
    path: ROUTES.ADMIN.ASSISTANTS,
    element: <AdminAssistants />,
    roles: [USER_ROLES.ADMIN],
  },
  {
    path: ROUTES.ADMIN.INSTRUCTORS,
    element: <AdminInstructors />,
    roles: [USER_ROLES.ADMIN],
  },
  // Courses, Students, Users, StudyYears, Roles, PromoteStudents
  // get added here as those pages exist — keep using ROUTES.ADMIN.* as the
  // single source of truth instead of hardcoded path strings.
];
