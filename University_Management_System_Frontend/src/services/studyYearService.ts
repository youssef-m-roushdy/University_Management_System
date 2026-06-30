// services/studyYearService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface StudyYear {
  id: number;
  startYear: number;
  endYear: number;
  isCurrent: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateStudyYearDto {
  startYear: number;
  endYear: number;
  isCurrent: boolean;
}

export interface UpdateStudyYearDto {
  startYear: number;
  endYear: number;
  isCurrent: boolean;
}

export interface PatchStudyYearDto {
  isCurrent: boolean;
}

export interface StudyYearFilterParams {
  StartYear?: number;
  EndYear?: number;
  IsCurrent?: boolean;
  HasSemesters?: boolean;
  HasRegistrations?: boolean;
  MinYear?: number;
  MaxYear?: number;
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
// STUDY YEAR SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const studyYearService = {
  /**
   * Get all study years (with optional filters)
   * GET /api/StudyYear
   */
  getAll: (params?: StudyYearFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<StudyYear>>(
        `${API_ENDPOINTS.STUDY_YEARS.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<StudyYear>>(
      API_ENDPOINTS.STUDY_YEARS.BASE
    );
  },

  /**
   * Get study years with filters
   */
  getAllWithFilters: (params: StudyYearFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<StudyYear>>(
      `${API_ENDPOINTS.STUDY_YEARS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get study years with pagination
   */
  getAllWithPagination: (
    filters: StudyYearFilterParams = {},
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
    return studyYearService.getAllWithFilters(params);
  },

  /**
   * Get current study year
   * GET /api/StudyYear/current
   */
  getCurrent: () => {
    return apiService.get<ApiResponse<StudyYear>>(
      API_ENDPOINTS.STUDY_YEARS.CURRENT
    );
  },

  /**
   * Get study year by ID
   * GET /api/StudyYear/{id}
   */
  getById: (id: number) => {
    return apiService.get<ApiResponse<StudyYear>>(
      API_ENDPOINTS.STUDY_YEARS.BY_ID(id)
    );
  },

  /**
   * Create a new study year
   * POST /api/StudyYear
   */
  create: (data: CreateStudyYearDto) => {
    return apiService.post<ApiResponse<StudyYear>>(
      API_ENDPOINTS.STUDY_YEARS.BASE,
      data
    );
  },

  /**
   * Update a study year
   * PUT /api/StudyYear/{id}
   */
  update: (id: number, data: UpdateStudyYearDto) => {
    return apiService.put<ApiResponse<StudyYear>>(
      API_ENDPOINTS.STUDY_YEARS.BY_ID(id),
      data
    );
  },

  /**
   * Patch a study year (partial update)
   * PATCH /api/StudyYear/{id}
   */
  patch: (id: number, data: PatchStudyYearDto) => {
    return apiService.patch<ApiResponse<StudyYear>>(
      API_ENDPOINTS.STUDY_YEARS.BY_ID(id),
      data
    );
  },

  /**
   * Delete a study year
   * DELETE /api/StudyYear/{id}
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.STUDY_YEARS.BY_ID(id)
    );
  },

  /**
   * Set a study year as current
   */
  setAsCurrent: async (id: number): Promise<StudyYear> => {
    // First, unset all current study years
    const allYears = await studyYearService.getAllWithPagination({}, 1, 100);
    const currentYears = allYears.data?.filter((y: StudyYear) => y.isCurrent) || [];

    for (const year of currentYears) {
      await studyYearService.patch(year.id, { isCurrent: false });
    }

    // Then set the specified study year as current
    const response = await studyYearService.patch(id, { isCurrent: true });
    return response.data as StudyYear;
  },

  /**
   * Get study years by year range
   */
  getByYearRange: (
    startYear: number,
    endYear: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudyYearFilterParams = {
      StartYear: startYear,
      EndYear: endYear,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studyYearService.getAllWithFilters(params);
  },

  /**
   * Get study years with semesters
   */
  getWithSemesters: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudyYearFilterParams = {
      HasSemesters: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studyYearService.getAllWithFilters(params);
  },

  /**
   * Get study years with registrations
   */
  getWithRegistrations: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudyYearFilterParams = {
      HasRegistrations: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studyYearService.getAllWithFilters(params);
  },

  /**
   * Get study years by minimum year
   */
  getByMinYear: (
    minYear: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudyYearFilterParams = {
      MinYear: minYear,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studyYearService.getAllWithFilters(params);
  },

  /**
   * Get study years by maximum year
   */
  getByMaxYear: (
    maxYear: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudyYearFilterParams = {
      MaxYear: maxYear,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studyYearService.getAllWithFilters(params);
  },

  /**
   * Get study year by year range exact match
   */
  getByYearRangeExact: async (
    startYear: number,
    endYear: number
  ): Promise<StudyYear | null> => {
    const response = await studyYearService.getAllWithFilters({
      StartYear: startYear,
      EndYear: endYear,
      PageSize: 1,
    });
    const years = response.data || [];
    return years.find((y: StudyYear) => y.startYear === startYear && y.endYear === endYear) || null;
  },

  /**
   * Check if a study year exists
   */
  exists: async (id: number): Promise<boolean> => {
    try {
      await studyYearService.getById(id);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Get study year range from year
   */
  getYearRange: (startYear: number): string => {
    return `${startYear}-${startYear + 1}`;
  },

  /**
   * Get current academic year
   */
  getCurrentAcademicYear: (): string => {
    const now = new Date();
    const currentYear = now.getFullYear();
    // If month is September (8) or later, it's the start of a new academic year
    const startYear = now.getMonth() >= 8 ? currentYear : currentYear - 1;
    return studyYearService.getYearRange(startYear);
  },

  /**
   * Create academic year from current date
   */
  createCurrentAcademicYear: async (): Promise<StudyYear> => {
    const now = new Date();
    const currentYear = now.getFullYear();
    const startYear = now.getMonth() >= 8 ? currentYear : currentYear - 1;
    const endYear = startYear + 1;

    // Check if it already exists
    const existing = await studyYearService.getByYearRangeExact(startYear, endYear);
    if (existing) {
      // If exists, set as current if not already
      if (!existing.isCurrent) {
        await studyYearService.setAsCurrent(existing.id);
      }
      return existing;
    }

    // Create new study year
    const response = await studyYearService.create({
      startYear,
      endYear,
      isCurrent: true,
    });
    return response.data as StudyYear;
  },

  /**
   * Get study year statistics
   */
  getStatistics: async () => {
    const [allYearsResponse, withSemestersResponse, withRegistrationsResponse] =
      await Promise.all([
        studyYearService.getAllWithPagination({}, 1, 100),
        studyYearService.getWithSemesters(1, 1),
        studyYearService.getWithRegistrations(1, 1),
      ]);

    const years = allYearsResponse.data || [];

    const totalYears = allYearsResponse.pagination?.totalCount ?? years.length;
    const currentYears = years.filter((y: StudyYear) => y.isCurrent).length;
    const withSemesters = withSemestersResponse.pagination?.totalCount ?? 0;
    const withRegistrations = withRegistrationsResponse.pagination?.totalCount ?? 0;

    const yearRanges = years.map((y: StudyYear) => `${y.startYear}-${y.endYear}`);
    const oldestYear = years.length > 0 ? Math.min(...years.map((y: StudyYear) => y.startYear)) : null;
    const newestYear = years.length > 0 ? Math.max(...years.map((y: StudyYear) => y.endYear)) : null;

    return {
      totalYears,
      currentYears,
      withSemesters,
      withRegistrations,
      yearRanges,
      oldestYear,
      newestYear,
    };
  },
};

export default studyYearService;