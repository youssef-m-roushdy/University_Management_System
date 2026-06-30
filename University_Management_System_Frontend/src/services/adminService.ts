// services/adminService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface Admin {
  id: string;
  name: string;
  userName: string;
  email: string;
  phoneNumber: string;
  profilePicture: string;
  address: string;
  gender: 'Male' | 'Female';
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateAdminDto {
  email: string;
  password: string;
  name: string;
  userName: string;
  phoneNumber: string;
  address: string;
  gender: 'Male' | 'Female';
  isActive: boolean;
}

export interface UpdateAdminDto {
  email?: string;
  name?: string;
  userName?: string;
  phoneNumber?: string;
  address?: string;
  gender?: 'Male' | 'Female';
  isActive?: boolean;
}

export interface AddAdminToExistingUserDto {
  userEmail: string;
}

export interface AdminFilterParams {
  Name?: string;
  Gender?: 'Male' | 'Female';
  IsActive?: boolean;
  SearchTerm?: string;
  PageNumber?: number;
  PageSize?: number;
  SortBy?: string;
  SortDirection?: 'Ascending' | 'Descending';
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

// ──────────────────────────────────────────────────────────────────────────────
// ADMIN SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const adminService = {
  /**
   * Get all admins (with optional filters)
   * GET /api/Admin
   */
  getAll: (params?: AdminFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Admin>>(
        `${API_ENDPOINTS.ADMINS.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Admin>>(
      API_ENDPOINTS.ADMINS.BASE
    );
  },

  /**
   * Get admins with filters
   */
  getAllWithFilters: (params: AdminFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Admin>>(
      `${API_ENDPOINTS.ADMINS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get admins with pagination
   */
  getAllWithPagination: (
    filters: AdminFilterParams = {},
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
    return adminService.getAllWithFilters(params);
  },

  /**
   * Get admin by ID
   * GET /api/Admin/{id}
   */
  getById: (id: string) => {
    return apiService.get<ApiResponse<Admin>>(
      API_ENDPOINTS.ADMINS.BY_ID(id)
    );
  },

  /**
   * Create a new admin
   * POST /api/Admin
   */
  create: (data: CreateAdminDto) => {
    return apiService.post<ApiResponse<Admin>>(
      API_ENDPOINTS.ADMINS.BASE,
      data
    );
  },

  /**
   * Update an admin
   * PUT /api/Admin/{id}
   */
  update: (id: string, data: UpdateAdminDto) => {
    return apiService.put<ApiResponse<Admin>>(
      API_ENDPOINTS.ADMINS.BY_ID(id),
      data
    );
  },

  /**
   * Delete an admin
   * DELETE /api/Admin/{id}
   */
  delete: (id: string) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.ADMINS.BY_ID(id)
    );
  },

  /**
   * Add admin role to existing user
   * POST /api/Admin/add-to-existing-user
   */
  addToExistingUser: (data: AddAdminToExistingUserDto) => {
    return apiService.post<ApiResponse<Admin>>(
      API_ENDPOINTS.ADMINS.ADD_TO_EXISTING_USER,
      data
    );
  },

  /**
   * Toggle admin active status
   */
  toggleActive: (id: string, isActive: boolean) => {
    return adminService.update(id, { isActive });
  },

  /**
   * Search admins
   */
  search: (searchTerm: string, maxResults: number = 20) => {
    const params: AdminFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };
    return adminService.getAllWithFilters(params);
  },

  /**
   * Get admins by gender
   */
  getByGender: (
    gender: 'Male' | 'Female',
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: AdminFilterParams = {
      Gender: gender,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return adminService.getAllWithFilters(params);
  },

  /**
   * Get active admins
   */
  getActive: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: AdminFilterParams = {
      IsActive: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return adminService.getAllWithFilters(params);
  },

  /**
   * Get inactive admins
   */
  getInactive: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: AdminFilterParams = {
      IsActive: false,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return adminService.getAllWithFilters(params);
  },
};

export default adminService;