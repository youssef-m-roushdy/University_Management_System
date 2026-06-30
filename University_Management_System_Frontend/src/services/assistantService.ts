// services/assistantService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface Assistant {
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

export interface CreateAssistantDto {
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

export interface UpdateAssistantDto {
  departmentId: number;
}

export interface AddAssistantToExistingUserDto {
  userEmail: string;
  departmentId: number;
}

export interface AssistantFilterParams {
  Name?: string;
  Gender?: 'Male' | 'Female';
  DepartmentId?: number;
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
// ASSISTANT SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const assistantService = {
  /**
   * Get all assistants (with optional filters)
   * GET /api/Assistant
   */
  getAll: (params?: AssistantFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Assistant>>(
        `${API_ENDPOINTS.ASSISTANTS.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Assistant>>(
      API_ENDPOINTS.ASSISTANTS.BASE
    );
  },

  /**
   * Get assistants with filters
   */
  getAllWithFilters: (params: AssistantFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Assistant>>(
      `${API_ENDPOINTS.ASSISTANTS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get assistants with pagination
   */
  getAllWithPagination: (
    filters: AssistantFilterParams = {},
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
    return assistantService.getAllWithFilters(params);
  },

  /**
   * Get assistant by ID
   * GET /api/Assistant/{id}
   */
  getById: (id: string) => {
    return apiService.get<ApiResponse<Assistant>>(
      API_ENDPOINTS.ASSISTANTS.BY_ID(id)
    );
  },

  /**
   * Create a new assistant
   * POST /api/Assistant
   */
  create: (data: CreateAssistantDto) => {
    return apiService.post<ApiResponse<Assistant>>(
      API_ENDPOINTS.ASSISTANTS.BASE,
      data
    );
  },

  /**
   * Update an assistant
   * PUT /api/Assistant/{id}
   */
  update: (id: string, data: UpdateAssistantDto) => {
    return apiService.put<ApiResponse<Assistant>>(
      API_ENDPOINTS.ASSISTANTS.BY_ID(id),
      data
    );
  },

  /**
   * Delete an assistant
   * DELETE /api/Assistant/{id}
   */
  delete: (id: string) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.ASSISTANTS.BY_ID(id)
    );
  },

  /**
   * Get assistants by department
   * GET /api/Assistant/department/{departmentId}
   */
  getByDepartment: (
    departmentId: number,
    params?: AssistantFilterParams
  ) => {
    const url = API_ENDPOINTS.ASSISTANTS.BY_DEPARTMENT(departmentId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Assistant>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Assistant>>(
      url
    );
  },

  /**
   * Get assistants by department with filters and pagination
   */
  getByDepartmentWithFilters: (
    departmentId: number,
    filters: AssistantFilterParams = {},
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
    return assistantService.getByDepartment(departmentId, params);
  },

  /**
   * Add assistant role to existing user
   * POST /api/Assistant/add-to-existing-user
   */
  addToExistingUser: (data: AddAssistantToExistingUserDto) => {
    return apiService.post<ApiResponse<Assistant>>(
      API_ENDPOINTS.ASSISTANTS.ADD_TO_EXISTING_USER,
      data
    );
  },

  /**
   * Toggle assistant active status
   */
  toggleActive: (id: string, isActive: boolean) => {
    // Note: Update endpoint only accepts departmentId
    // You might need to use a different endpoint or full update
    // This is a placeholder - you may need to implement this differently
    return assistantService.update(id, { departmentId: 0 }); // Placeholder
  },

  /**
   * Search assistants
   */
  search: (searchTerm: string, maxResults: number = 20) => {
    const params: AssistantFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };
    return assistantService.getAllWithFilters(params);
  },

  /**
   * Get assistants by gender
   */
  getByGender: (
    gender: 'Male' | 'Female',
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: AssistantFilterParams = {
      Gender: gender,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return assistantService.getAllWithFilters(params);
  },

  /**
   * Get active assistants
   */
  getActive: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: AssistantFilterParams = {
      IsActive: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return assistantService.getAllWithFilters(params);
  },

  /**
   * Get inactive assistants
   */
  getInactive: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: AssistantFilterParams = {
      IsActive: false,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return assistantService.getAllWithFilters(params);
  },

  /**
   * Get assistants by department search
   */
  getByDepartmentSearch: (
    departmentSearch: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: AssistantFilterParams = {
      DepartmentSearch: departmentSearch,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return assistantService.getAllWithFilters(params);
  },
};

export default assistantService;