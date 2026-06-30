// routes/routesTypes.ts

import { ReactElement } from 'react';
import { UserRole } from '../constants';

export interface AppRouteConfig {
  path: string;
  element: ReactElement;
  roles?: UserRole[];
}