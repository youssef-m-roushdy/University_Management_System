// services/specializationService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface Specialization {
  id: number;
  name: string;
  description: string;
  departmentId: number;
  departmentName: string;
  departmentCode: string;
  studentCount: number;
  courseCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateSpecializationDto {
  name: string;
  description: string;
  departmentId: number;
}

export interface UpdateSpecializationDto {
  name: string;
  description: string;
  departmentId: number;
}

export interface SpecializationFilterParams {
  DepartmentId?: number;
  Name?: string;
  HasStudents?: boolean;
  HasCourses?: boolean;
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
// SPECIALIZATION SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const specializationService = {
  /**
   * Get all specializations (with optional filters)
   * GET /api/Specialization
   */
  getAll: (params?: SpecializationFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Specialization>>(
        `${API_ENDPOINTS.SPECIALIZATIONS.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Specialization>>(
      API_ENDPOINTS.SPECIALIZATIONS.BASE
    );
  },

  /**
   * Get specializations with filters
   */
  getAllWithFilters: (params: SpecializationFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Specialization>>(
      `${API_ENDPOINTS.SPECIALIZATIONS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get specializations with pagination
   */
  getAllWithPagination: (
    filters: SpecializationFilterParams = {},
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
    return specializationService.getAllWithFilters(params);
  },

  /**
   * Get specialization by ID
   * GET /api/Specialization/{id}
   */
  getById: (id: number) => {
    return apiService.get<ApiResponse<Specialization>>(
      API_ENDPOINTS.SPECIALIZATIONS.BY_ID(id)
    );
  },

  /**
   * Create a new specialization
   * POST /api/Specialization
   */
  create: (data: CreateSpecializationDto) => {
    return apiService.post<ApiResponse<Specialization>>(
      API_ENDPOINTS.SPECIALIZATIONS.BASE,
      data
    );
  },

  /**
   * Update a specialization
   * PUT /api/Specialization/{id}
   */
  update: (id: number, data: UpdateSpecializationDto) => {
    return apiService.put<ApiResponse<Specialization>>(
      API_ENDPOINTS.SPECIALIZATIONS.BY_ID(id),
      data
    );
  },

  /**
   * Delete a specialization
   * DELETE /api/Specialization/{id}
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.SPECIALIZATIONS.BY_ID(id)
    );
  },

  /**
   * Get specializations by department
   * GET /api/Specialization/department/{departmentId}
   */
  getByDepartment: (
    departmentId: number,
    params?: SpecializationFilterParams
  ) => {
    const url = API_ENDPOINTS.SPECIALIZATIONS.BY_DEPARTMENT(departmentId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Specialization>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Specialization>>(url);
  },

  /**
   * Get specializations by department with pagination
   */
  getByDepartmentWithPagination: (
    departmentId: number,
    filters: SpecializationFilterParams = {},
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
    return specializationService.getByDepartment(departmentId, params);
  },

  /**
   * Get specializations with students
   */
  getWithStudents: (pageNumber: number = 1, pageSize: number = 10) => {
    const params: SpecializationFilterParams = {
      HasStudents: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return specializationService.getAllWithFilters(params);
  },

  /**
   * Get specializations with courses
   */
  getWithCourses: (pageNumber: number = 1, pageSize: number = 10) => {
    const params: SpecializationFilterParams = {
      HasCourses: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return specializationService.getAllWithFilters(params);
  },

  /**
   * Get specializations by name
   */
  getByName: (name: string, pageNumber: number = 1, pageSize: number = 10) => {
    const params: SpecializationFilterParams = {
      Name: name,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return specializationService.getAllWithFilters(params);
  },

  /**
   * Search specializations
   */
  search: (
    searchTerm: string,
    departmentId?: number,
    maxResults: number = 20
  ) => {
    const params: SpecializationFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };

    if (departmentId) {
      return specializationService.getByDepartment(departmentId, params);
    }
    return specializationService.getAllWithFilters(params);
  },

  /**
   * Check if a specialization exists
   */
  exists: async (id: number): Promise<boolean> => {
    try {
      await specializationService.getById(id);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Get specialization by name exact match
   */
  getByNameExact: async (
    name: string,
    departmentId?: number
  ): Promise<Specialization | null> => {
    const params: SpecializationFilterParams = {
      Name: name,
      PageSize: 1,
    };

    let response;
    if (departmentId) {
      response = await specializationService.getByDepartment(
        departmentId,
        params
      );
    } else {
      response = await specializationService.getAllWithFilters(params);
    }

    const specializations = response.data || [];
    return specializations.find(s => s.name === name) || null;
  },

  /**
   * Get specializations by department with student count
   */
  getByDepartmentWithStudentCount: async (
    departmentId: number
  ): Promise<Specialization[]> => {
    const response = await specializationService.getByDepartment(departmentId, {
      PageSize: 1000, // Get all
    });
    return response.data || [];
  },

  /**
   * Get specialization statistics
   */
  getStatistics: async () => {
    const response = await specializationService.getAllWithPagination(
      {},
      1,
      1000
    );
    const specializations = response.data || [];

    const totalSpecializations = specializations.length;
    const withStudents = specializations.filter(s => s.studentCount > 0).length;
    const withCourses = specializations.filter(s => s.courseCount > 0).length;
    const totalStudents = specializations.reduce(
      (sum, s) => sum + s.studentCount,
      0
    );
    const totalCourses = specializations.reduce(
      (sum, s) => sum + s.courseCount,
      0
    );

    return {
      totalSpecializations,
      withStudents,
      withCourses,
      totalStudents,
      totalCourses,
      averageStudentsPerSpecialization:
        totalSpecializations > 0 ? totalStudents / totalSpecializations : 0,
      averageCoursesPerSpecialization:
        totalSpecializations > 0 ? totalCourses / totalSpecializations : 0,
    };
  },

  /**
   * Get department specialization summary
   */
  getDepartmentSummary: async (departmentId: number) => {
    const response = await specializationService.getByDepartment(departmentId, {
      PageSize: 1000,
    });
    const specializations = response.data || [];

    return {
      departmentId,
      totalSpecializations: specializations.length,
      specializations: specializations.map(s => ({
        id: s.id,
        name: s.name,
        studentCount: s.studentCount,
        courseCount: s.courseCount,
      })),
      totalStudents: specializations.reduce(
        (sum, s) => sum + s.studentCount,
        0
      ),
      totalCourses: specializations.reduce((sum, s) => sum + s.courseCount, 0),
    };
  },
};

export default specializationService;
