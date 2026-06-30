// services/registrationService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export type RegistrationStatus = 'Pending' | 'Approved' | 'Suspended' | 'Rejected';
export type RegistrationProgress = 'Completed' | 'InProgress' | 'NotStarted';
export type Grade = 'A_Plus' | 'A' | 'A_Minus' | 'B_Plus' | 'B' | 'B_Minus' | 'C_Plus' | 'C' | 'C_Minus' | 'D_Plus' | 'D' | 'D_Minus' | 'F';
export type SemesterTitle = 'First_Semester' | 'Second_Semester' | 'Summer';

export interface Registration {
  id: number;
  studentId: string;
  studentName: string;
  academicCode: string;
  courseId: number;
  courseCode: string;
  courseName: string;
  credits: number;
  semesterId: number;
  semesterTitle: string;
  studyYearId: number;
  studyYearRange: string;
  status: RegistrationStatus;
  progress: RegistrationProgress;
  grade: Grade;
  isPassed: boolean;
  reason: string;
  registeredAt: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateRegistrationDto {
  courseId: number;
  studyYearId: number;
  semesterId: number;
}

export interface UpdateRegistrationDto {
  status: RegistrationStatus;
  progress: RegistrationProgress;
  reason: string;
  grade: Grade;
}

export interface UpdateRegistrationStatusDto {
  registrationId: number;
  status: RegistrationStatus;
  reason: string;
  studentId: string;
}

export interface UpdateRegistrationGradeDto {
  registrationId: number;
  grade: Grade;
}

export interface BulkUpdateGradeDto {
  updates: {
    registrationId: number;
    grade: Grade;
  }[];
}

export interface RegistrationFilterParams {
  StudentName?: string;
  AcademicCode?: string;
  CourseName?: string;
  CourseCode?: string;
  Status?: RegistrationStatus;
  IsPassed?: boolean;
  Progress?: RegistrationProgress;
  Grade?: Grade;
  RegisteredFrom?: string;
  RegisteredTo?: string;
  StudyYearStart?: number;
  StudyYearEnd?: number;
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
// REGISTRATION SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const registrationService = {
  /**
   * Create a new registration
   * POST /api/Registration
   */
  create: (data: CreateRegistrationDto) => {
    return apiService.post<ApiResponse<Registration>>(
      API_ENDPOINTS.REGISTRATIONS.BASE,
      data
    );
  },

  /**
   * Get all registrations
   * GET /api/Registration
   */
  getAll: () => {
    return apiService.get<ApiResponse<Registration[]>>(
      API_ENDPOINTS.REGISTRATIONS.BASE
    );
  },

  /**
   * Update a registration
   * PUT /api/Registration/{id}
   */
  update: (id: number, data: UpdateRegistrationDto) => {
    return apiService.put<ApiResponse<Registration>>(
      API_ENDPOINTS.REGISTRATIONS.BY_ID(id),
      data
    );
  },

  /**
   * Delete a registration
   * DELETE /api/Registration/{id}
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.REGISTRATIONS.BY_ID(id)
    );
  },

  /**
   * Get all student registrations with filters
   * GET /api/Registration/student/all
   */
  getStudentRegistrations: (params?: RegistrationFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Registration>>(
        `${API_ENDPOINTS.REGISTRATIONS.STUDENT_ALL}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Registration>>(
      API_ENDPOINTS.REGISTRATIONS.STUDENT_ALL
    );
  },

  /**
   * Get student registrations with pagination
   */
  getStudentRegistrationsWithPagination: (
    filters: RegistrationFilterParams = {},
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
    return registrationService.getStudentRegistrations(params);
  },

  /**
   * Get registrations by study year
   * GET /api/Registration/{studyYearId}/year
   */
  getByStudyYear: (studyYearId: number) => {
    return apiService.get<ApiResponse<Registration[]>>(
      API_ENDPOINTS.REGISTRATIONS.BY_STUDY_YEAR(studyYearId)
    );
  },

  /**
   * Get registrations by study year and semester
   * GET /api/Registration/student/{studyYearId}/year/{semesterId}/semester
   */
  getByStudyYearAndSemester: (studyYearId: number, semesterId: number) => {
    return apiService.get<ApiResponse<Registration[]>>(
      API_ENDPOINTS.REGISTRATIONS.BY_STUDY_YEAR_SEMESTER(studyYearId, semesterId)
    );
  },

  /**
   * Get registrations by semester and study year with filters
   * GET /api/Registration/semester/{semesterId}/study-year/{studyYearId}
   */
  getBySemesterAndStudyYear: (
    semesterId: number,
    studyYearId: number,
    params?: RegistrationFilterParams
  ) => {
    const url = API_ENDPOINTS.REGISTRATIONS.BY_SEMESTER_STUDY_YEAR(semesterId, studyYearId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Registration>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Registration>>(url);
  },

  /**
   * Get registrations by semester and study year with pagination
   */
  getBySemesterAndStudyYearWithPagination: (
    semesterId: number,
    studyYearId: number,
    filters: RegistrationFilterParams = {},
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
    return registrationService.getBySemesterAndStudyYear(
      semesterId,
      studyYearId,
      params
    );
  },

  /**
   * Get registrations by study year with filters
   * GET /api/Registration/study-year/{studyYearId}/registrations
   */
  getRegistrationsByStudyYear: (
    studyYearId: number,
    params?: RegistrationFilterParams
  ) => {
    const url = API_ENDPOINTS.REGISTRATIONS.REGISTRATIONS_BY_STUDY_YEAR(studyYearId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Registration>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Registration>>(url);
  },

  /**
   * Get registrations by study year with pagination
   */
  getRegistrationsByStudyYearWithPagination: (
    studyYearId: number,
    filters: RegistrationFilterParams = {},
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
    return registrationService.getRegistrationsByStudyYear(studyYearId, params);
  },

  /**
   * Update registration status
   * PATCH /api/Registration/{registrationId}/status
   */
  updateStatus: (registrationId: number, data: UpdateRegistrationStatusDto) => {
    return apiService.patch<ApiResponse<Registration>>(
      API_ENDPOINTS.REGISTRATIONS.UPDATE_STATUS(registrationId),
      data
    );
  },

  /**
   * Update registration grade
   * PATCH /api/Registration/{registrationId}/grade
   */
  updateGrade: (registrationId: number, data: UpdateRegistrationGradeDto) => {
    return apiService.patch<ApiResponse<Registration>>(
      API_ENDPOINTS.REGISTRATIONS.UPDATE_GRADE(registrationId),
      data
    );
  },

  /**
   * Bulk update grades
   * PATCH /api/Registration/grades/bulk
   */
  bulkUpdateGrades: (data: BulkUpdateGradeDto) => {
    return apiService.patch<ApiResponse<Registration[]>>(
      API_ENDPOINTS.REGISTRATIONS.BULK_UPDATE_GRADES,
      data
    );
  },

  /**
   * Get registrations by status
   */
  getByStatus: (
    status: RegistrationStatus,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: RegistrationFilterParams = {
      Status: status,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return registrationService.getStudentRegistrations(params);
  },

  /**
   * Get registrations by progress
   */
  getByProgress: (
    progress: RegistrationProgress,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: RegistrationFilterParams = {
      Progress: progress,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return registrationService.getStudentRegistrations(params);
  },

  /**
   * Get passed registrations
   */
  getPassed: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: RegistrationFilterParams = {
      IsPassed: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return registrationService.getStudentRegistrations(params);
  },

  /**
   * Get failed registrations
   */
  getFailed: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: RegistrationFilterParams = {
      IsPassed: false,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return registrationService.getStudentRegistrations(params);
  },

  /**
   * Get registrations by grade
   */
  getByGrade: (
    grade: Grade,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: RegistrationFilterParams = {
      Grade: grade,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return registrationService.getStudentRegistrations(params);
  },

  /**
   * Search registrations
   */
  search: (
    searchTerm: string,
    maxResults: number = 20
  ) => {
    const params: RegistrationFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };
    return registrationService.getStudentRegistrations(params);
  },
};

export default registrationService;