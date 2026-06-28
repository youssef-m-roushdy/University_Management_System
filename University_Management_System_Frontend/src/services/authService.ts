// services/authService.ts

import apiService from './apiService';
import { API_ENDPOINTS, STORAGE_KEYS, UserRole } from '../constants';
import {
  AuthData,
  AuthResponse,
  ApiResponse,
  User,
  ResetPasswordRequest,
  ChangePasswordRequest,
  VerifyEmailRequest,
  LoginResponse,
} from '../types';

// Helper to convert AuthData to User
const convertAuthDataToUser = (authData: AuthData): User => {
  return {
    id: authData.userId,
    userId: authData.userId,
    email: authData.email,
    name: authData.name,
    roles: authData.roles,
    profilePicture: authData.profilePicture,
    studentProfile: authData.studentProfile,
    instructorProfile: authData.instructorProfile,
    assistantProfile: authData.assistantProfile,
    adminProfile: authData.adminProfile,
    academicCode: authData.studentProfile?.academicCode,
  };
};

const authService = {
  // ──────────────────────────────────────────────────────────────────────────────
  // AUTHENTICATION
  // ──────────────────────────────────────────────────────────────────────────────

  async login(
    email: string,
    password: string
  ): Promise<{ user: User; data: AuthData }> {
    const response = await apiService.post<LoginResponse>(
      API_ENDPOINTS.AUTH.LOGIN,
      { email, password }
    );

    if (response?.success && response?.data) {
      const { accessToken, refreshToken, accessTokenExpiry } = response.data;

      apiService.setTokens(accessToken, refreshToken);
      localStorage.setItem(STORAGE_KEYS.TOKEN_EXPIRY, accessTokenExpiry);
      localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(response.data));

      const user = convertAuthDataToUser(response.data);

      return { user, data: response.data };
    }

    throw new Error('Login failed');
  },

  async logout(): Promise<void> {
    const refreshToken = apiService.getRefreshToken();

    if (refreshToken) {
      try {
        await apiService.post(API_ENDPOINTS.AUTH.LOGOUT, { refreshToken });
      } catch (error) {
        console.error('Logout error:', error);
      }
    }

    this.clearAuthData();
  },

  async refreshToken(): Promise<{ user: User; data: AuthData }> {
    const accessToken = apiService.getToken();
    const refreshToken = apiService.getRefreshToken();

    if (!accessToken || !refreshToken) {
      throw new Error('No tokens available for refresh');
    }

    const response = await apiService.post<AuthResponse>(
      API_ENDPOINTS.AUTH.REFRESH_TOKEN,
      { accessToken, refreshToken }
    );

    if (response?.success && response?.data) {
      const {
        accessToken: newAccessToken,
        refreshToken: newRefreshToken,
        accessTokenExpiry,
      } = response.data;

      apiService.setTokens(newAccessToken, newRefreshToken);
      localStorage.setItem(STORAGE_KEYS.TOKEN_EXPIRY, accessTokenExpiry);
      localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(response.data));

      const user = convertAuthDataToUser(response.data);
      return { user, data: response.data };
    }

    throw new Error('Token refresh failed');
  },

  async revokeToken(refreshToken: string): Promise<ApiResponse<string>> {
    return apiService.post(API_ENDPOINTS.AUTH.REVOKE_TOKEN, { refreshToken });
  },

  async revokeAllTokens(): Promise<ApiResponse<string>> {
    return apiService.post(API_ENDPOINTS.AUTH.REVOKE_ALL_TOKENS);
  },

  // ──────────────────────────────────────────────────────────────────────────────
  // PASSWORD MANAGEMENT
  // ──────────────────────────────────────────────────────────────────────────────

  async forgotPassword(email: string): Promise<ApiResponse<string>> {
    return apiService.post(API_ENDPOINTS.AUTH.FORGOT_PASSWORD, { email });
  },

  async resetPassword(
    data: ResetPasswordRequest
  ): Promise<ApiResponse<string>> {
    return apiService.post(API_ENDPOINTS.AUTH.RESET_PASSWORD, data);
  },

  async changePassword(
    data: ChangePasswordRequest
  ): Promise<ApiResponse<string>> {
    return apiService.post(API_ENDPOINTS.AUTH.CHANGE_PASSWORD, data);
  },

  // ──────────────────────────────────────────────────────────────────────────────
  // EMAIL VERIFICATION
  // ──────────────────────────────────────────────────────────────────────────────

  async verifyEmail(data: VerifyEmailRequest): Promise<ApiResponse<string>> {
    return apiService.post(API_ENDPOINTS.AUTH.VERIFY_EMAIL, data);
  },

  async resendVerification(email: string): Promise<ApiResponse<string>> {
    return apiService.post(API_ENDPOINTS.AUTH.RESEND_VERIFICATION, { email });
  },

  // ──────────────────────────────────────────────────────────────────────────────
  // HELPERS
  // ──────────────────────────────────────────────────────────────────────────────

  clearAuthData(): void {
    apiService.clearTokens();
  },

  getCurrentUser(): User | null {
    const userData = localStorage.getItem(STORAGE_KEYS.USER);
    if (!userData) return null;

    try {
      const authData = JSON.parse(userData) as AuthData;
      return convertAuthDataToUser(authData);
    } catch {
      return null;
    }
  },

  getCurrentAuthData(): AuthData | null {
    const userData = localStorage.getItem(STORAGE_KEYS.USER);
    if (!userData) return null;

    try {
      return JSON.parse(userData) as AuthData;
    } catch {
      return null;
    }
  },

  isAuthenticated(): boolean {
    const token = apiService.getToken();
    const expiry = localStorage.getItem(STORAGE_KEYS.TOKEN_EXPIRY);

    if (!token) return false;

    if (expiry) {
      const expiryDate = new Date(expiry);
      if (expiryDate <= new Date()) {
        this.clearAuthData();
        return false;
      }
    }

    return true;
  },

  getUserRoles(): UserRole[] {
    const user = this.getCurrentUser();
    return (user?.roles || []) as UserRole[];
  },

  hasRole(role: UserRole): boolean {
    const roles = this.getUserRoles();
    return roles.includes(role);
  },

  hasAnyRole(roles: UserRole[]): boolean {
    const userRoles = this.getUserRoles();
    return roles.some(role => userRoles.includes(role));
  },

  getUserProfile(): any {
    const user = this.getCurrentUser();
    if (!user) return null;

    if (user.roles.includes('Admin')) {
      return user.adminProfile;
    } else if (user.roles.includes('Instructor')) {
      return user.instructorProfile;
    } else if (user.roles.includes('Assistant')) {
      return user.assistantProfile;
    } else if (user.roles.includes('Student')) {
      return user.studentProfile;
    }

    return null;
  },

  getAccessToken(): string | null {
    return apiService.getToken();
  },

  getRefreshToken(): string | null {
    return apiService.getRefreshToken();
  },

  isTokenExpired(): boolean {
    const expiry = localStorage.getItem(STORAGE_KEYS.TOKEN_EXPIRY);
    if (!expiry) return true;

    try {
      const expiryDate = new Date(expiry);
      return expiryDate <= new Date();
    } catch {
      return true;
    }
  },

  getTokenExpiry(): Date | null {
    const expiry = localStorage.getItem(STORAGE_KEYS.TOKEN_EXPIRY);
    if (!expiry) return null;

    try {
      return new Date(expiry);
    } catch {
      return null;
    }
  },

  getTokenExpirySeconds(): number {
    const expiry = this.getTokenExpiry();
    if (!expiry) return 0;

    const now = new Date();
    const diff = expiry.getTime() - now.getTime();
    return Math.max(0, Math.floor(diff / 1000));
  },

  updateUserData(userData: User): void {
    // Get current auth data
    const authData = this.getCurrentAuthData();
    if (!authData) return;

    // Update only the fields that exist in both User and AuthData
    const updatedAuthData: AuthData = {
      ...authData,
      name: userData.name || authData.name,
      email: userData.email || authData.email,
      profilePicture: userData.profilePicture || authData.profilePicture,
      roles: userData.roles || authData.roles,
      studentProfile: userData.studentProfile || authData.studentProfile,
      instructorProfile:
        userData.instructorProfile || authData.instructorProfile,
      assistantProfile: userData.assistantProfile || authData.assistantProfile,
      adminProfile: userData.adminProfile || authData.adminProfile,
    };

    localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(updatedAuthData));
  },
};

export default authService;
