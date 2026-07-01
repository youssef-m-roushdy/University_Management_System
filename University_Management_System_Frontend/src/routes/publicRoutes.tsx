// routes/publicRoutes.tsx

import { ROUTES } from '../constants';
import { AppRouteConfig } from './routesTypes';
import Login from '../pages/auth/Login';
import ForgotPassword from '../pages/auth/ForgotPassword';
import ResetPassword from '../pages/auth/ResetPassword';

export const publicRoutes: AppRouteConfig[] = [
  { path: ROUTES.LOGIN, element: <Login /> },
  { path: ROUTES.FORGOT_PASSWORD, element: <ForgotPassword /> },
  { path: ROUTES.RESET_PASSWORD, element: <ResetPassword /> },
];
