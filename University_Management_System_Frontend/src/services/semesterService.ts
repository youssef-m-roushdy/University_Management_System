// services/semesterService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export type SemesterTitle = 'First_Semester' | 'Second_Semester' | 'Summer';

export interface Semester {
  id: number;
  title: SemesterTitle;
  startDate: string;
  endDate: string;
}

export interface CreateSemesterDto {
  studyYearId: number;
  title: SemesterTitle;
  startDate: string;
  endDate: string;
}

export interface UpdateSemesterDto {
  studyYearId: number;
  title: SemesterTitle;
  startDate: string;
  endDate: string;
}

export interface SemesterFilterParams {
  IsActive?: boolean;
  StartDateFrom?: string;
  StartDateTo?: string;
  EndDateFrom?: string;
  EndDateTo?: string;
  StudyYearStart?: number;
  StudyYearEnd?: number;
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
// SEMESTER SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const semesterService = {
  /**
   * Get all semesters (with optional filters)
   * GET /api/Semester
   */
  getAll: (params?: SemesterFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Semester>>(
        `${API_ENDPOINTS.SEMESTERS.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Semester>>(
      API_ENDPOINTS.SEMESTERS.BASE
    );
  },

  /**
   * Get semesters with filters
   */
  getAllWithFilters: (params: SemesterFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Semester>>(
      `${API_ENDPOINTS.SEMESTERS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get semesters with pagination
   */
  getAllWithPagination: (
    filters: SemesterFilterParams = {},
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
    return semesterService.getAllWithFilters(params);
  },

  /**
   * Get semester by ID
   * GET /api/Semester/{id}
   */
  getById: (id: number) => {
    return apiService.get<ApiResponse<Semester>>(
      API_ENDPOINTS.SEMESTERS.BY_ID(id)
    );
  },

  /**
   * Create a new semester
   * POST /api/Semester
   */
  create: (data: CreateSemesterDto) => {
    return apiService.post<ApiResponse<Semester>>(
      API_ENDPOINTS.SEMESTERS.BASE,
      data
    );
  },

  /**
   * Update a semester
   * PUT /api/Semester/{id}
   */
  update: (id: number, data: UpdateSemesterDto) => {
    return apiService.put<ApiResponse<Semester>>(
      API_ENDPOINTS.SEMESTERS.BY_ID(id),
      data
    );
  },

  /**
   * Delete a semester
   * DELETE /api/Semester/{id}
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.SEMESTERS.BY_ID(id)
    );
  },

  /**
   * Get semesters by study year
   * GET /api/Semester/study-year/{studyYearId}
   */
  getByStudyYear: (studyYearId: number) => {
    return apiService.get<PaginatedResponse<Semester>>(
      API_ENDPOINTS.SEMESTERS.BY_STUDY_YEAR(studyYearId)
    );
  },

  /**
   * Create semesters for a study year
   * POST /api/Semester/study-year/{studyYearId}
   */
  createForStudyYear: (studyYearId: number, data: CreateSemesterDto) => {
    return apiService.post<PaginatedResponse<Semester>>(
      API_ENDPOINTS.SEMESTERS.CREATE_FOR_STUDY_YEAR(studyYearId),
      data
    );
  },

  /**
   * Get active semesters
   */
  getActive: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SemesterFilterParams = {
      IsActive: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return semesterService.getAllWithFilters(params);
  },

  /**
   * Get semesters by date range
   */
  getByDateRange: (
    startDateFrom: string,
    startDateTo: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SemesterFilterParams = {
      StartDateFrom: startDateFrom,
      StartDateTo: startDateTo,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return semesterService.getAllWithFilters(params);
  },

  /**
   * Get semesters by study year range
   */
  getByStudyYearRange: (
    studyYearStart: number,
    studyYearEnd: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SemesterFilterParams = {
      StudyYearStart: studyYearStart,
      StudyYearEnd: studyYearEnd,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return semesterService.getAllWithFilters(params);
  },

  /**
   * Get semester by title
   */
  getByTitle: async (
    title: SemesterTitle,
    studyYearId?: number
  ): Promise<Semester | null> => {
    let params: SemesterFilterParams = {};
    
    if (studyYearId) {
      // If studyYearId is provided, use the study year endpoint
      const response = await semesterService.getByStudyYear(studyYearId);
      const semesters = response.data || [];
      return semesters.find((s: Semester) => s.title === title) || null;
    } else {
      // Otherwise search all semesters
      // Note: This might need to be adjusted based on API capabilities
      const response = await semesterService.getAllWithPagination({}, 1, 100);
      const semesters = response.data || [];
      return semesters.find((s: Semester) => s.title === title) || null;
    }
  },

  /**
   * Check if a semester exists
   */
  exists: async (id: number): Promise<boolean> => {
    try {
      await semesterService.getById(id);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Get current semester
   */
  getCurrentSemester: async (): Promise<Semester | null> => {
    const now = new Date().toISOString();
    const response = await semesterService.getAllWithPagination({}, 1, 100);
    const semesters = response.data || [];
    
    // Find semester where current date is between start and end date
    return semesters.find((s: Semester) => {
      const start = new Date(s.startDate);
      const end = new Date(s.endDate);
      const current = new Date();
      return current >= start && current <= end;
    }) || null;
  },

  /**
   * Get upcoming semesters
   */
  getUpcoming: async (
    pageNumber: number = 1,
    pageSize: number = 10
  ): Promise<Semester[]> => {
    const now = new Date().toISOString();
    const response = await semesterService.getAllWithPagination({}, pageNumber, pageSize);
    const semesters = response.data || [];
    
    return semesters.filter((s: Semester) => {
      const start = new Date(s.startDate);
      return start > new Date();
    });
  },

  /**
   * Get past semesters
   */
  getPast: async (
    pageNumber: number = 1,
    pageSize: number = 10
  ): Promise<Semester[]> => {
    const now = new Date().toISOString();
    const response = await semesterService.getAllWithPagination({}, pageNumber, pageSize);
    const semesters = response.data || [];
    
    return semesters.filter((s: Semester) => {
      const end = new Date(s.endDate);
      return end < new Date();
    });
  },

  /**
   * Get semesters count by study year
   */
  getCountByStudyYear: async (studyYearId: number): Promise<number> => {
    const response = await semesterService.getByStudyYear(studyYearId);
    return response.data.length || 0;
  },

  /**
   * Create default semesters for a study year
   */
  createDefaultSemesters: async (studyYearId: number): Promise<Semester[]> => {
    const year = new Date().getFullYear();
    const semesters: CreateSemesterDto[] = [
      {
        studyYearId,
        title: 'First_Semester',
        startDate: new Date(year, 8, 1).toISOString(), // September 1
        endDate: new Date(year, 11, 31).toISOString(), // December 31
      },
      {
        studyYearId,
        title: 'Second_Semester',
        startDate: new Date(year + 1, 0, 15).toISOString(), // January 15
        endDate: new Date(year + 1, 4, 31).toISOString(), // May 31
      },
      {
        studyYearId,
        title: 'Summer',
        startDate: new Date(year + 1, 5, 1).toISOString(), // June 1
        endDate: new Date(year + 1, 7, 31).toISOString(), // August 31
      },
    ];

    const createdSemesters: Semester[] = [];
    for (const semesterData of semesters) {
      try {
        const response = await semesterService.create(semesterData);
        if (response.data) {
          createdSemesters.push(response.data);
        }
      } catch (error) {
        console.error(`Failed to create semester ${semesterData.title}:`, error);
      }
    }
    return createdSemesters;
  },
};

export default semesterService;