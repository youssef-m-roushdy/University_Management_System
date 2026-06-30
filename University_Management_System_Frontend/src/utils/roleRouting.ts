// utils/roleRouting.ts

import { ROUTES, USER_ROLES, UserRole } from '../constants';

/**
 * Single source of truth for "which dashboard does this role land on".
 * Used by both ProtectedRoute (unauthorized fallback) and
 * RoleDashboardRedirect (post-login landing), so they can never disagree.
 */
const ROLE_DASHBOARD_MAP: Record<UserRole, string> = {
  [USER_ROLES.ADMIN]: ROUTES.ADMIN.DASHBOARD,
  [USER_ROLES.STUDENT]: ROUTES.STUDENT.DASHBOARD,
  [USER_ROLES.INSTRUCTOR]: ROUTES.INSTRUCTOR.DASHBOARD,
  [USER_ROLES.ASSISTANT]: ROUTES.ASSISTANT.DASHBOARD,
};

/**
 * Returns the dashboard path for a user's primary role, or falls back to
 * the first role in their roles list if primaryRole isn't set, or to
 * /login if the user has no recognized role at all.
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