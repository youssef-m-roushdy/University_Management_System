// routes/protectedRoutes.tsx

import { AppRouteConfig } from './routesTypes';
import { adminRoutes } from './adminRoutes';
import { studentRoutes } from './studentRoutes';
import { instructorRoutes } from './instructorRoutes';
import { assistantRoutes } from './assistantRoutes';

export const protectedRoutes: AppRouteConfig[] = [
  ...adminRoutes,
  ...studentRoutes,
  ...instructorRoutes,
  ...assistantRoutes,
];