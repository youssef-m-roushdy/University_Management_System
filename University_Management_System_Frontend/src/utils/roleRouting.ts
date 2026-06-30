// utils/roleRouting.ts

import { ROUTES, USER_ROLES, UserRole } from '../constants';

const ROLE_DASHBOARD_MAP: Record<UserRole, string> = {
  [USER_ROLES.ADMIN]: ROUTES.ADMIN.DASHBOARD,
  [USER_ROLES.STUDENT]: ROUTES.STUDENT.DASHBOARD,
  [USER_ROLES.INSTRUCTOR]: ROUTES.INSTRUCTOR.DASHBOARD,
  [USER_ROLES.ASSISTANT]: ROUTES.ASSISTANT.DASHBOARD,
};

/**
 * Returns the dashboard path for a user's primary role.
 * Falls back to first role in list, or /login if no role.
 */
export function getDashboardRoute(
  primaryRole: UserRole | null | undefined,
  roles?: UserRole[] | null
): string {
  if (primaryRole && ROLE_DASHBOARD_MAP[primaryRole]) {
    return ROLE_DASHBOARD_MAP[primaryRole];
  }

  const fallbackRole = roles?.find(r => ROLE_DASHBOARD_MAP[r]);
  if (fallbackRole) {
    return ROLE_DASHBOARD_MAP[fallbackRole];
  }

  return ROUTES.LOGIN;
}

/**
 * Returns the initial landing route after authentication:
 * - 1 role → direct role dashboard (clean URL)
 * - Multiple roles → composite dashboard (/dashboard/me)
 * - No roles → login
 */
export function getInitialDashboardRoute(
  primaryRole: UserRole | null | undefined,
  roles?: UserRole[] | null
): string {
  if (!roles || roles.length === 0) return ROUTES.LOGIN;

  if (roles.length === 1) {
    return getDashboardRoute(primaryRole, roles);
  }

  return ROUTES.DASHBOARD_ME;
}
