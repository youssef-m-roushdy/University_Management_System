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
   * GET /api/Assistant
   */
  getAllWithFilters: (params: AssistantFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Assistant>>(
      `${API_ENDPOINTS.ASSISTANTS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get assistants with pagination
   * GET /api/Assistant
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
    return apiService.get<PaginatedResponse<Assistant>>(url);
  },

  /**
   * Get assistants by department with filters and pagination
   * GET /api/Assistant/department/{departmentId}
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
   * Note: This is a placeholder as the update endpoint only accepts departmentId
   */
  toggleActive: (id: string, isActive: boolean) => {
    console.warn('toggleActive is a placeholder - update endpoint only accepts departmentId');
    return assistantService.update(id, { departmentId: 0 });
  },

  /**
   * Search assistants
   * GET /api/Assistant?SearchTerm={searchTerm}
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
   * GET /api/Assistant?Gender={gender}
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
   * GET /api/Assistant?IsActive=true
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
   * GET /api/Assistant?IsActive=false
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
   * GET /api/Assistant?DepartmentSearch={departmentSearch}
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

  /**
   * Get assistants by name
   * GET /api/Assistant?Name={name}
   */
  getByName: (
    name: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: AssistantFilterParams = {
      Name: name,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return assistantService.getAllWithFilters(params);
  },

  /**
   * Check if an assistant exists
   */
  exists: async (id: string): Promise<boolean> => {
    try {
      await assistantService.getById(id);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Get assistant statistics
   */
  getStatistics: async () => {
    const response = await assistantService.getAllWithPagination({}, 1, 1000);
    const assistants = response.data || [];
    
    const active = assistants.filter((a) => a.isActive).length;
    const inactive = assistants.length - active;
    const male = assistants.filter((a) => a.gender === 'Male').length;
    const female = assistants.filter((a) => a.gender === 'Female').length;
    
    // Group by department
    const departmentStats: Record<string, number> = {};
    assistants.forEach((a) => {
      departmentStats[a.departmentName] = (departmentStats[a.departmentName] || 0) + 1;
    });
    
    return {
      totalAssistants: assistants.length,
      active,
      inactive,
      male,
      female,
      departmentStats,
      topDepartments: Object.entries(departmentStats)
        .sort((a, b) => b[1] - a[1])
        .slice(0, 5),
    };
  },
};

export default assistantService;