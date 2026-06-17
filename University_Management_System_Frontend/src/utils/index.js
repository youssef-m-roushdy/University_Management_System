/**
 * Format date string to locale date
 */
export function formatDate(str) {
  if (!str) return '—';
  return new Date(str).toLocaleDateString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
  });
}

/**
 * Format date with time
 */
export function formatDateTime(str) {
  if (!str) return '—';
  return new Date(str).toLocaleString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
}

/**
 * Truncate text
 */
export function truncate(text, len = 50) {
  if (!text) return '';
  return text.length > len ? text.slice(0, len) + '…' : text;
}

/**
 * Get initials from name
 */
export function getInitials(name) {
  if (!name) return '';
  return name
    .split(' ')
    .map(w => w[0])
    .join('')
    .toUpperCase()
    .slice(0, 2);
}

/**
 * Safely parse JSON or return null
 */
export function safeParse(str) {
  try {
    return JSON.parse(str);
  } catch {
    return null;
  }
}
