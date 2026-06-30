// services/rolesService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface Role {
  id: string;
  name: string;
}

export interface CreateRoleDto {
  roleName: string;
}

export interface UpdateRoleDto {
  newRoleName: string;
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
// ROLES SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const rolesService = {
  /**
   * Get all roles
   * GET /api/Roles
   */
  getAll: () => {
    return apiService.get<ApiResponse<Role[]>>(
      API_ENDPOINTS.ROLES.BASE
    );
  },

  /**
   * Get role by ID
   * GET /api/Roles/{roleId}
   */
  getById: (roleId: string) => {
    return apiService.get<ApiResponse<Role>>(
      API_ENDPOINTS.ROLES.BY_ID(roleId)
    );
  },

  /**
   * Get role by name
   * GET /api/Roles/by-name/{roleName}
   */
  getByName: (roleName: string) => {
    return apiService.get<ApiResponse<Role>>(
      API_ENDPOINTS.ROLES.BY_NAME(roleName)
    );
  },

  /**
   * Create a new role
   * POST /api/Roles
   */
  create: (data: CreateRoleDto) => {
    return apiService.post<ApiResponse<Role>>(
      API_ENDPOINTS.ROLES.BASE,
      data
    );
  },

  /**
   * Update a role
   * PUT /api/Roles/{roleId}
   */
  update: (roleId: string, data: UpdateRoleDto) => {
    return apiService.put<ApiResponse<string>>(
      API_ENDPOINTS.ROLES.BY_ID(roleId),
      data
    );
  },

  /**
   * Delete a role
   * DELETE /api/Roles/{roleId}
   */
  delete: (roleId: string) => {
    return apiService.delete<ApiResponse<string>>(
      API_ENDPOINTS.ROLES.BY_ID(roleId)
    );
  },

  /**
   * Check if a role exists
   */
  exists: async (roleId: string): Promise<boolean> => {
    try {
      await rolesService.getById(roleId);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Check if a role exists by name
   */
  existsByName: async (roleName: string): Promise<boolean> => {
    try {
      await rolesService.getByName(roleName);
      return true;
    } catch {
      return false;
    }
  },

  /**
   * Get role ID by name
   */
  getRoleIdByName: async (roleName: string): Promise<string | null> => {
    try {
      const response = await rolesService.getByName(roleName);
      return response.data.id || null;
    } catch {
      return null;
    }
  },

  /**
   * Get role name by ID
   */
  getRoleNameById: async (roleId: string): Promise<string | null> => {
    try {
      const response = await rolesService.getById(roleId);
      return response.data.name || null;
    } catch {
      return null;
    }
  },

  /**
   * Create a role if it doesn't exist
   */
  createIfNotExists: async (roleName: string): Promise<Role | null> => {
    try {
      // Check if role exists
      const existing = await rolesService.getByName(roleName);
      return existing.data || null;
    } catch {
      // Role doesn't exist, create it
      const response = await rolesService.create({ roleName });
      return response.data|| null;
    }
  },

  /**
   * Get all role names
   */
  getAllRoleNames: async (): Promise<string[]> => {
    const response = await rolesService.getAll();
    const roles = response.data || [];
    return roles.map((role: Role) => role.name);
  },

  /**
   * Get all role IDs
   */
  getAllRoleIds: async (): Promise<string[]> => {
    const response = await rolesService.getAll();
    const roles = response.data || [];
    return roles.map((role: Role) => role.id);
  },

  /**
   * Validate if a role name is valid
   */
  isValidRoleName: async (roleName: string): Promise<boolean> => {
    try {
      const response = await rolesService.getAll();
      const roles = response.data|| [];
      return roles.some((role: Role) => role.name === roleName);
    } catch {
      return false;
    }
  },
};

export default rolesService;