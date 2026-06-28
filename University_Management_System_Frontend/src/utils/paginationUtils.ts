// utils/paginationUtils.ts

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export interface PaginationParams {
  PageNumber: number;
  PageSize: number;
  SortBy?: string;
  SortDirection?: 'Ascending' | 'Descending';
  SearchTerm?: string;
  [key: string]: any;
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

// ──────────────────────────────────────────────────────────────────────────────
// HELPERS
// ──────────────────────────────────────────────────────────────────────────────

/**
 * Build a query string from an object of parameters
 * Filters out undefined, null, and empty string values
 */
export const buildQueryString = (params: Record<string, any>): string => {
  const searchParams = new URLSearchParams();

  Object.entries(params).forEach(([key, value]) => {
    // Skip undefined, null, and empty strings (but allow 0 and false)
    if (
      value === undefined ||
      value === null ||
      (typeof value === 'string' && value.trim() === '')
    ) {
      return;
    }

    // Handle arrays
    if (Array.isArray(value)) {
      value.forEach(item => {
        if (item !== undefined && item !== null && item !== '') {
          searchParams.append(key, String(item));
        }
      });
    } else {
      searchParams.append(key, String(value));
    }
  });

  const queryString = searchParams.toString();
  return queryString ? `?${queryString}` : '';
};

/**
 * Create filter page parameters with pagination
 */
export const createFilterPageParams = (
  filters: Record<string, any> = {},
  pageNumber: number = 1,
  pageSize: number = 10,
  sortBy: string | undefined = undefined,
  sortDirection: 'Ascending' | 'Descending' = 'Ascending'
): PaginationParams => {
  // Start with a clean copy of filters
  const params: PaginationParams = {
    PageNumber: pageNumber,
    PageSize: pageSize,
    ...filters,
  };

  // Remove any undefined or null values
  Object.keys(params).forEach(key => {
    if (params[key] === undefined || params[key] === null) {
      delete params[key];
    }
  });

  // Add sorting if provided
  if (sortBy && sortBy.trim()) {
    params.SortBy = sortBy.trim();
    params.SortDirection = sortDirection;
  }

  // Ensure PageNumber and PageSize are valid
  params.PageNumber = Math.max(1, Math.floor(params.PageNumber) || 1);
  params.PageSize = Math.max(
    1,
    Math.min(100, Math.floor(params.PageSize) || 10)
  );

  return params;
};

/**
 * Extract pagination parameters from URL search params
 */
export const getPaginationFromQuery = (
  query: Record<string, string | string[] | undefined>
): PaginationParams => {
  const params: PaginationParams = {
    PageNumber: 1,
    PageSize: 10,
  };

  // Extract page number
  if (query.PageNumber) {
    const page = parseInt(query.PageNumber as string, 10);
    if (!isNaN(page) && page > 0) {
      params.PageNumber = page;
    }
  }

  // Extract page size
  if (query.PageSize) {
    const size = parseInt(query.PageSize as string, 10);
    if (!isNaN(size) && size > 0 && size <= 100) {
      params.PageSize = size;
    }
  }

  // Extract sort by
  if (query.SortBy && typeof query.SortBy === 'string') {
    params.SortBy = query.SortBy;
  }

  // Extract sort direction
  if (
    query.SortDirection &&
    typeof query.SortDirection === 'string' &&
    ['Ascending', 'Descending'].includes(query.SortDirection)
  ) {
    params.SortDirection = query.SortDirection as 'Ascending' | 'Descending';
  }

  // Extract search term
  if (query.SearchTerm && typeof query.SearchTerm === 'string') {
    params.SearchTerm = query.SearchTerm;
  }

  return params;
};

/**
 * Get the current page from a URL query string
 */
export const getPageNumber = (
  query: Record<string, string | string[] | undefined>
): number => {
  return getPaginationFromQuery(query).PageNumber;
};

/**
 * Get the page size from a URL query string
 */
export const getPageSize = (
  query: Record<string, string | string[] | undefined>
): number => {
  return getPaginationFromQuery(query).PageSize;
};

/**
 * Get sort parameters from a URL query string
 */
export const getSortParams = (
  query: Record<string, string | string[] | undefined>
): {
  sortBy: string | undefined;
  sortDirection: 'Ascending' | 'Descending';
} => {
  const params = getPaginationFromQuery(query);
  return {
    sortBy: params.SortBy,
    sortDirection: params.SortDirection || 'Ascending',
  };
};

/**
 * Build pagination object for API responses
 */
export const buildPagination = (
  totalCount: number,
  pageNumber: number,
  pageSize: number
): {
  currentPage: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
} => {
  const totalPages = Math.ceil(totalCount / pageSize);

  return {
    currentPage: pageNumber,
    pageSize: pageSize,
    totalPages: totalPages,
    totalCount: totalCount,
    hasNextPage: pageNumber < totalPages,
    hasPreviousPage: pageNumber > 1,
  };
};

/**
 * Get the skip value for database queries
 */
export const getSkipValue = (pageNumber: number, pageSize: number): number => {
  return Math.max(0, (pageNumber - 1) * pageSize);
};

/**
 * Get the take value for database queries
 */
export const getTakeValue = (pageSize: number): number => {
  return Math.max(1, Math.min(100, pageSize));
};

/**
 * Format pagination for API response
 */
export const formatPaginationResponse = <T>(
  data: T[],
  totalCount: number,
  pageNumber: number,
  pageSize: number
): {
  data: T[];
  totalCount: number;
  pagination: ReturnType<typeof buildPagination>;
} => {
  return {
    data,
    totalCount,
    pagination: buildPagination(totalCount, pageNumber, pageSize),
  };
};

/**
 * Calculate pagination range for display
 */
export const getPaginationRange = (
  currentPage: number,
  totalPages: number,
  maxVisible: number = 5
): number[] => {
  if (totalPages <= maxVisible) {
    return Array.from({ length: totalPages }, (_, i) => i + 1);
  }

  const half = Math.floor(maxVisible / 2);
  let start = Math.max(1, currentPage - half);
  const end = Math.min(totalPages, start + maxVisible - 1);

  if (end - start + 1 < maxVisible) {
    start = Math.max(1, end - maxVisible + 1);
  }

  return Array.from({ length: end - start + 1 }, (_, i) => start + i);
};

/**
 * Check if pagination should be displayed
 */
export const shouldShowPagination = (
  totalCount: number,
  pageSize: number
): boolean => {
  return totalCount > pageSize;
};

/**
 * Get pagination info for display
 */
export const getPaginationInfo = (
  totalCount: number,
  pageNumber: number,
  pageSize: number
): string => {
  const start = (pageNumber - 1) * pageSize + 1;
  const end = Math.min(pageNumber * pageSize, totalCount);

  if (totalCount === 0) {
    return 'No items found';
  }

  return `Showing ${start} to ${end} of ${totalCount} items`;
};

// ──────────────────────────────────────────────────────────────────────────────
// DEFAULT EXPORT
// ──────────────────────────────────────────────────────────────────────────────

export default {
  buildQueryString,
  createFilterPageParams,
  getPaginationFromQuery,
  getPageNumber,
  getPageSize,
  getSortParams,
  buildPagination,
  getSkipValue,
  getTakeValue,
  formatPaginationResponse,
  getPaginationRange,
  shouldShowPagination,
  getPaginationInfo,
};
