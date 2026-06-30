// services/instructorService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface Instructor {
  id: string;
  name: string;
  userName: string;
  email: string;
  phoneNumber: string;
  profilePicture: string;
  address: string;
  gender: 'Male' | 'Female';
  isActive: boolean;
  departmentId: number;
  departmentName: string;
  departmentCode: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateInstructorDto {
  email: string;
  password: string;
  name: string;
  userName: string;
  phoneNumber: string;
  address: string;
  gender: 'Male' | 'Female';
  isActive: boolean;
  departmentId: number;
}

export interface UpdateInstructorDto {
  departmentId: number;
}

export interface AddInstructorToExistingUserDto {
  userEmail: string;
  departmentId: number;
}

export interface InstructorFilterParams {
  Name?: string;
  Gender?: 'Male' | 'Female';
  DepartmentSearch?: string;
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
// INSTRUCTOR SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const instructorService = {
  /**
   * Get all instructors (with optional filters)
   * GET /api/Instructor
   */
  getAll: (params?: InstructorFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Instructor>>(
        `${API_ENDPOINTS.INSTRUCTORS.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Instructor>>(
      API_ENDPOINTS.INSTRUCTORS.BASE
    );
  },

  /**
   * Get instructors with filters
   */
  getAllWithFilters: (params: InstructorFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Instructor>>(
      `${API_ENDPOINTS.INSTRUCTORS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get instructors with pagination
   */
  getAllWithPagination: (
    filters: InstructorFilterParams = {},
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
    return instructorService.getAllWithFilters(params);
  },

  /**
   * Get instructor by ID
   * GET /api/Instructor/{id}
   */
  getById: (id: string) => {
    return apiService.get<ApiResponse<Instructor>>(
      API_ENDPOINTS.INSTRUCTORS.BY_ID(id)
    );
  },

  /**
   * Create a new instructor
   * POST /api/Instructor
   */
  create: (data: CreateInstructorDto) => {
    return apiService.post<ApiResponse<Instructor>>(
      API_ENDPOINTS.INSTRUCTORS.BASE,
      data
    );
  },

  /**
   * Update an instructor
   * PUT /api/Instructor/{id}
   */
  update: (id: string, data: UpdateInstructorDto) => {
    return apiService.put<ApiResponse<Instructor>>(
      API_ENDPOINTS.INSTRUCTORS.BY_ID(id),
      data
    );
  },

  /**
   * Delete an instructor
   * DELETE /api/Instructor/{id}
   */
  delete: (id: string) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.INSTRUCTORS.BY_ID(id)
    );
  },

  /**
   * Get instructors by department
   * GET /api/Instructor/department/{departmentId}
   */
  getByDepartment: (
    departmentId: number,
    params?: InstructorFilterParams
  ) => {
    const url = API_ENDPOINTS.INSTRUCTORS.BY_DEPARTMENT(departmentId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Instructor>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Instructor>>(url);
  },

  /**
   * Get instructors by department with filters and pagination
   */
  getByDepartmentWithFilters: (
    departmentId: number,
    filters: InstructorFilterParams = {},
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
    return instructorService.getByDepartment(departmentId, params);
  },

  /**
   * Add instructor role to existing user
   * POST /api/Instructor/add-to-existing-user
   */
  addToExistingUser: (data: AddInstructorToExistingUserDto) => {
    return apiService.post<ApiResponse<Instructor>>(
      API_ENDPOINTS.INSTRUCTORS.ADD_TO_EXISTING_USER,
      data
    );
  },

  /**
   * Toggle instructor active status
   * Note: This is a placeholder as the update endpoint only accepts departmentId
   * You may need to implement this differently based on your API
   */
  toggleActive: (id: string, isActive: boolean) => {
    // This is a placeholder - the actual implementation depends on your API
    // If your API supports updating isActive, you would need to modify this
    console.warn('toggleActive is a placeholder - update endpoint only accepts departmentId');
    return instructorService.update(id, { departmentId: 0 });
  },

  /**
   * Search instructors
   */
  search: (searchTerm: string, maxResults: number = 20) => {
    const params: InstructorFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };
    return instructorService.getAllWithFilters(params);
  },

  /**
   * Get instructors by gender
   */
  getByGender: (
    gender: 'Male' | 'Female',
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: InstructorFilterParams = {
      Gender: gender,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return instructorService.getAllWithFilters(params);
  },

  /**
   * Get active instructors
   */
  getActive: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: InstructorFilterParams = {
      IsActive: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return instructorService.getAllWithFilters(params);
  },

  /**
   * Get inactive instructors
   */
  getInactive: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: InstructorFilterParams = {
      IsActive: false,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return instructorService.getAllWithFilters(params);
  },

  /**
   * Get instructors by department search
   */
  getByDepartmentSearch: (
    departmentSearch: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: InstructorFilterParams = {
      DepartmentSearch: departmentSearch,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return instructorService.getAllWithFilters(params);
  },

  /**
   * Get instructors by name
   */
  getByName: (
    name: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: InstructorFilterParams = {
      Name: name,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return instructorService.getAllWithFilters(params);
  },
};

export default instructorService;