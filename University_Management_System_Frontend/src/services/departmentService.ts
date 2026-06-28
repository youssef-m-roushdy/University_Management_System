// services/departmentService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface Department {
  id: number;
  name: string;
  code: string;
  description: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface CreateDepartmentDto {
  name: string;
  code: string;
  description: string;
}

export interface UpdateDepartmentDto {
  name: string;
  code: string;
  description: string;
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
// DEPARTMENT SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const departmentService = {
  /**
   * Get all departments
   */
  getAll: () => {
    return apiService.get<ApiResponse<Department[]>>(
      API_ENDPOINTS.DEPARTMENTS.BASE
    );
  },

  /**
   * Get department by ID
   */
  getById: (id: number) => {
    return apiService.get<ApiResponse<Department>>(
      API_ENDPOINTS.DEPARTMENTS.BY_ID(id)
    );
  },

  /**
   * Create a new department
   */
  create: (data: CreateDepartmentDto) => {
    return apiService.post<ApiResponse<Department>>(
      API_ENDPOINTS.DEPARTMENTS.BASE,
      data
    );
  },

  /**
   * Update a department
   */
  update: (id: number, data: UpdateDepartmentDto) => {
    return apiService.put<ApiResponse<Department>>(
      API_ENDPOINTS.DEPARTMENTS.BY_ID(id),
      data
    );
  },

  /**
   * Delete a department
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.DEPARTMENTS.BY_ID(id)
    );
  },
};

export default departmentService;
