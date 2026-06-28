// utils/formatUtils.ts

// ──────────────────────────────────────────────────────────────────────────────
// DATE FORMATTING
// ──────────────────────────────────────────────────────────────────────────────

/**
 * Format date string to locale date
 * @param str - Date string or Date object
 * @param fallback - Fallback value if date is invalid (default: '—')
 * @returns Formatted date string (e.g., "15 Jan 2024")
 */
export const formatDate = (
  str: string | Date | null | undefined,
  fallback: string = '—'
): string => {
  if (!str) return fallback;

  try {
    const date = typeof str === 'string' ? new Date(str) : str;
    if (isNaN(date.getTime())) return fallback;

    return date.toLocaleDateString('en-GB', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
    });
  } catch {
    return fallback;
  }
};

/**
 * Format date with time
 * @param str - Date string or Date object
 * @param fallback - Fallback value if date is invalid (default: '—')
 * @returns Formatted date and time string (e.g., "15 Jan 2024, 14:30")
 */
export const formatDateTime = (
  str: string | Date | null | undefined,
  fallback: string = '—'
): string => {
  if (!str) return fallback;

  try {
    const date = typeof str === 'string' ? new Date(str) : str;
    if (isNaN(date.getTime())) return fallback;

    return date.toLocaleString('en-GB', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  } catch {
    return fallback;
  }
};

/**
 * Format date to relative time (e.g., "2 hours ago", "3 days ago")
 * @param str - Date string or Date object
 * @returns Relative time string
 */
export const formatRelativeTime = (
  str: string | Date | null | undefined
): string => {
  if (!str) return '—';

  try {
    const date = typeof str === 'string' ? new Date(str) : str;
    if (isNaN(date.getTime())) return '—';

    const now = new Date();
    const diff = now.getTime() - date.getTime();
    const seconds = Math.floor(diff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const weeks = Math.floor(days / 7);
    const months = Math.floor(days / 30);
    const years = Math.floor(days / 365);

    if (seconds < 5) return 'Just now';
    if (seconds < 60) return `${seconds} seconds ago`;
    if (minutes < 60) return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    if (hours < 24) return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    if (days < 7) return `${days} day${days > 1 ? 's' : ''} ago`;
    if (weeks < 4) return `${weeks} week${weeks > 1 ? 's' : ''} ago`;
    if (months < 12) return `${months} month${months > 1 ? 's' : ''} ago`;
    return `${years} year${years > 1 ? 's' : ''} ago`;
  } catch {
    return '—';
  }
};

/**
 * Format date to time only
 * @param str - Date string or Date object
 * @param fallback - Fallback value if date is invalid (default: '—')
 * @returns Formatted time string (e.g., "14:30")
 */
export const formatTime = (
  str: string | Date | null | undefined,
  fallback: string = '—'
): string => {
  if (!str) return fallback;

  try {
    const date = typeof str === 'string' ? new Date(str) : str;
    if (isNaN(date.getTime())) return fallback;

    return date.toLocaleTimeString('en-GB', {
      hour: '2-digit',
      minute: '2-digit',
    });
  } catch {
    return fallback;
  }
};

/**
 * Format date to short date (e.g., "15/01/2024")
 * @param str - Date string or Date object
 * @param fallback - Fallback value if date is invalid (default: '—')
 * @returns Formatted short date string
 */
export const formatShortDate = (
  str: string | Date | null | undefined,
  fallback: string = '—'
): string => {
  if (!str) return fallback;

  try {
    const date = typeof str === 'string' ? new Date(str) : str;
    if (isNaN(date.getTime())) return fallback;

    return date.toLocaleDateString('en-GB', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });
  } catch {
    return fallback;
  }
};

// ──────────────────────────────────────────────────────────────────────────────
// TEXT FORMATTING
// ──────────────────────────────────────────────────────────────────────────────

/**
 * Truncate text with ellipsis
 * @param text - Text to truncate
 * @param len - Maximum length (default: 50)
 * @param ellipsis - Ellipsis character (default: '…')
 * @returns Truncated text
 */
export const truncate = (
  text: string | null | undefined,
  len: number = 50,
  ellipsis: string = '…'
): string => {
  if (!text) return '';
  if (text.length <= len) return text;
  return text.slice(0, len).trim() + ellipsis;
};

/**
 * Get initials from name
 * @param name - Full name
 * @param max - Maximum number of initials (default: 2)
 * @returns Initials (e.g., "JD")
 */
export const getInitials = (
  name: string | null | undefined,
  max: number = 2
): string => {
  if (!name) return '';

  const parts = name.trim().split(/\s+/);
  const initials = parts
    .map(word => word[0])
    .filter(char => char && /[a-zA-Z]/.test(char))
    .join('')
    .toUpperCase();

  return initials.slice(0, max);
};

/**
 * Capitalize first letter of each word
 * @param text - Text to capitalize
 * @returns Capitalized text
 */
export const capitalize = (text: string | null | undefined): string => {
  if (!text) return '';
  return text
    .split(' ')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
    .join(' ');
};

/**
 * Convert to slug (URL-friendly string)
 * @param text - Text to convert
 * @returns Slug string
 */
export const toSlug = (text: string | null | undefined): string => {
  if (!text) return '';
  return text
    .toLowerCase()
    .trim()
    .replace(/[^\w\s-]/g, '')
    .replace(/[\s_-]+/g, '-')
    .replace(/^-+|-+$/g, '');
};

/**
 * Convert snake_case to Title Case
 * @param text - Snake case text
 * @returns Title Case
 */
export const snakeToTitle = (text: string | null | undefined): string => {
  if (!text) return '';
  return text
    .split('_')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
    .join(' ');
};

/**
 * Convert camelCase to Title Case
 * @param text - Camel case text
 * @returns Title Case
 */
export const camelToTitle = (text: string | null | undefined): string => {
  if (!text) return '';
  return text
    .replace(/([A-Z])/g, ' $1')
    .replace(/^./, str => str.toUpperCase())
    .trim();
};

// ──────────────────────────────────────────────────────────────────────────────
// JSON HELPERS
// ──────────────────────────────────────────────────────────────────────────────

/**
 * Safely parse JSON or return null
 * @param str - JSON string to parse
 * @returns Parsed object or null
 */
export const safeParse = <T = any>(
  str: string | null | undefined
): T | null => {
  if (!str) return null;
  try {
    return JSON.parse(str) as T;
  } catch {
    return null;
  }
};

/**
 * Safely stringify JSON
 * @param data - Data to stringify
 * @param fallback - Fallback value if stringify fails (default: '{}')
 * @returns JSON string
 */
export const safeStringify = (data: any, fallback: string = '{}'): string => {
  try {
    return JSON.stringify(data);
  } catch {
    return fallback;
  }
};

// ──────────────────────────────────────────────────────────────────────────────
// NUMBER FORMATTING
// ──────────────────────────────────────────────────────────────────────────────

/**
 * Format number with commas
 * @param num - Number to format
 * @param fallback - Fallback value if number is invalid (default: '0')
 * @returns Formatted number (e.g., "1,234")
 */
export const formatNumber = (
  num: number | string | null | undefined,
  fallback: string = '0'
): string => {
  if (num === undefined || num === null || isNaN(Number(num))) return fallback;
  return Number(num).toLocaleString('en-US');
};

/**
 * Format number as currency
 * @param num - Number to format
 * @param currency - Currency code (default: 'USD')
 * @param fallback - Fallback value if number is invalid (default: '$0.00')
 * @returns Formatted currency (e.g., "$1,234.56")
 */
export const formatCurrency = (
  num: number | string | null | undefined,
  currency: string = 'USD',
  fallback: string = '$0.00'
): string => {
  if (num === undefined || num === null || isNaN(Number(num))) return fallback;
  return Number(num).toLocaleString('en-US', {
    style: 'currency',
    currency: currency,
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });
};

/**
 * Format number as percentage
 * @param num - Number to format
 * @param decimalPlaces - Number of decimal places (default: 0)
 * @param fallback - Fallback value if number is invalid (default: '0%')
 * @returns Formatted percentage (e.g., "75%")
 */
export const formatPercentage = (
  num: number | string | null | undefined,
  decimalPlaces: number = 0,
  fallback: string = '0%'
): string => {
  if (num === undefined || num === null || isNaN(Number(num))) return fallback;
  return Number(num).toLocaleString('en-US', {
    style: 'percent',
    minimumFractionDigits: decimalPlaces,
    maximumFractionDigits: decimalPlaces,
  });
};

// ──────────────────────────────────────────────────────────────────────────────
// FILE SIZE FORMATTING
// ──────────────────────────────────────────────────────────────────────────────

/**
 * Format file size in bytes to human-readable format
 * @param bytes - File size in bytes
 * @param decimalPlaces - Number of decimal places (default: 1)
 * @returns Formatted file size (e.g., "1.5 MB")
 */
export const formatFileSize = (
  bytes: number | null | undefined,
  decimalPlaces: number = 1
): string => {
  if (bytes === undefined || bytes === null || bytes === 0) return '0 Bytes';

  const k = 1024;
  const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));

  return (
    parseFloat((bytes / Math.pow(k, i)).toFixed(decimalPlaces)) + ' ' + sizes[i]
  );
};

// ──────────────────────────────────────────────────────────────────────────────
// PHONE NUMBER FORMATTING
// ──────────────────────────────────────────────────────────────────────────────

/**
 * Format phone number
 * @param phone - Phone number string
 * @param fallback - Fallback value if phone is invalid (default: '—')
 * @returns Formatted phone number
 */
export const formatPhone = (
  phone: string | null | undefined,
  fallback: string = '—'
): string => {
  if (!phone) return fallback;

  // Remove all non-numeric characters
  const cleaned = phone.replace(/\D/g, '');

  // Format based on length
  if (cleaned.length === 10) {
    return `(${cleaned.slice(0, 3)}) ${cleaned.slice(3, 6)}-${cleaned.slice(6)}`;
  } else if (cleaned.length === 11) {
    return `${cleaned.slice(0, 1)} (${cleaned.slice(1, 4)}) ${cleaned.slice(4, 7)}-${cleaned.slice(7)}`;
  } else {
    return phone;
  }
};

// ──────────────────────────────────────────────────────────────────────────────
// DEFAULT EXPORT
// ──────────────────────────────────────────────────────────────────────────────

export default {
  // Date formatting
  formatDate,
  formatDateTime,
  formatRelativeTime,
  formatTime,
  formatShortDate,

  // Text formatting
  truncate,
  getInitials,
  capitalize,
  toSlug,
  snakeToTitle,
  camelToTitle,

  // JSON helpers
  safeParse,
  safeStringify,

  // Number formatting
  formatNumber,
  formatCurrency,
  formatPercentage,

  // File size formatting
  formatFileSize,

  // Phone formatting
  formatPhone,
};
