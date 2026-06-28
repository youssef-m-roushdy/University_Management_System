// contexts/AuthContext.tsx

import React, {
  createContext,
  useContext,
  useReducer,
  useCallback,
  useMemo,
  ReactNode,
} from 'react';
import authService from '../services/authService';
import { STATUS, USER_ROLES, Status, UserRole } from '../constants';
import { User } from '../types';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  status: Status;
  error: string | null;
}

type AuthAction =
  | { type: 'LOADING' }
  | { type: 'LOGIN_SUCCESS'; payload: User }
  | { type: 'UPDATE_USER'; payload: Partial<User> }
  | { type: 'LOGOUT' }
  | { type: 'ERROR'; payload: string }
  | { type: 'CLEAR_ERROR' };

interface AuthContextType extends AuthState {
  login: (email: string, password: string) => Promise<any>;
  logout: () => void;
  clearError: () => void;
  updateUser: (fields: Partial<User>) => void;
  hasRole: (role: UserRole) => boolean;
  hasAnyRole: (roles: UserRole[]) => boolean;
  hasAllRoles: (roles: UserRole[]) => boolean;
  can: (permission: string) => boolean;
  isAdmin: boolean;
  isStudent: boolean;
  isInstructor: boolean;
  isAssistant: boolean;
  isSuperAdmin: boolean;
  roles: UserRole[];
  primaryRole: UserRole | undefined;
}

interface AuthProviderProps {
  children: ReactNode;
}

// ──────────────────────────────────────────────────────────────────────────────
// PERMISSIONS CONFIG
// ──────────────────────────────────────────────────────────────────────────────

type PermissionMap = Record<string, UserRole[]>;

const PERMISSIONS: PermissionMap = {
  'create-course': [USER_ROLES.ADMIN, USER_ROLES.INSTRUCTOR],
  'delete-course': [USER_ROLES.ADMIN],
  'enroll-course': [USER_ROLES.STUDENT],
  'grade-students': [USER_ROLES.INSTRUCTOR, USER_ROLES.ADMIN],
  'view-reports': [USER_ROLES.ADMIN, USER_ROLES.INSTRUCTOR],
  'manage-users': [USER_ROLES.ADMIN],
  'manage-departments': [USER_ROLES.ADMIN],
  'manage-study-years': [USER_ROLES.ADMIN],
  'manage-fees': [USER_ROLES.ADMIN],
  'view-all-students': [
    USER_ROLES.ADMIN,
    USER_ROLES.INSTRUCTOR,
    USER_ROLES.ASSISTANT,
  ],
  'view-own-courses': [USER_ROLES.STUDENT],
  'upload-course-material': [USER_ROLES.INSTRUCTOR, USER_ROLES.ASSISTANT],
  'approve-registrations': [USER_ROLES.ADMIN],
};

// ──────────────────────────────────────────────────────────────────────────────
// HELPER: Validate UserRole
// ──────────────────────────────────────────────────────────────────────────────

const isValidUserRole = (role: string): role is UserRole => {
  return Object.values(USER_ROLES).includes(role as UserRole);
};

const validateRoles = (roles: string[] | undefined): UserRole[] => {
  if (!roles) return [];
  return roles.filter(role => isValidUserRole(role)) as UserRole[];
};

// ──────────────────────────────────────────────────────────────────────────────
// INITIAL STATE
// ──────────────────────────────────────────────────────────────────────────────

const initialState: AuthState = {
  user: authService.getCurrentUser(),
  isAuthenticated: authService.isAuthenticated(),
  status: STATUS.IDLE,
  error: null,
};

// ──────────────────────────────────────────────────────────────────────────────
// REDUCER
// ──────────────────────────────────────────────────────────────────────────────

function authReducer(state: AuthState, action: AuthAction): AuthState {
  switch (action.type) {
    case 'LOADING':
      return { ...state, status: STATUS.LOADING, error: null };

    case 'LOGIN_SUCCESS':
      return {
        ...state,
        user: action.payload,
        isAuthenticated: true,
        status: STATUS.SUCCESS,
        error: null,
      };

    case 'UPDATE_USER':
      return {
        ...state,
        user: state.user ? { ...state.user, ...action.payload } : null,
      };

    case 'LOGOUT':
      return {
        ...state,
        user: null,
        isAuthenticated: false,
        status: STATUS.IDLE,
        error: null,
      };

    case 'ERROR':
      return { ...state, status: STATUS.ERROR, error: action.payload };

    case 'CLEAR_ERROR':
      return { ...state, error: null, status: STATUS.IDLE };

    default:
      return state;
  }
}

// ──────────────────────────────────────────────────────────────────────────────
// PROVIDER
// ──────────────────────────────────────────────────────────────────────────────

export function AuthProvider({ children }: AuthProviderProps) {
  const [state, dispatch] = useReducer(authReducer, initialState);

  // ─── Login ──────────────────────────────────────────────────────────────────
  const login = useCallback(async (email: string, password: string) => {
    dispatch({ type: 'LOADING' });
    try {
      const { user } = await authService.login(email, password);
      console.log('Login successful:', user);
      dispatch({ type: 'LOGIN_SUCCESS', payload: user });
      return user;
    } catch (err: any) {
      const errorMessage =
        err?.errorMessage || err?.ErrorMessage || 'Login failed';
      dispatch({ type: 'ERROR', payload: errorMessage });
      throw err;
    }
  }, []);

  // ─── Logout ─────────────────────────────────────────────────────────────────
  const logout = useCallback(() => {
    authService.logout();
    dispatch({ type: 'LOGOUT' });
  }, []);

  // ─── Clear Error ───────────────────────────────────────────────────────────
  const clearError = useCallback(() => {
    dispatch({ type: 'CLEAR_ERROR' });
  }, []);

  // ─── Update User ───────────────────────────────────────────────────────────
  const updateUser = useCallback((fields: Partial<User>) => {
    dispatch({ type: 'UPDATE_USER', payload: fields });

    // Also update the stored user data
    const currentUser = authService.getCurrentUser();
    if (currentUser) {
      const updatedUser = { ...currentUser, ...fields };
      authService.updateUserData(updatedUser);
    }
  }, []);

  // ─── Role Checking ─────────────────────────────────────────────────────────
  const hasRole = useCallback(
    (role: UserRole): boolean => {
      const userRoles = validateRoles(state.user?.roles as string[]);
      return userRoles.includes(role);
    },
    [state.user]
  );

  const hasAnyRole = useCallback(
    (roles: UserRole[]): boolean => {
      const userRoles = validateRoles(state.user?.roles as string[]);
      return roles.some(role => userRoles.includes(role));
    },
    [state.user]
  );

  const hasAllRoles = useCallback(
    (roles: UserRole[]): boolean => {
      const userRoles = validateRoles(state.user?.roles as string[]);
      return roles.every(role => userRoles.includes(role));
    },
    [state.user]
  );

  // ─── Permission Checking ───────────────────────────────────────────────────
  const can = useCallback(
    (permission: string): boolean => {
      const requiredRoles = PERMISSIONS[permission];
      if (!requiredRoles) return false;
      return hasAnyRole(requiredRoles);
    },
    [hasAnyRole]
  );

  // ─── Computed Role Checks ─────────────────────────────────────────────────
  const roleChecks = useMemo(
    () => ({
      isAdmin: hasRole(USER_ROLES.ADMIN),
      isStudent: hasRole(USER_ROLES.STUDENT),
      isInstructor: hasRole(USER_ROLES.INSTRUCTOR),
      isAssistant: hasRole(USER_ROLES.ASSISTANT),
      isSuperAdmin: hasRole(USER_ROLES.ADMIN),
    }),
    [hasRole]
  );

  // ─── Roles ──────────────────────────────────────────────────────────────────
  const roles = useMemo(() => {
    return validateRoles(state.user?.roles as string[]);
  }, [state.user]);

  const primaryRole = useMemo(() => roles[0], [roles]);

  // ──────────────────────────────────────────────────────────────────────────────
  // CONTEXT VALUE
  // ──────────────────────────────────────────────────────────────────────────────

  const contextValue: AuthContextType = {
    // State
    ...state,
    user: state.user,
    isAuthenticated: state.isAuthenticated,
    status: state.status,
    error: state.error,

    // Actions
    login,
    logout,
    clearError,
    updateUser,

    // Role checking
    hasRole,
    hasAnyRole,
    hasAllRoles,
    can,

    // Computed role checks
    ...roleChecks,

    // Roles
    roles,
    primaryRole,
  };

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
}

// ──────────────────────────────────────────────────────────────────────────────
// HOOK
// ──────────────────────────────────────────────────────────────────────────────

export function useAuth(): AuthContextType {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return ctx;
}

// ──────────────────────────────────────────────────────────────────────────────
// CONTEXT
// ──────────────────────────────────────────────────────────────────────────────

const AuthContext = createContext<AuthContextType | null>(null);

export default AuthContext;
