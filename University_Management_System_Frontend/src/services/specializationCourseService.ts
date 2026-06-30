// services/specializationCourseService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export type SpecializationCourseRole = 'Core' | 'Specialization_Core' | 'Elective';

export interface SpecializationCourse {
  id: number;
  specializationId: number;
  specializationName: string;
  courseId: number;
  courseCode: string;
  courseName: string;
  credits: number;
  role: SpecializationCourseRole;
  departmentName: string;
  prerequisitesCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateSpecializationCourseDto {
  specializationId: number;
  courseId: number;
  role: SpecializationCourseRole;
}

export interface BulkCreateSpecializationCourseDto {
  specializationId: number;
  courseIds: number[];
  role: SpecializationCourseRole;
}

export interface UpdateSpecializationCourseDto {
  role: SpecializationCourseRole;
}

export interface SpecializationCourseFilterParams {
  SpecializationId?: number;
  CourseId?: number;
  Role?: SpecializationCourseRole;
  CourseName?: string;
  CourseCode?: string;
  MinCredits?: number;
  MaxCredits?: number;
  HasPrerequisites?: boolean;
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
// SPECIALIZATION COURSE SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const specializationCourseService = {
  /**
   * Get all specialization courses (with optional filters)
   * GET /api/SpecializationCourse
   */
  getAll: (params?: SpecializationCourseFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<SpecializationCourse>>(
        `${API_ENDPOINTS.SPECIALIZATION_COURSES.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<SpecializationCourse>>(
      API_ENDPOINTS.SPECIALIZATION_COURSES.BASE
    );
  },

  /**
   * Get specialization courses with filters
   */
  getAllWithFilters: (params: SpecializationCourseFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<SpecializationCourse>>(
      `${API_ENDPOINTS.SPECIALIZATION_COURSES.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get specialization courses with pagination
   */
  getAllWithPagination: (
    filters: SpecializationCourseFilterParams = {},
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
    return specializationCourseService.getAllWithFilters(params);
  },

  /**
   * Create a new specialization course
   * POST /api/SpecializationCourse
   */
  create: (data: CreateSpecializationCourseDto) => {
    return apiService.post<ApiResponse<SpecializationCourse>>(
      API_ENDPOINTS.SPECIALIZATION_COURSES.BASE,
      data
    );
  },

  /**
   * Create multiple specialization courses in bulk
   * POST /api/SpecializationCourse/bulk
   */
  createBulk: (data: BulkCreateSpecializationCourseDto) => {
    return apiService.post<ApiResponse<SpecializationCourse[]>>(
      API_ENDPOINTS.SPECIALIZATION_COURSES.BULK,
      data
    );
  },

  /**
   * Get specialization courses by specialization
   * GET /api/SpecializationCourse/specialization/{specializationId}
   */
  getBySpecialization: (
    specializationId: number,
    params?: SpecializationCourseFilterParams
  ) => {
    const url = API_ENDPOINTS.SPECIALIZATION_COURSES.BY_SPECIALIZATION(specializationId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<SpecializationCourse>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<SpecializationCourse>>(url);
  },

  /**
   * Get specialization courses by specialization with pagination
   */
  getBySpecializationWithPagination: (
    specializationId: number,
    filters: SpecializationCourseFilterParams = {},
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
    return specializationCourseService.getBySpecialization(specializationId, params);
  },

  /**
   * Get specialization courses by course
   * GET /api/SpecializationCourse/course/{courseId}
   */
  getByCourse: (courseId: number) => {
    return apiService.get<ApiResponse<SpecializationCourse[]>>(
      API_ENDPOINTS.SPECIALIZATION_COURSES.BY_COURSE(courseId)
    );
  },

  /**
   * Update a specialization course
   * PUT /api/SpecializationCourse/{specializationId}/{courseId}
   */
  update: (
    specializationId: number,
    courseId: number,
    data: UpdateSpecializationCourseDto
  ) => {
    return apiService.put<ApiResponse<SpecializationCourse>>(
      API_ENDPOINTS.SPECIALIZATION_COURSES.UPDATE(specializationId, courseId),
      data
    );
  },

  /**
   * Delete a specialization course
   * DELETE /api/SpecializationCourse/{specializationId}/{courseId}
   */
  delete: (specializationId: number, courseId: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.SPECIALIZATION_COURSES.DELETE(specializationId, courseId)
    );
  },

  /**
   * Get core courses for a specialization
   */
  getCoreCourses: (
    specializationId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SpecializationCourseFilterParams = {
      Role: 'Core',
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return specializationCourseService.getBySpecialization(specializationId, params);
  },

  /**
   * Get specialization core courses
   */
  getSpecializationCoreCourses: (
    specializationId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SpecializationCourseFilterParams = {
      Role: 'Specialization_Core',
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return specializationCourseService.getBySpecialization(specializationId, params);
  },

  /**
   * Get elective courses for a specialization
   */
  getElectiveCourses: (
    specializationId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SpecializationCourseFilterParams = {
      Role: 'Elective',
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return specializationCourseService.getBySpecialization(specializationId, params);
  },

  /**
   * Get courses by role
   */
  getByRole: (
    role: SpecializationCourseRole,
    specializationId?: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SpecializationCourseFilterParams = {
      Role: role,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    
    if (specializationId) {
      return specializationCourseService.getBySpecialization(specializationId, params);
    }
    return specializationCourseService.getAllWithFilters(params);
  },

  /**
   * Get courses with prerequisites
   */
  getWithPrerequisites: (
    specializationId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SpecializationCourseFilterParams = {
      HasPrerequisites: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return specializationCourseService.getBySpecialization(specializationId, params);
  },

  /**
   * Get courses without prerequisites
   */
  getWithoutPrerequisites: (
    specializationId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SpecializationCourseFilterParams = {
      HasPrerequisites: false,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return specializationCourseService.getBySpecialization(specializationId, params);
  },

  /**
   * Get courses by credit range
   */
  getByCreditRange: (
    specializationId: number,
    minCredits: number,
    maxCredits: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SpecializationCourseFilterParams = {
      MinCredits: minCredits,
      MaxCredits: maxCredits,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return specializationCourseService.getBySpecialization(specializationId, params);
  },

  /**
   * Search specialization courses
   */
  search: (
    searchTerm: string,
    specializationId?: number,
    maxResults: number = 20
  ) => {
    const params: SpecializationCourseFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };
    
    if (specializationId) {
      return specializationCourseService.getBySpecialization(specializationId, params);
    }
    return specializationCourseService.getAllWithFilters(params);
  },

  /**
   * Check if a course exists in a specialization
   */
  exists: async (
    specializationId: number,
    courseId: number
  ): Promise<boolean> => {
    try {
      const response = await specializationCourseService.getBySpecialization(specializationId, {
        CourseId: courseId,
        PageSize: 1,
      });
      const courses = response.data || [];
      return courses.length > 0;
    } catch {
      return false;
    }
  },

  /**
   * Get course count by specialization
   */
  getCourseCount: async (specializationId: number): Promise<number> => {
    const response = await specializationCourseService.getBySpecialization(specializationId, {
      PageSize: 1,
    });
    return response.pagination?.totalCount ?? response.data?.length ?? 0;
  },

  /**
   * Get course count by role
   */
  getCourseCountByRole: async (
    specializationId: number,
    role: SpecializationCourseRole
  ): Promise<number> => {
    const response = await specializationCourseService.getBySpecialization(specializationId, {
      Role: role,
      PageSize: 1,
    });
    return response.pagination?.totalCount ?? response.data?.length ?? 0;
  },

  /**
   * Get specialization course statistics
   */
  getStatistics: async (specializationId: number) => {
    const response = await specializationCourseService.getBySpecialization(specializationId, {
      PageSize: 1000,
    });
    const courses = response.data || [];
    
    const coreCourses = courses.filter((c) => c.role === 'Core');
    const specCoreCourses = courses.filter((c) => c.role === 'Specialization_Core');
    const electiveCourses = courses.filter((c) => c.role === 'Elective');
    const withPrereqs = courses.filter((c) => c.prerequisitesCount > 0);
    
    return {
      totalCourses: courses.length,
      coreCourses: coreCourses.length,
      specializationCoreCourses: specCoreCourses.length,
      electiveCourses: electiveCourses.length,
      coursesWithPrerequisites: withPrereqs.length,
      coursesWithoutPrerequisites: courses.length - withPrereqs.length,
      totalCredits: courses.reduce((sum, c) => sum + c.credits, 0),
      averageCredits: courses.length > 0 
        ? courses.reduce((sum, c) => sum + c.credits, 0) / courses.length 
        : 0,
    };
  },

  /**
   * Replace all courses for a specialization
   */
  replaceCourses: async (
    specializationId: number,
    courseIds: number[],
    role: SpecializationCourseRole = 'Core'
  ): Promise<any> => {
    // First, get existing courses for this specialization
    const existing = await specializationCourseService.getBySpecialization(specializationId, {
      PageSize: 1000,
    });
    const existingCourses = existing.data || [];
    
    // Remove all existing courses
    const deletePromises = existingCourses.map((sc) =>
      specializationCourseService.delete(specializationId, sc.courseId)
    );
    await Promise.all(deletePromises);
    
    // Add new courses in bulk
    if (courseIds.length > 0) {
      return specializationCourseService.createBulk({
        specializationId,
        courseIds,
        role,
      });
    }
    
    return { data: [] };
  },
};

export default specializationCourseService;