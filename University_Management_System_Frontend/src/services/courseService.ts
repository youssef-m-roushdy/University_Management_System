// services/courseService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface Course {
  id: number;
  code: string;
  name: string;
  description: string;
  credits: number;
  status: 'Opened' | 'Closed';
  departmentId: number;
  departmentName: string;
  createdAt: string;
  updatedAt: string;
  prerequisitesCount: number;
  dependenciesCount: number;
}

export interface CourseSearchResult {
  id: number;
  code: string;
  name: string;
}

export interface CreateCourseDto {
  code: string;
  name: string;
  description: string;
  credits: number;
  status: 'Opened' | 'Closed';
  departmentId: number;
}

export interface UpdateCourseDto {
  code: string;
  name: string;
  description: string;
  credits: number;
  status: 'Opened' | 'Closed';
  departmentId: number;
}

export interface UpdateCourseStatusDto {
  status: 'Opened' | 'Closed';
}

export interface CourseFilterParams {
  Status?: 'Opened' | 'Closed';
  Code?: string;
  Name?: string;
  DepartmentId?: number;
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
// COURSE SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const courseService = {
  /**
   * Get all courses (with optional filters)
   */
  getAll: (params?: CourseFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Course>>(
        `${API_ENDPOINTS.COURSES.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Course>>(
      API_ENDPOINTS.COURSES.BASE
    );
  },

  /**
   * Get courses with filters
   */
  getAllWithFilters: (params: CourseFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Course>>(
      `${API_ENDPOINTS.COURSES.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get courses with pagination
   */
  getAllWithPagination: (
    filters: CourseFilterParams = {},
    pageNumber: number = 1,
    pageSize: number = 10,
    sortBy: string | null = null,
    sortDirection: 'Ascending' | 'Descending' = 'Ascending'
  ) => {
    // Fix: Convert null to undefined
    const sortByParam = sortBy || undefined;
    const params = createFilterPageParams(
      filters,
      pageNumber,
      pageSize,
      sortByParam,
      sortDirection
    );
    return courseService.getAllWithFilters(params);
  },

  /**
   * Get course by ID
   */
  getById: (id: number) => {
    return apiService.get<ApiResponse<Course>>(API_ENDPOINTS.COURSES.BY_ID(id));
  },

  /**
   * Create a new course
   */
  create: (data: CreateCourseDto) => {
    return apiService.post<ApiResponse<Course>>(
      API_ENDPOINTS.COURSES.BASE,
      data
    );
  },

  /**
   * Update a course
   */
  update: (id: number, data: UpdateCourseDto) => {
    return apiService.put<ApiResponse<Course>>(
      API_ENDPOINTS.COURSES.BY_ID(id),
      data
    );
  },

  /**
   * Delete a course
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.COURSES.BY_ID(id)
    );
  },

  /**
   * Update course status
   */
  updateStatus: (id: number, data: UpdateCourseStatusDto) => {
    return apiService.patch<ApiResponse<Course>>(
      API_ENDPOINTS.COURSES.UPDATE_STATUS(id),
      data
    );
  },

  /**
   * Get course uploads
   */
  getUploads: (id: number) => {
    return apiService.get(API_ENDPOINTS.COURSES.UPLOADS(id));
  },

  /**
   * Get course registrations
   */
  getRegistrations: (id: number, yearId: number) => {
    return apiService.get(API_ENDPOINTS.COURSES.REGISTRATIONS(id, yearId));
  },

  /**
   * Upload file for a course
   */
  uploadFile: (courseId: number, formData: FormData) => {
    return apiService.postForm(
      API_ENDPOINTS.COURSES.UPLOAD_FILE(courseId),
      formData
    );
  },

  /**
   * Get courses by department with filters
   */
  getDeptCourses: (
    deptId: number,
    filters: CourseFilterParams = {},
    pageNumber: number = 1,
    pageSize: number = 10,
    sortBy: string | null = null,
    sortDirection: 'Ascending' | 'Descending' = 'Ascending'
  ) => {
    const url = API_ENDPOINTS.COURSES.DEPT_COURSES(deptId);
    // Fix: Convert null to undefined
    const sortByParam = sortBy || undefined;
    const params = createFilterPageParams(
      filters,
      pageNumber,
      pageSize,
      sortByParam,
      sortDirection
    );
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Course>>(
      `${url}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get course prerequisites
   */
  getPrerequisites: (id: number) => {
    return apiService.get<ApiResponse<Course[]>>(
      API_ENDPOINTS.COURSES.PREREQUISITES(id)
    );
  },

  /**
   * Get course dependencies
   */
  getDependencies: (id: number) => {
    return apiService.get<ApiResponse<Course[]>>(
      API_ENDPOINTS.COURSES.DEPENDENCIES(id)
    );
  },

  /**
   * Get open courses by department
   */
  getDeptOpenCourses: (deptId: number) => {
    return apiService.get<ApiResponse<Course[]>>(
      API_ENDPOINTS.COURSES.OPEN_BY_DEPT(deptId)
    );
  },

  /**
   * Search courses
   */
  search: (q: string, departmentId?: number, maxResults: number = 20) => {
    const params = new URLSearchParams();
    if (q) params.append('q', q);
    if (departmentId) params.append('departmentId', departmentId.toString());
    if (maxResults) params.append('maxResults', maxResults.toString());

    const url = `${API_ENDPOINTS.COURSES.SEARCH}?${params.toString()}`;
    return apiService.get<ApiResponse<CourseSearchResult[]>>(url);
  },

  /**
   * Get student registration courses
   */
  getStudentRegistrationCourses: (
    userId: string,
    studyYearId: number,
    semesterId: number
  ) => {
    return apiService.get(
      API_ENDPOINTS.COURSES.STUDENT_REGISTRATION_COURSES(
        studyYearId,
        semesterId
      )
    );
  },
};

export default courseService;
