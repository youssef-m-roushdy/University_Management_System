// services/userService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface User {
  id: string;
  name: string;
  displayName?: string;
  email: string;
  userName: string;
  phoneNumber: string;
  profilePicture: string;
  address: string;
  gender: 'Male' | 'Female';
  isActive: boolean;
  roles: string[];
  createdAt: string;
  updatedAt: string;
}

export interface UpdateUserDto {
  name: string;
  phoneNumber: string;
  address: string;
  gender: 'Male' | 'Female';
}

export interface UserFilterParams {
  Name?: string;
  Email?: string;
  Gender?: 'Male' | 'Female';
  IsActive?: boolean;
  Role?: string;
  SearchTerm?: string;
  PageNumber?: number;
  PageSize?: number;
  SortBy?: string;
  SortDirection?: 'Ascending' | 'Descending';
}

export interface ApiResponse<T> {
  success: boolean;
  statusCode: number;
  message: string;
  data: T;
  errors?: Array<{
    code: string;
    message: string;
    field?: string;
  }>;
  timestamp: string;
}

export interface PaginatedResponse<T> {
  success: boolean;
  statusCode: number;
  message: string;
  data: T[];
  errors?: Array<{
    code: string;
    message: string;
    field?: string;
  }>;
  timestamp: string;
  pagination: {
    currentPage: number;
    pageSize: number;
    totalPages: number;
    totalCount: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
  };
}

// ──────────────────────────────────────────────────────────────────────────────
// USER SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const userService = {
  /**
   * Get current user profile
   */
  getMyProfile: () => {
    return apiService.get<ApiResponse<User>>(API_ENDPOINTS.USERS.MY_PROFILE);
  },

  /**
   * Get user profile by ID
   */
  getProfileById: (userId: string) => {
    return apiService.get<ApiResponse<User>>(
      API_ENDPOINTS.USERS.PROFILE_BY_ID(userId)
    );
  },

  /**
   * Get user profile by academic code
   */
  getProfileByAcademicCode: (academicCode: string) => {
    return apiService.get<ApiResponse<User>>(
      API_ENDPOINTS.USERS.PROFILE(academicCode)
    );
  },

  /**
   * Get user by email
   */
  getByEmail: (email: string) => {
    return apiService.get<ApiResponse<User>>(
      API_ENDPOINTS.USERS.BY_EMAIL(email)
    );
  },

  /**
   * Get user by ID
   */
  getById: (userId: string) => {
    return apiService.get<ApiResponse<User>>(API_ENDPOINTS.USERS.BY_ID(userId));
  },

  /**
   * Get all users with filters
   */
  getAll: (filters: UserFilterParams = {}) => {
    const queryString = buildQueryString(filters);
    return apiService.get<PaginatedResponse<User>>(
      `${API_ENDPOINTS.USERS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get all users with pagination
   */
  getAllPaginated: (
    filters: UserFilterParams = {},
    pageNumber: number = 1,
    pageSize: number = 10,
    sortBy: string | null = null,
    sortDirection: 'Ascending' | 'Descending' = 'Ascending'
  ) => {
    const sortByParam = sortBy || undefined;
    const params = createFilterPageParams(
      filters,
      pageNumber,
      pageSize,
      sortByParam,
      sortDirection
    );
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<User>>(
      `${API_ENDPOINTS.USERS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Update current user
   */
  update: (data: UpdateUserDto) => {
    return apiService.put<ApiResponse<string>>(API_ENDPOINTS.USERS.BASE, data);
  },

  /**
   * Update profile picture
   */
  updateProfilePicture: (formData: FormData) => {
    return apiService.postForm<ApiResponse<string>>(
      API_ENDPOINTS.USERS.UPDATE_PROFILE_PICTURE,
      formData
    );
  },

  /**
   * Delete profile picture
   */
  deleteProfilePicture: () => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.USERS.DELETE_PROFILE_PICTURE
    );
  },

  /**
   * Activate user
   */
  activate: (userId: string) => {
    return apiService.patch<ApiResponse<string>>(
      API_ENDPOINTS.USERS.ACTIVATE(userId)
    );
  },

  /**
   * Deactivate user
   */
  deactivate: (userId: string) => {
    return apiService.patch<ApiResponse<string>>(
      API_ENDPOINTS.USERS.DEACTIVATE(userId)
    );
  },

  /**
   * Delete user
   */
  delete: (userId: string) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.USERS.BY_ID(userId)
    );
  },

  /**
   * Update student specialization
   */
  updateSpecialization: (academicCode: string, data: any) => {
    return apiService.patch<ApiResponse<string>>(
      API_ENDPOINTS.USERS.UPDATE_SPECIALIZATION,
      data
    );
  },
};

export default userService;
