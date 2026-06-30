// services/feeService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface Fee {
  id: number;
  type: 'Academic' | 'Registration';
  level: 'Preparatory_Year' | 'First_Year' | 'Second_Year' | 'Third_Year' | 'Fourth_Year' | 'Graduate';
  description: string;
  amount: number;
  departmentId: number;
  departmentName: string;
  studyYearId: number;
}

export interface CreateFeeDto {
  amount: number;
  type: 'Academic' | 'Registration';
  level: 'Preparatory_Year' | 'First_Year' | 'Second_Year' | 'Third_Year' | 'Fourth_Year' | 'Graduate';
  description: string;
  studyYearId: number;
  departmentId: number;
}

export interface UpdateFeeDto {
  type: 'Academic' | 'Registration';
  level: 'Preparatory_Year' | 'First_Year' | 'Second_Year' | 'Third_Year' | 'Fourth_Year' | 'Graduate';
  description: string;
  amount: number;
}

export interface FeeFilterParams {
  DepartmentName?: string;
  DepartmentCode?: string;
  Level?: 'Preparatory_Year' | 'First_Year' | 'Second_Year' | 'Third_Year' | 'Fourth_Year' | 'Graduate';
  FeeType?: 'Academic' | 'Registration';
  MinAmount?: number;
  MaxAmount?: number;
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
// FEE SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const feeService = {
  /**
   * Create a new fee
   * POST /api/Fee
   */
  create: (data: CreateFeeDto) => {
    return apiService.post<ApiResponse<Fee>>(
      API_ENDPOINTS.FEES.BASE,
      data
    );
  },

  /**
   * Update a fee
   * PUT /api/Fee/{id}
   */
  update: (id: number, data: UpdateFeeDto) => {
    return apiService.put<ApiResponse<Fee>>(
      API_ENDPOINTS.FEES.BY_ID(id),
      data
    );
  },

  /**
   * Delete a fee
   * DELETE /api/Fee/{id}
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.FEES.BY_ID(id)
    );
  },

  /**
   * Get fees by study year
   * GET /api/Fee/study-year/{studyYearId}
   */
  getByStudyYear: (
    studyYearId: number,
    params?: FeeFilterParams
  ) => {
    const url = API_ENDPOINTS.FEES.BY_STUDY_YEAR(studyYearId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Fee>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Fee>>(url);
  },

  /**
   * Get fees by study year with filters and pagination
   */
  getByStudyYearWithFilters: (
    studyYearId: number,
    filters: FeeFilterParams = {},
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
    return feeService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get academic fees
   */
  getAcademicFees: (
    studyYearId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: FeeFilterParams = {
      FeeType: 'Academic',
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return feeService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get registration fees
   */
  getRegistrationFees: (
    studyYearId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: FeeFilterParams = {
      FeeType: 'Registration',
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return feeService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get fees by level
   */
  getByLevel: (
    studyYearId: number,
    level: 'Preparatory_Year' | 'First_Year' | 'Second_Year' | 'Third_Year' | 'Fourth_Year' | 'Graduate',
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: FeeFilterParams = {
      Level: level,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return feeService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get fees by department
   */
  getByDepartment: (
    studyYearId: number,
    departmentName: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: FeeFilterParams = {
      DepartmentName: departmentName,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return feeService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get fees by department code
   */
  getByDepartmentCode: (
    studyYearId: number,
    departmentCode: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: FeeFilterParams = {
      DepartmentCode: departmentCode,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return feeService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get fees by amount range
   */
  getByAmountRange: (
    studyYearId: number,
    minAmount: number,
    maxAmount: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: FeeFilterParams = {
      MinAmount: minAmount,
      MaxAmount: maxAmount,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return feeService.getByStudyYear(studyYearId, params);
  },

  /**
   * Search fees
   */
  search: (
    studyYearId: number,
    searchTerm: string,
    maxResults: number = 20
  ) => {
    // Note: The API doesn't have a search parameter for fees,
    // so we'll use department name or code for searching
    const params: FeeFilterParams = {
      DepartmentName: searchTerm,
      PageSize: maxResults,
    };
    return feeService.getByStudyYear(studyYearId, params);
  },

  /**
   * Get total fees by study year
   */
  getTotalFees: async (studyYearId: number): Promise<number> => {
    const response = await feeService.getByStudyYear(studyYearId, {
      PageSize: 1000, // Get all fees
    });
    const fees = response.data || [];
    return fees.reduce((total: number, fee: Fee) => total + fee.amount, 0);
  },

  /**
   * Get total academic fees by study year
   */
  getTotalAcademicFees: async (studyYearId: number): Promise<number> => {
    const response = await feeService.getAcademicFees(studyYearId, 1, 1000);
    const fees = response.data || [];
    return fees.reduce((total: number, fee: Fee) => total + fee.amount, 0);
  },

  /**
   * Get total registration fees by study year
   */
  getTotalRegistrationFees: async (studyYearId: number): Promise<number> => {
    const response = await feeService.getRegistrationFees(studyYearId, 1, 1000);
    const fees = response.data || [];
    return fees.reduce((total: number, fee: Fee) => total + fee.amount, 0);
  },
};

export default feeService;