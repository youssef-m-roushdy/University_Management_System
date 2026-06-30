// services/studentService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';
import {
  buildQueryString,
  createFilterPageParams,
} from '../utils/paginationUtils';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export type StudentLevel = 'Preparatory_Year' | 'First_Year' | 'Second_Year' | 'Third_Year' | 'Fourth_Year' | 'Graduate';
export type Gender = 'Male' | 'Female';

export interface Student {
  id: string;
  name: string;
  userName: string;
  phoneNumber: string;
  profilePicture: string;
  address: string;
  gender: Gender;
  academicCode: string;
  level: StudentLevel;
  totalCredits: number;
  allowedCredits: number;
  totalGPA: number;
  department: string;
  specialization: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateStudentDto {
  email: string;
  password: string;
  name: string;
  userName: string;
  phoneNumber: string;
  address: string;
  gender: Gender;
  isActive: boolean;
  academicCode: string;
  level: StudentLevel;
  totalCredits: number;
  allowedCredits: number;
  totalGPA: number;
  departmentId: number;
  specializationId: number;
}

export interface UpdateStudentDto {
  academicCode: string;
  level: StudentLevel;
  totalCredits: number;
  allowedCredits: number;
  totalGPA: number;
  departmentId: number;
  specializationId: number;
}

export interface AddStudentToExistingUserDto {
  userEmail: string;
  academicCode: string;
  level: StudentLevel;
  totalCredits: number;
  allowedCredits: number;
  totalGPA: number;
  departmentId: number;
  specializationId: number;
}

export interface StudentFilterParams {
  Level?: StudentLevel;
  Gender?: Gender;
  DepartmentSearch?: string;
  SpecializationSearch?: string;
  MinGPA?: number;
  MaxGPA?: number;
  MinTotalCredits?: number;
  MaxTotalCredits?: number;
  MinAllowedCredits?: number;
  MaxAllowedCredits?: number;
  IsGraduated?: boolean;
  IsActive?: boolean;
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
// STUDENT SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const studentService = {
  /**
   * Get all students (with optional filters)
   * GET /api/Student
   */
  getAll: (params?: StudentFilterParams) => {
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Student>>(
        `${API_ENDPOINTS.STUDENTS.BASE}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Student>>(
      API_ENDPOINTS.STUDENTS.BASE
    );
  },

  /**
   * Get students with filters
   */
  getAllWithFilters: (params: StudentFilterParams) => {
    const queryString = buildQueryString(params);
    return apiService.get<PaginatedResponse<Student>>(
      `${API_ENDPOINTS.STUDENTS.BASE}${queryString ? `?${queryString}` : ''}`
    );
  },

  /**
   * Get students with pagination
   */
  getAllWithPagination: (
    filters: StudentFilterParams = {},
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
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get student by ID
   * GET /api/Student/{id}
   */
  getById: (id: string) => {
    return apiService.get<ApiResponse<Student>>(
      API_ENDPOINTS.STUDENTS.BY_ID(id)
    );
  },

  /**
   * Get student by academic code
   * GET /api/Student/academic-code/{academicCode}
   */
  getByAcademicCode: (academicCode: string) => {
    return apiService.get<ApiResponse<Student>>(
      API_ENDPOINTS.STUDENTS.BY_ACADEMIC_CODE(academicCode)
    );
  },

  /**
   * Create a new student
   * POST /api/Student
   */
  create: (data: CreateStudentDto) => {
    return apiService.post<ApiResponse<Student>>(
      API_ENDPOINTS.STUDENTS.BASE,
      data
    );
  },

  /**
   * Update a student
   * PUT /api/Student/{id}
   */
  update: (id: string, data: UpdateStudentDto) => {
    return apiService.put<ApiResponse<Student>>(
      API_ENDPOINTS.STUDENTS.BY_ID(id),
      data
    );
  },

  /**
   * Delete a student
   * DELETE /api/Student/{id}
   */
  delete: (id: string) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.STUDENTS.BY_ID(id)
    );
  },

  /**
   * Get students by department
   * GET /api/Student/department/{departmentId}
   */
  getByDepartment: (
    departmentId: number,
    params?: StudentFilterParams
  ) => {
    const url = API_ENDPOINTS.STUDENTS.BY_DEPARTMENT(departmentId);
    if (params) {
      const queryString = buildQueryString(params);
      return apiService.get<PaginatedResponse<Student>>(
        `${url}${queryString ? `?${queryString}` : ''}`
      );
    }
    return apiService.get<PaginatedResponse<Student>>(url);
  },

  /**
   * Get students by department with pagination
   */
  getByDepartmentWithPagination: (
    departmentId: number,
    filters: StudentFilterParams = {},
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
    return studentService.getByDepartment(departmentId, params);
  },

  /**
   * Add student role to existing user
   * POST /api/Student/add-to-existing-user
   */
  addToExistingUser: (data: AddStudentToExistingUserDto) => {
    return apiService.post<ApiResponse<Student>>(
      API_ENDPOINTS.STUDENTS.ADD_TO_EXISTING_USER,
      data
    );
  },

  /**
   * Get students by level
   */
  getByLevel: (
    level: StudentLevel,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentFilterParams = {
      Level: level,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get students by gender
   */
  getByGender: (
    gender: Gender,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentFilterParams = {
      Gender: gender,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get students by GPA range
   */
  getByGPARange: (
    minGPA: number,
    maxGPA: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentFilterParams = {
      MinGPA: minGPA,
      MaxGPA: maxGPA,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get students by total credits range
   */
  getByTotalCreditsRange: (
    minCredits: number,
    maxCredits: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentFilterParams = {
      MinTotalCredits: minCredits,
      MaxTotalCredits: maxCredits,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get graduated students
   */
  getGraduated: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentFilterParams = {
      IsGraduated: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get active students
   */
  getActive: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentFilterParams = {
      IsActive: true,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get inactive students
   */
  getInactive: (
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentFilterParams = {
      IsActive: false,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Search students
   */
  search: (
    searchTerm: string,
    maxResults: number = 20
  ) => {
    const params: StudentFilterParams = {
      SearchTerm: searchTerm,
      PageSize: maxResults,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get students by specialization
   */
  getBySpecialization: (
    specializationSearch: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentFilterParams = {
      SpecializationSearch: specializationSearch,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get students by department search
   */
  getByDepartmentSearch: (
    departmentSearch: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) => {
    const params: StudentFilterParams = {
      DepartmentSearch: departmentSearch,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    return studentService.getAllWithFilters(params);
  },

  /**
   * Get student statistics
   */
  getStatistics: async () => {
    const response = await studentService.getAllWithPagination({}, 1, 1000);
    const students = response.data || [];
    
    const totalStudents = students.length;
    const graduated = students.filter((s: Student) => s.level === 'Graduate').length;
    const active = students.filter((s: Student) => s).length;
    
    const levels: Record<StudentLevel, number> = {
      'Preparatory_Year': 0,
      'First_Year': 0,
      'Second_Year': 0,
      'Third_Year': 0,
      'Fourth_Year': 0,
      'Graduate': 0,
    };
    
    students.forEach((s: Student) => {
      if (s.level in levels) {
        levels[s.level]++;
      }
    });
    
    const totalGPA = students.reduce((sum: number, s: Student) => sum + s.totalGPA, 0);
    const totalCredits = students.reduce((sum: number, s: Student) => sum + s.totalCredits, 0);
    
    return {
      totalStudents,
      graduated,
      active,
      inactive: totalStudents - active,
      levels,
      averageGPA: totalStudents > 0 ? totalGPA / totalStudents : 0,
      totalCredits,
      averageCredits: totalStudents > 0 ? totalCredits / totalStudents : 0,
    };
  },

  /**
   * Get department student summary
   */
  getDepartmentSummary: async (departmentId: number) => {
    const response = await studentService.getByDepartment(departmentId, {
      PageSize: 1000,
    });
    const students = response.data || [];
    
    const levels: Record<StudentLevel, number> = {
      'Preparatory_Year': 0,
      'First_Year': 0,
      'Second_Year': 0,
      'Third_Year': 0,
      'Fourth_Year': 0,
      'Graduate': 0,
    };
    
    students.forEach((s: Student) => {
      if (s.level in levels) {
        levels[s.level]++;
      }
    });
    
    const totalGPA = students.reduce((sum: number, s: Student) => sum + s.totalGPA, 0);
    
    return {
      departmentId,
      totalStudents: students.length,
      levels,
      averageGPA: students.length > 0 ? totalGPA / students.length : 0,
    };
  },

  /**
   * Check if student exists
   */
  exists: async (id: string): Promise<boolean> => {
    try {
      await studentService.getById(id);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Check if student exists by academic code
   */
  existsByAcademicCode: async (academicCode: string): Promise<boolean> => {
    try {
      await studentService.getByAcademicCode(academicCode);
      return true;
    } catch {
      return false;
    }
  },
};

export default studentService;