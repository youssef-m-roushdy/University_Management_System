// services/studentStudyYearService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';
import { StudentLevel } from './studentService';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface StudentStudyYear {
  id: number;
  studentId: string;
  studyYearId: number;
  semesterId: string;
  academicCode: string;
  studentName: string;
  email: string;
  departmentName: string;
  departmentCode: string;
  departmentId: number;
  specializationName: string;
  specializationId: number;
  startYear: number;
  endYear: number;
  yearRange: string;
  isCurrentStudyYear: boolean;
  level: StudentLevel;
  totalGPA: number;
  totalCredits: number;
  allowedCredits: number;
  isActive: boolean;
  status: string;
  enrolledAt: string;
  graduatedAt: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateStudentStudyYearDto {
  studentId: string;
  studyYearId: number;
  level: StudentLevel;
  isActive: boolean;
}

export interface UpdateStudentStudyYearDto {
  level: StudentLevel;
  isActive: boolean;
}

export interface StudentStudyYearFilterParams {
  IsActive?: boolean;
  Level?: StudentLevel;
  DepartmentName?: string;
  DepartmentCode?: string;
  MinGPA?: number;
  MaxGPA?: number;
  EnrolledFrom?: string;
  EnrolledTo?: string;
  SearchTerm?: string;
  PageNumber?: number;
  PageSize?: number;
  SortBy?: string;
  SortDirection?: 'Ascending' | 'Descending';
}

export interface StudentTimeline {
  studentId: string;
  currentLevel: StudentLevel;
  department: string;
  totalYearsCompleted: number;
  isGraduated: boolean;
  studyYears: {
    studentStudyYearId: number;
    startYear: number;
    endYear: number;
    isCurrent: boolean;
    level: StudentLevel;
    enrolledAt: string;
  }[];
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
// STUDENT STUDY YEAR SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const studentStudyYearService = {
  /**
   * Get student study years by study year
   * GET /api/StudentStudyYear/study-year/{studyYearId}
   */
  getByStudyYear: (
    studyYearId: number,
    params?: StudentStudyYearFilterParams
  ) => {
    const url = API_ENDPOINTS.STUDENT_STUDY_YEARS.BY_STUDY_YEAR(studyYearId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<StudentStudyYear>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<StudentStudyYear>>(url);
  },

  /**
   * Get student study years by study year with pagination
   */
  getByStudyYearWithPagination: (
    studyYearId: number,
    filters: StudentStudyYearFilterParams = {},
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
    return studentStudyYearService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get my student study years
   * GET /api/StudentStudyYear/me
   */
  getMyStudyYears: () => {
    return apiService.get<ApiResponse<StudentStudyYear[]>>(
      API_ENDPOINTS.STUDENT_STUDY_YEARS.ME
    );
  },

  /**
   * Get my current study year
   * GET /api/StudentStudyYear/me/current
   */
  getMyCurrentStudyYear: () => {
    return apiService.get<ApiResponse<StudentStudyYear>>(
      API_ENDPOINTS.STUDENT_STUDY_YEARS.ME_CURRENT
    );
  },

  /**
   * Create a new student study year
   * POST /api/StudentStudyYear
   */
  create: (data: CreateStudentStudyYearDto) => {
    return apiService.post<ApiResponse<StudentStudyYear>>(
      API_ENDPOINTS.STUDENT_STUDY_YEARS.BASE,
      data
    );
  },

  /**
   * Update a student study year
   * PUT /api/StudentStudyYear/{id}
   */
  update: (id: number, data: UpdateStudentStudyYearDto) => {
    return apiService.put<ApiResponse<StudentStudyYear>>(
      API_ENDPOINTS.STUDENT_STUDY_YEARS.BY_ID(id),
      data
    );
  },

  /**
   * Delete a student study year
   * DELETE /api/StudentStudyYear/{id}
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.STUDENT_STUDY_YEARS.BY_ID(id)
    );
  },

  /**
   * Get student timeline
   * GET /api/StudentStudyYear/student/{studentId}/timeline
   */
  getStudentTimeline: (studentId: string) => {
    return apiService.get<ApiResponse<StudentTimeline>>(
      API_ENDPOINTS.STUDENT_STUDY_YEARS.STUDENT_TIMELINE(studentId)
    );
  },

  /**
   * Get active student study years
   */
  getActive: (
    studyYearId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentStudyYearFilterParams = {
      IsActive: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentStudyYearService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get inactive student study years
   */
  getInactive: (
    studyYearId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentStudyYearFilterParams = {
      IsActive: false,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentStudyYearService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get student study years by level
   */
  getByLevel: (
    studyYearId: number,
    level: StudentLevel,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentStudyYearFilterParams = {
      Level: level,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentStudyYearService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get student study years by department
   */
  getByDepartment: (
    studyYearId: number,
    departmentName: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentStudyYearFilterParams = {
      DepartmentName: departmentName,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentStudyYearService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get student study years by department code
   */
  getByDepartmentCode: (
    studyYearId: number,
    departmentCode: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentStudyYearFilterParams = {
      DepartmentCode: departmentCode,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentStudyYearService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get student study years by GPA range
   */
  getByGPARange: (
    studyYearId: number,
    minGPA: number,
    maxGPA: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentStudyYearFilterParams = {
      MinGPA: minGPA,
      MaxGPA: maxGPA,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentStudyYearService.getByStudyYear(studyYearId, params);
  },

  /**
   * Search student study years
   */
  search: (
    studyYearId: number,
    searchTerm: string,
    maxResults: number = 20
  ) => {
    const params: StudentStudyYearFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };
    return studentStudyYearService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get current study year for a student
   */
  getCurrentStudyYear: async (studentId: string): Promise<StudentStudyYear | null> => {
    const response = await studentStudyYearService.getStudentTimeline(studentId);
    const timeline = response.data;
    if (!timeline) return null;
    
    const currentYear = timeline.studyYears.find((y) => y.isCurrent);
    if (!currentYear) return null;
    
    // Get the full study year record
    const studyYearsResponse = await studentStudyYearService.getByStudyYear(
      currentYear.studentStudyYearId,
      { PageSize: 1 }
    );
    const studyYears = studyYearsResponse.data || [];
    return studyYears[0] || null;
  },

  /**
   * Check if a student is graduated
   */
  isGraduated: async (studentId: string): Promise<boolean> => {
    const response = await studentStudyYearService.getStudentTimeline(studentId);
    const timeline = response.data;
    return timeline?.isGraduated || false;
  },

  /**
   * Get total years completed by a student
   */
  getTotalYearsCompleted: async (studentId: string): Promise<number> => {
    const response = await studentStudyYearService.getStudentTimeline(studentId);
    const timeline = response.data;
    return timeline?.totalYearsCompleted || 0;
  },

  /**
   * Get student's current level
   */
  getCurrentLevel: async (studentId: string): Promise<StudentLevel | null> => {
    const response = await studentStudyYearService.getStudentTimeline(studentId);
    const timeline = response.data;
    return timeline?.currentLevel || null;
  },

  /**
   * Get study year statistics
   */
  getStudyYearStatistics: async (studyYearId: number) => {
    const response = await studentStudyYearService.getByStudyYear(studyYearId, {
      PageSize: 1000,
    });
    const records = response.data || [];
    
    const levels: Record<StudentLevel, number> = {
      'Preparatory_Year': 0,
      'First_Year': 0,
      'Second_Year': 0,
      'Third_Year': 0,
      'Fourth_Year': 0,
      'Graduate': 0,
    };
    
    records.forEach((r) => {
      if (r.level in levels) {
        levels[r.level]++;
      }
    });
    
    const totalGPA = records.reduce((sum, r) => sum + r.totalGPA, 0);
    const totalCredits = records.reduce((sum, r) => sum + r.totalCredits, 0);
    
    return {
      studyYearId,
      totalRecords: records.length,
      active: records.filter((r) => r.isActive).length,
      inactive: records.filter((r) => !r.isActive).length,
      levels,
      averageGPA: records.length > 0 ? totalGPA / records.length : 0,
      totalCredits,
      averageCredits: records.length > 0 ? totalCredits / records.length : 0,
    };
  },

  /**
   * Get department statistics for a study year
   */
  getDepartmentStatistics: async (studyYearId: number) => {
    const response = await studentStudyYearService.getByStudyYear(studyYearId, {
      PageSize: 1000,
    });
    const records = response.data || [];
    
    const departments: Record<string, number> = {};
    records.forEach((r) => {
      departments[r.departmentName] = (departments[r.departmentName] || 0) + 1;
    });
    
    return {
      studyYearId,
      totalDepartments: Object.keys(departments).length,
      departments,
    };
  },
};

export default studentStudyYearService;