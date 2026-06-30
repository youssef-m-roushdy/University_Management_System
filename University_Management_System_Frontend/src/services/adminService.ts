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
    return apiService.get<PaginatedResponse<Admin>>(API_ENDPOINTS.ADMINS.BASE);
  },

  /**
   * Get admins with filters
   * GET /api/Admin
   */
  getAllWithFilters: (params: AdminFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Admin>>(
      `${API_ENDPOINTS.ADMINS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get admins with pagination
   * GET /api/Admin
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
    return apiService.get<ApiResponse<Admin>>(API_ENDPOINTS.ADMINS.BY_ID(id));
  },

  /**
   * Create a new admin
   * POST /api/Admin
   */
  create: (data: CreateAdminDto) => {
    return apiService.post<ApiResponse<Admin>>(API_ENDPOINTS.ADMINS.BASE, data);
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
   * PATCH /api/Admin/{id}
   */
  toggleActive: (id: string, isActive: boolean) => {
    return adminService.update(id, { isActive });
  },

  /**
   * Search admins
   * GET /api/Admin?SearchTerm={searchTerm}
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
   * GET /api/Admin?Gender={gender}
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
   * GET /api/Admin?IsActive=true
   */
  getActive: (pageNumber: number = 1, pageSize: number = 10) => {
    const params: AdminFilterParams = {
      IsActive: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return adminService.getAllWithFilters(params);
  },

  /**
   * Get inactive admins
   * GET /api/Admin?IsActive=false
   */
  getInactive: (pageNumber: number = 1, pageSize: number = 10) => {
    const params: AdminFilterParams = {
      IsActive: false,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return adminService.getAllWithFilters(params);
  },

  /**
   * Get admins by name
   * GET /api/Admin?Name={name}
   */
  getByName: (name: string, pageNumber: number = 1, pageSize: number = 10) => {
    const params: AdminFilterParams = {
      Name: name,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return adminService.getAllWithFilters(params);
  },

  /**
   * Check if an admin exists
   */
  exists: async (id: string): Promise<boolean> => {
    try {
      await adminService.getById(id);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Check if an admin exists by email
   */
  existsByEmail: async (email: string): Promise<boolean> => {
    try {
      const response = await adminService.getAllWithFilters({
        SearchTerm: email,
        PageSize: 1,
      });
      const admins = response.data || [];
      return admins.some(admin => admin.email === email);
    } catch {
      return false;
    }
  },

  /**
   * Get admin statistics
   */
  getStatistics: async () => {
    const response = await adminService.getAllWithPagination({}, 1, 1000);
    const admins = response.data || [];

    const active = admins.filter(a => a.isActive).length;
    const inactive = admins.length - active;
    const male = admins.filter(a => a.gender === 'Male').length;
    const female = admins.filter(a => a.gender === 'Female').length;

    return {
      totalAdmins: admins.length,
      active,
      inactive,
      male,
      female,
      activePercentage: admins.length > 0 ? (active / admins.length) * 100 : 0,
      malePercentage: admins.length > 0 ? (male / admins.length) * 100 : 0,
      femalePercentage: admins.length > 0 ? (female / admins.length) * 100 : 0,
    };
  },

  /**
   * Get admin by email
   */
  getByEmail: async (email: string): Promise<Admin | null> => {
    try {
      const response = await adminService.getAllWithFilters({
        SearchTerm: email,
        PageSize: 1,
      });
      const admins = response.data || [];
      return admins.find(admin => admin.email === email) || null;
    } catch {
      return null;
    }
  },

  /**
   * Bulk update admin status
   */
  bulkUpdateStatus: async (ids: string[], isActive: boolean): Promise<void> => {
    const promises = ids.map(id => adminService.toggleActive(id, isActive));
    await Promise.all(promises);
  },
};

export default adminService;
