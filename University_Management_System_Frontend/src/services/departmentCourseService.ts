// services/departmentCourseService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface DepartmentCourse {
  id: number;
  departmentId: number;
  departmentName: string;
  departmentCode: string;
  courseId: number;
  courseCode: string;
  courseName: string;
  credits: number;
  role: 'Major' | 'Minor' | 'Elective';
  createdAt: string;
  updatedAt: string;
}

export interface CreateDepartmentCourseDto {
  departmentId: number;
  courseId: number;
  role: 'Major' | 'Minor' | 'Elective';
}

export interface BulkCreateDepartmentCourseDto {
  departmentId: number;
  courseIds: number[];
  role: 'Major' | 'Minor' | 'Elective';
}

export interface UpdateDepartmentCourseDto {
  role: 'Major' | 'Minor' | 'Elective';
}

export interface DepartmentCourseFilterParams {
  Role?: 'Major' | 'Minor' | 'Elective';
  DepartmentId?: number;
  CourseName?: string;
  CourseCode?: string;
  MinCredits?: number;
  MaxCredits?: number;
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
// DEPARTMENT COURSE SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const departmentCourseService = {
  /**
   * Get all department courses (with optional filters)
   * GET /api/DepartmentCourse/all
   */
  getAll: (params?: DepartmentCourseFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<ApiResponse<DepartmentCourse[]>>(
        `${API_ENDPOINTS.DEPARTMENT_COURSES.ALL}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<ApiResponse<DepartmentCourse[]>>(
      API_ENDPOINTS.DEPARTMENT_COURSES.ALL
    );
  },

  /**
   * Get department courses by department
   * GET /api/DepartmentCourse/department/{departmentId}
   */
  getByDepartment: (
    departmentId: number,
    params?: DepartmentCourseFilterParams
  ) => {
    const url = API_ENDPOINTS.DEPARTMENT_COURSES.BY_DEPARTMENT(departmentId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<DepartmentCourse>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<DepartmentCourse>>(url);
  },

  /**
   * Get department courses by department with filters and pagination
   */
  getByDepartmentWithFilters: (
    departmentId: number,
    filters: DepartmentCourseFilterParams = {},
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
    return departmentCourseService.getByDepartment(departmentId, params);
  },

  /**
   * Create a new department course
   * POST /api/DepartmentCourse
   */
  create: (data: CreateDepartmentCourseDto) => {
    return apiService.post<ApiResponse<DepartmentCourse>>(
      API_ENDPOINTS.DEPARTMENT_COURSES.BASE,
      data
    );
  },

  /**
   * Create multiple department courses in bulk
   * POST /api/DepartmentCourse/bulk
   */
  createBulk: (data: BulkCreateDepartmentCourseDto) => {
    return apiService.post<ApiResponse<DepartmentCourse[]>>(
      API_ENDPOINTS.DEPARTMENT_COURSES.BULK,
      data
    );
  },

  /**
   * Update a department course role
   * PUT /api/DepartmentCourse/{departmentId}/{courseId}
   */
  update: (
    departmentId: number,
    courseId: number,
    data: UpdateDepartmentCourseDto
  ) => {
    return apiService.put<ApiResponse<DepartmentCourse>>(
      API_ENDPOINTS.DEPARTMENT_COURSES.UPDATE(departmentId, courseId),
      data
    );
  },

  /**
   * Delete a department course
   * DELETE /api/DepartmentCourse/{departmentId}/{courseId}
   */
  delete: (departmentId: number, courseId: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.DEPARTMENT_COURSES.DELETE(departmentId, courseId)
    );
  },

  /**
   * Get courses by role
   */
  getByRole: (
    role: 'Major' | 'Minor' | 'Elective',
    departmentId?: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: DepartmentCourseFilterParams = {
      Role: role,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };

    if (departmentId) {
      params.DepartmentId = departmentId;
      return departmentCourseService.getByDepartmentWithFilters(
        departmentId,
        params,
        pageNumber,
        pageSize
      );
    }

    return departmentCourseService.getAll(params);
  },

  /**
   * Get major courses
   */
  getMajorCourses: (
    departmentId?: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    return departmentCourseService.getByRole(
      'Major',
      departmentId,
      pageNumber,
      pageSize
    );
  },

  /**
   * Get minor courses
   */
  getMinorCourses: (
    departmentId?: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    return departmentCourseService.getByRole(
      'Minor',
      departmentId,
      pageNumber,
      pageSize
    );
  },

  /**
   * Get elective courses
   */
  getElectiveCourses: (
    departmentId?: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    return departmentCourseService.getByRole(
      'Elective',
      departmentId,
      pageNumber,
      pageSize
    );
  },

  /**
   * Search department courses
   */
  search: (
    searchTerm: string,
    departmentId?: number,
    maxResults: number = 20
  ) => {
    const params: DepartmentCourseFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };

    if (departmentId) {
      return departmentCourseService.getByDepartmentWithFilters(
        departmentId,
        params,
        1,
        maxResults
      );
    }

    return departmentCourseService.getAll(params);
  },

  /**
   * Check if a course exists in a department
   */
  exists: async (departmentId: number, courseId: number): Promise<boolean> => {
    try {
      const response = await departmentCourseService.getByDepartment(
        departmentId,
        {
          PageSize: 1,
        }
      );
      const courses = response.data || [];
      return courses.some(dc => dc.courseId === courseId);
    } catch {
      return false;
    }
  },

  /**
   * Get courses by department with credit range
   */
  getByCreditRange: (
    departmentId: number,
    minCredits: number,
    maxCredits: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: DepartmentCourseFilterParams = {
      MinCredits: minCredits,
      MaxCredits: maxCredits,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return departmentCourseService.getByDepartmentWithFilters(
      departmentId,
      params,
      pageNumber,
      pageSize
    );
  },

  /**
   * Get department course statistics
   */
  getStatistics: async (departmentId: number) => {
    const response = await departmentCourseService.getByDepartment(
      departmentId,
      {
        PageSize: 1000,
      }
    );
    const courses = response.data || [];

    const majorCourses = courses.filter(c => c.role === 'Major');
    const minorCourses = courses.filter(c => c.role === 'Minor');
    const electiveCourses = courses.filter(c => c.role === 'Elective');

    return {
      departmentId,
      totalCourses: courses.length,
      majorCourses: majorCourses.length,
      minorCourses: minorCourses.length,
      electiveCourses: electiveCourses.length,
      totalCredits: courses.reduce((sum, c) => sum + c.credits, 0),
      averageCredits:
        courses.length > 0
          ? courses.reduce((sum, c) => sum + c.credits, 0) / courses.length
          : 0,
    };
  },

  /**
   * Get course count by department
   */
  getCourseCount: async (departmentId: number): Promise<number> => {
    const response = await departmentCourseService.getByDepartment(
      departmentId,
      {
        PageSize: 1,
      }
    );
    return response.pagination?.totalCount ?? response.data?.length ?? 0;
  },

  /**
   * Get course count by role
   */
  getCourseCountByRole: async (
    departmentId: number,
    role: 'Major' | 'Minor' | 'Elective'
  ): Promise<number> => {
    const response = await departmentCourseService.getByDepartment(
      departmentId,
      {
        Role: role,
        PageSize: 1,
      }
    );
    return response.pagination?.totalCount ?? response.data?.length ?? 0;
  },
};

export default departmentCourseService;
