// services/academicScheduleService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface AcademicSchedule {
  id: number;
  title: string;
  url: string;
  description: string;
  departmentId: number;
  departmentName: string;
  departmentCode: string;
  semesterId: number;
  semesterTitle: string;
  studyYearId: number;
  studyYearRange: string;
  adminName: string;
  scheduleDate: string;
  scheduleDateDisplay: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateAcademicScheduleDto {
  title: string;
  description: string;
  departmentId: number;
  semesterId: number;
  studyYearId: number;
  scheduleDate: string;
  file?: File;
}

export interface AcademicScheduleFilterParams {
  DepartmentId?: string;
  ScheduleDateFrom?: string;
  ScheduleDateTo?: string;
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

// Reuse existing PaginatedResponse and ApiResponse interfaces
// export interface PaginatedResponse<T> { ... }
// export interface ApiResponse<T> { ... }

// ──────────────────────────────────────────────────────────────────────────────
// ACADEMIC SCHEDULE SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const academicScheduleService = {
  /**
   * Get all academic schedules by department
   */
  getByDepartment: (
    departmentId: number,
    params?: AcademicScheduleFilterParams
  ) => {
    const url = API_ENDPOINTS.ACADEMIC_SCHEDULES.BY_DEPARTMENT(departmentId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<AcademicSchedule>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<AcademicSchedule>>(url);
  },

  /**
   * Get academic schedules by department with filters and pagination
   */
  getByDepartmentWithFilters: (
    departmentId: number,
    filters: AcademicScheduleFilterParams = {},
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
    return academicScheduleService.getByDepartment(departmentId, params);
  },

  /**
   * Get academic schedules by department and semester
   */
  getByDepartmentAndSemester: (
    departmentId: number,
    semesterId: number,
    params?: AcademicScheduleFilterParams
  ) => {
    const url = API_ENDPOINTS.ACADEMIC_SCHEDULES.BY_DEPARTMENT_SEMESTER(
      departmentId,
      semesterId
    );
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<AcademicSchedule>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<AcademicSchedule>>(url);
  },

  /**
   * Get academic schedules by department and semester with filters and pagination
   */
  getByDepartmentAndSemesterWithFilters: (
    departmentId: number,
    semesterId: number,
    filters: AcademicScheduleFilterParams = {},
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
    return academicScheduleService.getByDepartmentAndSemester(
      departmentId,
      semesterId,
      params
    );
  },

  /**
   * Get academic schedule by ID
   */
  getById: (id: number) => {
    return apiService.get<ApiResponse<AcademicSchedule>>(
      API_ENDPOINTS.ACADEMIC_SCHEDULES.BY_ID(id)
    );
  },

  /**
   * Create a new academic schedule (with file upload)
   */
  create: (data: CreateAcademicScheduleDto) => {
    const formData = new FormData();
    formData.append('Title', data.title);
    formData.append('Description', data.description);
    formData.append('DepartmentId', data.departmentId.toString());
    formData.append('SemesterId', data.semesterId.toString());
    formData.append('StudyYearId', data.studyYearId.toString());
    formData.append('ScheduleDate', data.scheduleDate);
    if (data.file) {
      formData.append('File', data.file);
    }

    return apiService.postForm<ApiResponse<AcademicSchedule>>(
      API_ENDPOINTS.ACADEMIC_SCHEDULES.BASE,
      formData
    );
  },

  /**
   * Delete an academic schedule
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.ACADEMIC_SCHEDULES.BY_ID(id)
    );
  },

  /**
   * Helper: Get all schedules for a specific department (convenience method)
   */
  getSchedulesByDepartment: (
    departmentId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    return academicScheduleService.getByDepartmentWithFilters(
      departmentId,
      {},
      pageNumber,
      pageSize
    );
  },

  /**
   * Helper: Get all schedules for a specific semester (convenience method)
   */
  getSchedulesBySemester: (
    semesterId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    // Note: This requires the semester endpoint which might be different
    // Using the department+semester endpoint with a generic departmentId
    // You might want to add a specific endpoint for this if needed
    return academicScheduleService.getByDepartmentAndSemesterWithFilters(
      0, // This might need adjustment based on your API
      semesterId,
      {},
      pageNumber,
      pageSize
    );
  },

  /**
   * Search academic schedules
   */
  search: (
    searchTerm: string,
    departmentId?: number,
    semesterId?: number,
    maxResults: number = 20
  ) => {
    const params: AcademicScheduleFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };
    
    if (departmentId) {
      params.DepartmentId = departmentId.toString();
    }
    
    // Use the appropriate endpoint based on provided parameters
    if (departmentId && semesterId) {
      return academicScheduleService.getByDepartmentAndSemester(
        departmentId,
        semesterId,
        params
      );
    } else if (departmentId) {
      return academicScheduleService.getByDepartment(departmentId, params);
    } else {
      // If no department is specified, you might want to use a general search endpoint
      // This would need to be added to the API if not available
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<AcademicSchedule>>(
        `${API_ENDPOINTS.ACADEMIC_SCHEDULES.SEARCH}${queryString ? `?${queryString}` : ''}`
      );
    }
  },
};

export default academicScheduleService;