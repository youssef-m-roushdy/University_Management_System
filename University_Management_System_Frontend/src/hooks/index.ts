import { useState, useEffect, useCallback } from 'react';

/**
 * Hook for API calls with loading / error state
 */
export function useApi<T, A extends unknown[] = unknown[]>(
  apiFn: (...args: A) => Promise<T>,
  immediate = false
) {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const execute = useCallback(
    async (...args: A) => {
      setLoading(true);
      setError(null);
      try {
        const result = await apiFn(...args);
        setData(result);
        return result;
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Something went wrong');
        throw err;
      } finally {
        setLoading(false);
      }
    },
    [apiFn]
  );

  useEffect(() => {
    // `immediate` calls are only meaningful for apiFns that take no required
    // args, so we pass an empty tuple cast to A here rather than widening
    // execute's own signature to `Partial<A>` everywhere it's used.
    if (immediate) execute(...([] as unknown as A));
  }, [immediate, execute]);

  return { data, loading, error, execute, setData };
}

/**
 * Debounce hook
 */
export function useDebounce<T>(value: T, delay = 400): T {
  const [debounced, setDebounced] = useState<T>(value);
  useEffect(() => {
    const t = setTimeout(() => setDebounced(value), delay);
    return () => clearTimeout(t);
  }, [value, delay]);
  return debounced;
}