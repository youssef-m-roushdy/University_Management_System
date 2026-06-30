// src/contexts/ThemeContext.tsx
//
// Provides light/dark theme state app-wide. On every mode change it:
//   1. Persists the choice to localStorage (so it survives reloads)
//   2. Pushes the matching palette onto :root as CSS variables via
//      applyCssVariables() (so plain CSS files can use var(--primary) etc.)
//
// FOUC prevention: The inline <script> in public/index.html applies
// CSS variables + data-theme BEFORE React mounts, so the first frame
// always paints with the correct theme. This context just keeps React
// in sync after mount.
//
// Usage:
//   <ThemeProvider><App /></ThemeProvider>
//   const { mode, colors, toggleTheme } = useTheme();

import React, {
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
  ReactNode,
} from 'react';
import {
  ThemeMode,
  ThemeColors,
  getColors,
  applyCssVariables,
} from '../constants/colors';

const STORAGE_KEY = 'uniman-theme';

interface ThemeContextValue {
  mode: ThemeMode;
  colors: ThemeColors;
  toggleTheme: () => void;
  setTheme: (mode: ThemeMode) => void;
}

const ThemeContext = createContext<ThemeContextValue | undefined>(undefined);

const getInitialTheme = (): ThemeMode => {
  if (typeof window === 'undefined') return 'dark';

  // First: check data-theme attribute set by the blocking inline script
  // in index.html — this is the most reliable source since it was set
  // BEFORE React mounted and already matches what the user sees.
  const attr = document.documentElement.getAttribute('data-theme');
  if (attr === 'light' || attr === 'dark') return attr;

  // Fallback: localStorage
  const stored = window.localStorage.getItem(STORAGE_KEY);
  if (stored === 'light' || stored === 'dark') return stored;

  // Fallback: system preference
  const prefersDark = window.matchMedia?.(
    '(prefers-color-scheme: dark)'
  ).matches;
  return prefersDark ? 'dark' : 'light';
};

interface ThemeProviderProps {
  children: ReactNode;
  /** Force a starting mode (skips localStorage/system-preference detection) */
  defaultMode?: ThemeMode;
}

export const ThemeProvider: React.FC<ThemeProviderProps> = ({
  children,
  defaultMode,
}) => {
  const [mode, setMode] = useState<ThemeMode>(
    () => defaultMode ?? getInitialTheme()
  );

  useEffect(() => {
    applyCssVariables(mode);
    window.localStorage.setItem(STORAGE_KEY, mode);
  }, [mode]);

  const toggleTheme = () =>
    setMode(prev => (prev === 'light' ? 'dark' : 'light'));
  const setTheme = (next: ThemeMode) => setMode(next);

  const value = useMemo<ThemeContextValue>(
    () => ({ mode, colors: getColors(mode), toggleTheme, setTheme }),
    [mode]
  );

  return (
    <ThemeContext.Provider value={value}>{children}</ThemeContext.Provider>
  );
};

export const useTheme = (): ThemeContextValue => {
  const ctx = useContext(ThemeContext);
  if (!ctx) {
    throw new Error('useTheme must be used within a <ThemeProvider>');
  }
  return ctx;
};
