// services/coursePrerequisiteService.ts

import apiService from './apiService';
import { API_ENDPOINTS } from '../constants';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface Course {
    id: number;
    code: string;
    name: string;
    description: string;
    credits: number;
    status: 'Opened' | 'Closed';
    departmentId: number;
    departmentName: string;
    createdAt: string;
    updatedAt: string;
    prerequisitesCount: number;
    dependenciesCount: number;
}

export interface CreatePrerequisiteDto {
    courseId: number;
    prerequisiteCourseId: number;
}

export interface BulkCreatePrerequisiteDto {
    courseId: number;
    prerequisiteCourseIds: number[];
}

export interface CreateDependencyDto {
    courseId: number;
    prerequisiteCourseId: number;
}

export interface BulkCreateDependencyDto {
    prerequisiteCourseId: number;
    dependentCourseIds: number[];
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
// COURSE PREREQUISITE SERVICE
// ──────────────────────────────────────────────────────────────────────────────

const coursePrerequisiteService = {
    /**
     * Get prerequisites for a course
     * GET /api/CoursePrerequisite/{id}/prerequisites
     */
    getPrerequisites: (id: number) => {
        return apiService.get<ApiResponse<Course[]>>(
            API_ENDPOINTS.COURSE_PREREQUISITES.PREREQUISITES(id)
        );
    },

    /**
     * Get dependencies for a course
     * GET /api/CoursePrerequisite/{id}/dependencies
     */
    getDependencies: (id: number) => {
        return apiService.get<ApiResponse<Course[]>>(
            API_ENDPOINTS.COURSE_PREREQUISITES.DEPENDENCIES(id)
        );
    },

    /**
     * Add a prerequisite to a course
     * POST /api/CoursePrerequisite/prerequisites
     */
    addPrerequisite: (data: CreatePrerequisiteDto) => {
        return apiService.post<ApiResponse<Course>>(
            API_ENDPOINTS.COURSE_PREREQUISITES.ADD_PREREQUISITE,
            data
        );
    },

    /**
     * Add multiple prerequisites to a course
     * POST /api/CoursePrerequisite/prerequisites/bulk
     */
    addPrerequisitesBulk: (data: BulkCreatePrerequisiteDto) => {
        return apiService.post<ApiResponse<Course[]>>(
            API_ENDPOINTS.COURSE_PREREQUISITES.ADD_PREREQUISITES_BULK,
            data
        );
    },

    /**
     * Remove a prerequisite from a course
     * DELETE /api/CoursePrerequisite/prerequisites/{courseId}/{prerequisiteCourseId}
     */
    removePrerequisite: (courseId: number, prerequisiteCourseId: number) => {
        return apiService.delete<ApiResponse<string>>(
            API_ENDPOINTS.COURSE_PREREQUISITES.REMOVE_PREREQUISITE(
                courseId,
                prerequisiteCourseId
            )
        );
    },

    /**
     * Add a dependency to a course
     * POST /api/CoursePrerequisite/dependencies
     */
    addDependency: (data: CreateDependencyDto) => {
        return apiService.post<ApiResponse<Course>>(
            API_ENDPOINTS.COURSE_PREREQUISITES.ADD_DEPENDENCY,
            data
        );
    },

    /**
     * Add multiple dependencies to a course
     * POST /api/CoursePrerequisite/dependencies/bulk
     */
    addDependenciesBulk: (data: BulkCreateDependencyDto) => {
        return apiService.post<ApiResponse<Course[]>>(
            API_ENDPOINTS.COURSE_PREREQUISITES.ADD_DEPENDENCIES_BULK,
            data
        );
    },

    /**
     * Remove a dependency from a course
     * DELETE /api/CoursePrerequisite/dependencies/{courseId}/{dependencyCourseId}
     */
    removeDependency: (courseId: number, dependencyCourseId: number) => {
        return apiService.delete<ApiResponse<string>>(
            API_ENDPOINTS.COURSE_PREREQUISITES.REMOVE_DEPENDENCY(
                courseId,
                dependencyCourseId
            )
        );
    },

    /**
     * Check if a course has prerequisites
     */
    hasPrerequisites: async (id: number): Promise<boolean> => {
        const response = await coursePrerequisiteService.getPrerequisites(id);
        return (response.data?.length ?? 0) > 0;
    },

    /**
     * Check if a course has dependencies
     */
    hasDependencies: async (id: number): Promise<boolean> => {
        const response = await coursePrerequisiteService.getDependencies(id);
        return (response.data?.length ?? 0) > 0;
    },

    /**
     * Get prerequisite count for a course
     */
    getPrerequisiteCount: async (id: number): Promise<number> => {
        const response = await coursePrerequisiteService.getPrerequisites(id);
        return response.data?.length ?? 0;
    },

    /**
     * Get dependency count for a course
     */
    getDependencyCount: async (id: number): Promise<number> => {
        const response = await coursePrerequisiteService.getDependencies(id);
        return response.data?.length ?? 0;
    },

    /**
     * Replace all prerequisites for a course
     * (Helper method that removes all existing and adds new ones)
     */
    replacePrerequisites: async (
        courseId: number,
        prerequisiteCourseIds: number[]
    ): Promise<any> => {
        // First, get existing prerequisites
        const existingPrereqs = await coursePrerequisiteService.getPrerequisites(courseId);
        // existingPrereqs.data is the Course[] array (since ApiResponse<Course[]> has data: Course[])
        const existingIds = existingPrereqs.data?.map((c) => c.id) ?? [];

        // Remove all existing prerequisites
        for (const id of existingIds) {
            await coursePrerequisiteService.removePrerequisite(courseId, id);
        }

        // Add new prerequisites in bulk
        if (prerequisiteCourseIds.length > 0) {
            return coursePrerequisiteService.addPrerequisitesBulk({
                courseId,
                prerequisiteCourseIds,
            });
        }

        return { data: [] };
    },

    /**
     * Replace all dependencies for a course
     * (Helper method that removes all existing and adds new ones)
     */
    replaceDependencies: async (
        prerequisiteCourseId: number,
        dependentCourseIds: number[]
    ): Promise<any> => {
        // First, get existing dependencies
        const existingDeps = await coursePrerequisiteService.getDependencies(prerequisiteCourseId);
        // existingDeps.data is the Course[] array (since ApiResponse<Course[]> has data: Course[])
        const existingIds = existingDeps.data?.map((c) => c.id) ?? [];

        // Remove all existing dependencies
        for (const id of existingIds) {
            await coursePrerequisiteService.removeDependency(prerequisiteCourseId, id);
        }

        // Add new dependencies in bulk
        if (dependentCourseIds.length > 0) {
            return coursePrerequisiteService.addDependenciesBulk({
                prerequisiteCourseId,
                dependentCourseIds,
            });
        }

        return { data: [] };
    },
};

export default coursePrerequisiteService;