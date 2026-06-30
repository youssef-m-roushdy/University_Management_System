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
   * GET /api/Department
   */
  getAll: () => {
    return apiService.get<ApiResponse<Department[]>>(
      API_ENDPOINTS.DEPARTMENTS.BASE
    );
  },

  /**
   * Get department by ID
   * GET /api/Department/{id}
   */
  getById: (id: number) => {
    return apiService.get<ApiResponse<Department>>(
      API_ENDPOINTS.DEPARTMENTS.BY_ID(id)
    );
  },

  /**
   * Create a new department
   * POST /api/Department
   */
  create: (data: CreateDepartmentDto) => {
    return apiService.post<ApiResponse<Department>>(
      API_ENDPOINTS.DEPARTMENTS.BASE,
      data
    );
  },

  /**
   * Update a department
   * PUT /api/Department/{id}
   */
  update: (id: number, data: UpdateDepartmentDto) => {
    return apiService.put<ApiResponse<Department>>(
      API_ENDPOINTS.DEPARTMENTS.BY_ID(id),
      data
    );
  },

  /**
   * Delete a department
   * DELETE /api/Department/{id}
   */
  delete: (id: number) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.DEPARTMENTS.BY_ID(id)
    );
  },

  /**
   * Check if a department exists
   */
  exists: async (id: number): Promise<boolean> => {
    try {
      await departmentService.getById(id);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Get department by code
   */
  getByCode: async (code: string): Promise<Department | null> => {
    try {
      const response = await departmentService.getAll();
      const departments = response.data || [];
      return departments.find((dept: Department) => dept.code === code) || null;
    } catch {
      return null;
    }
  },

  /**
   * Get department by name
   */
  getByName: async (name: string): Promise<Department | null> => {
    try {
      const response = await departmentService.getAll();
      const departments = response.data || [];
      return departments.find((dept: Department) => dept.name === name) || null;
    } catch {
      return null;
    }
  },

  /**
   * Search departments by name or code
   */
  search: async (searchTerm: string): Promise<Department[]> => {
    try {
      const response = await departmentService.getAll();
      const departments = response.data || [];
      const term = searchTerm.toLowerCase();
      return departments.filter(
        (dept: Department) =>
          dept.name.toLowerCase().includes(term) ||
          dept.code.toLowerCase().includes(term)
      );
    } catch {
      return [];
    }
  },

  /**
   * Get department statistics
   */
  getStatistics: async () => {
    const response = await departmentService.getAll();
    const departments = response.data || [];
    
    return {
      totalDepartments: departments.length,
      departments: departments.map((dept: Department) => ({
        id: dept.id,
        name: dept.name,
        code: dept.code,
      })),
    };
  },
};

export default departmentService;