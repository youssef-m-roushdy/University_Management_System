// services/semesterGPAService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';
import { SemesterTitle } from './semesterService';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface SemesterGPA {
  id: number;
  studentId: string;
  studentName: string;
  academicCode: string;
  semesterId: number;
  semesterTitle: SemesterTitle;
  studyYearId: number;
  studyYearRange: string;
  gpa: number;
  totalCreditHours: number;
  departmentId: number;
  departmentName: string;
  departmentCode: string;
  createdAt: string;
  updatedAt: string;
}

export interface SemesterGPAFilterParams {
  StudyYearId?: number;
  MinGPA?: number;
  MaxGPA?: number;
  MinCreditHours?: number;
  MaxCreditHours?: number;
  DepartmentId?: number;
  CalculatedFrom?: string;
  CalculatedTo?: string;
  SemesterTitle?: SemesterTitle;
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
// SEMESTER GPA SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const semesterGPAService = {
  /**
   * Get GPA for a specific student
   * GET /api/SemesterGPA/student/{id}
   */
  getByStudent: (id: number) => {
    return apiService.get<ApiResponse<SemesterGPA>>(
      API_ENDPOINTS.SEMESTER_GPA.BY_STUDENT(id)
    );
  },

  /**
   * Get GPAs by study year with filters
   * GET /api/SemesterGPA/studyyear/{studyYearId}
   */
  getByStudyYear: (
    studyYearId: number,
    params?: SemesterGPAFilterParams
  ) => {
    const url = API_ENDPOINTS.SEMESTER_GPA.BY_STUDY_YEAR(studyYearId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<SemesterGPA>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<SemesterGPA>>(url);
  },

  /**
   * Get GPAs by study year with pagination
   */
  getByStudyYearWithPagination: (
    studyYearId: number,
    filters: SemesterGPAFilterParams = {},
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
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get GPAs by semester with filters
   * GET /api/SemesterGPA/semester/{semesterId}
   */
  getBySemester: (
    semesterId: number,
    params?: SemesterGPAFilterParams
  ) => {
    const url = API_ENDPOINTS.SEMESTER_GPA.BY_SEMESTER(semesterId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<SemesterGPA>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<SemesterGPA>>(url);
  },

  /**
   * Get GPAs by semester with pagination
   */
  getBySemesterWithPagination: (
    semesterId: number,
    filters: SemesterGPAFilterParams = {},
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
    return semesterGPAService.getBySemester(semesterId, params);
  },

  /**
   * Get GPAs by GPA range
   */
  getByGPARange: (
    studyYearId: number,
    minGPA: number,
    maxGPA: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SemesterGPAFilterParams = {
      MinGPA: minGPA,
      MaxGPA: maxGPA,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get GPAs by credit hours range
   */
  getByCreditHoursRange: (
    studyYearId: number,
    minCreditHours: number,
    maxCreditHours: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SemesterGPAFilterParams = {
      MinCreditHours: minCreditHours,
      MaxCreditHours: maxCreditHours,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get GPAs by department
   */
  getByDepartment: (
    studyYearId: number,
    departmentId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SemesterGPAFilterParams = {
      DepartmentId: departmentId,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get GPAs by semester title
   */
  getBySemesterTitle: (
    studyYearId: number,
    semesterTitle: SemesterTitle,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SemesterGPAFilterParams = {
      SemesterTitle: semesterTitle,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get average GPA for a study year
   */
  getAverageGPA: async (studyYearId: number): Promise<number> => {
    const response = await semesterGPAService.getByStudyYear(studyYearId, {
      PageSize: 1000, // Get all records
    });
    const records = response.data || [];
    if (records.length === 0) return 0;
    
    const totalGPA = records.reduce((sum: number, record: SemesterGPA) => sum + record.gpa, 0);
    return totalGPA / records.length;
  },

  /**
   * Get average GPA for a semester
   */
  getAverageGPAForSemester: async (semesterId: number): Promise<number> => {
    const response = await semesterGPAService.getBySemester(semesterId, {
      PageSize: 1000, // Get all records
    });
    const records = response.data || [];
    if (records.length === 0) return 0;
    
    const totalGPA = records.reduce((sum: number, record: SemesterGPA) => sum + record.gpa, 0);
    return totalGPA / records.length;
  },

  /**
   * Get top performing students by GPA
   */
  getTopPerformers: (
    studyYearId: number,
    limit: number = 10
  ) => {
    const params: SemesterGPAFilterParams = {
      PageSize: limit,
      SortBy: 'gpa',
      SortDirection: 'Descending',
    };
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get bottom performing students by GPA
   */
  getBottomPerformers: (
    studyYearId: number,
    limit: number = 10
  ) => {
    const params: SemesterGPAFilterParams = {
      PageSize: limit,
      SortBy: 'gpa',
      SortDirection: 'Ascending',
    };
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get students with GPA above threshold
   */
  getAboveGPA: (
    studyYearId: number,
    threshold: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SemesterGPAFilterParams = {
      MinGPA: threshold,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get students with GPA below threshold
   */
  getBelowGPA: (
    studyYearId: number,
    threshold: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: SemesterGPAFilterParams = {
      MaxGPA: threshold,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Search semester GPAs
   */
  search: (
    studyYearId: number,
    searchTerm: string,
    maxResults: number = 20
  ) => {
    const params: SemesterGPAFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };
    return semesterGPAService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get GPA statistics for a study year
   */
  getStatistics: async (studyYearId: number) => {
    const response = await semesterGPAService.getByStudyYear(studyYearId, {
      PageSize: 1000,
    });
    const records = response.data || [];
    
    if (records.length === 0) {
      return {
        count: 0,
        averageGPA: 0,
        minGPA: 0,
        maxGPA: 0,
        totalCreditHours: 0,
      };
    }

    const gpas = records.map((r: SemesterGPA) => r.gpa);
    const creditHours = records.map((r: SemesterGPA) => r.totalCreditHours);
    
    return {
      count: records.length,
      averageGPA: gpas.reduce((a: number, b: number) => a + b, 0) / gpas.length,
      minGPA: Math.min(...gpas),
      maxGPA: Math.max(...gpas),
      totalCreditHours: creditHours.reduce((a: number, b: number) => a + b, 0),
    };
  },
};

export default semesterGPAService;