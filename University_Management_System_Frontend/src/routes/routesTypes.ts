import { ReactElement } from 'react';
import { UserRole } from '../constants';

export interface AppRouteConfig {
  /** Path relative to its parent <Route> — no leading slash, e.g. "admin/departments" */
  path: string;
  element: ReactElement;
  /** Roles allowed to view this route. Omit for any authenticated user. */
  roles?: UserRole[];
}

/**
 * ROUTES constants store absolute, leading-slash paths (useful for <Link>/navigate()).
 * Nested <Route> definitions need them relative to their parent, so we strip the slash here
 * rather than keeping two separate sources of truth for the same path.
 */
export const relative = (path: string): string => path.replace(/^\//, '');
