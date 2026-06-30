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

export interface UpdateAcademicScheduleDto {
  title?: string;
  description?: string;
  departmentId?: number;
  semesterId?: number;
  studyYearId?: number;
  scheduleDate?: string;
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

// ──────────────────────────────────────────────────────────────────────────────
// ACADEMIC SCHEDULE SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const academicScheduleService = {
  /**
   * Get all academic schedules (with optional filters)
   * GET /api/AcademicSchedule
   */
  getAll: (params?: AcademicScheduleFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<AcademicSchedule>>(
        `${API_ENDPOINTS.ACADEMIC_SCHEDULES.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<AcademicSchedule>>(
      API_ENDPOINTS.ACADEMIC_SCHEDULES.BASE
    );
  },

  /**
   * Get academic schedules by department
   * GET /api/AcademicSchedule/department/{departmentId}
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
   * GET /api/AcademicSchedule/department/{departmentId}
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
   * GET /api/AcademicSchedule/department/{departmentId}/semester/{semesterId}
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
   * GET /api/AcademicSchedule/department/{departmentId}/semester/{semesterId}
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
   * GET /api/AcademicSchedule/{id}
   */
  getById: (id: number) => {
    return apiService.get<ApiResponse<AcademicSchedule>>(
      API_ENDPOINTS.ACADEMIC_SCHEDULES.BY_ID(id)
    );
  },

  /**
   * Create a new academic schedule (with file upload)
   * POST /api/AcademicSchedule
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
   * Update an academic schedule
   * PUT /api/AcademicSchedule/{id}
   */
  update: (id: number, data: UpdateAcademicScheduleDto) => {
    const formData = new FormData();
    if (data.title) formData.append('Title', data.title);
    if (data.description) formData.append('Description', data.description);
    if (data.departmentId) formData.append('DepartmentId', data.departmentId.toString());
    if (data.semesterId) formData.append('SemesterId', data.semesterId.toString());
    if (data.studyYearId) formData.append('StudyYearId', data.studyYearId.toString());
    if (data.scheduleDate) formData.append('ScheduleDate', data.scheduleDate);
    if (data.file) formData.append('File', data.file);

    return apiService.put<ApiResponse<AcademicSchedule>>(
      API_ENDPOINTS.ACADEMIC_SCHEDULES.BY_ID(id),
      formData
    );
  },

  /**
   * Delete an academic schedule
   * DELETE /api/AcademicSchedule/{id}
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
    return academicScheduleService.getByDepartmentAndSemesterWithFilters(
      0,
      semesterId,
      {},
      pageNumber,
      pageSize
    );
  },

  /**
   * Search academic schedules
   * GET /api/AcademicSchedule/search
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
    
    if (departmentId && semesterId) {
      return academicScheduleService.getByDepartmentAndSemester(
        departmentId,
        semesterId,
        params
      );
    } else if (departmentId) {
      return academicScheduleService.getByDepartment(departmentId, params);
    } else {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<AcademicSchedule>>(
        `${API_ENDPOINTS.ACADEMIC_SCHEDULES.SEARCH}${queryString ? `?${queryString}` : ''}`
      );
    }
  },

  /**
   * Get schedules by date range
   * GET /api/AcademicSchedule?ScheduleDateFrom={from}&ScheduleDateTo={to}
   */
  getByDateRange: (
    fromDate: string,
    toDate: string,
    departmentId?: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: AcademicScheduleFilterParams = {
      ScheduleDateFrom: fromDate,
      ScheduleDateTo: toDate,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    
    if (departmentId) {
      params.DepartmentId = departmentId.toString();
      return academicScheduleService.getByDepartment(departmentId, params);
    }
    
    return academicScheduleService.getAll(params);
  },

  /**
   * Check if an academic schedule exists
   */
  exists: async (id: number): Promise<boolean> => {
    try {
      await academicScheduleService.getById(id);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Get schedules by study year
   */
  getByStudyYear: (
    studyYearId: number,
    departmentId?: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    // Note: This would need a specific endpoint if not available
    // Using the department endpoint as fallback
    if (departmentId) {
      return academicScheduleService.getByDepartmentWithFilters(
        departmentId,
        { PageNumber: pageNumber, PageSize: pageSize },
        pageNumber,
        pageSize
      );
    }
    return academicScheduleService.getAll({
      PageNumber: pageNumber,
      PageSize: pageSize,
    });
  },

  /**
   * Get schedule statistics
   */
  getStatistics: async (departmentId?: number) => {
    let response;
    if (departmentId) {
      response = await academicScheduleService.getByDepartment(departmentId, {
        PageSize: 1000,
      });
    } else {
      response = await academicScheduleService.getAll({
        PageSize: 1000,
      });
    }
    
    const schedules = response.data || [];
    
    // Group by semester
    const semesterStats: Record<string, number> = {};
    schedules.forEach((s) => {
      semesterStats[s.semesterTitle] = (semesterStats[s.semesterTitle] || 0) + 1;
    });
    
    // Group by department
    const deptStats: Record<string, number> = {};
    schedules.forEach((s) => {
      deptStats[s.departmentName] = (deptStats[s.departmentName] || 0) + 1;
    });
    
    const now = new Date();
    const upcoming = schedules.filter((s) => new Date(s.scheduleDate) > now).length;
    const past = schedules.filter((s) => new Date(s.scheduleDate) < now).length;
    
    return {
      totalSchedules: schedules.length,
      upcoming,
      past,
      semesterStats,
      departmentStats: deptStats,
    };
  },
};

export default academicScheduleService;