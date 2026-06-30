// src/styles/colors.ts
//
// Centralized color tokens for UniMan's light & dark themes.
// Palette inspired by GitHub / Linear / Vercel — tuned for long admin-panel
// sessions with strong readability in both modes.
//
// This file is the single source of truth: ThemeContext reads from it to
// apply CSS custom properties, and components/CSS consume those variables
// (var(--primary), var(--text-primary), etc.) instead of hard-coded hex.

export type ThemeMode = 'light' | 'dark';

export interface ThemeColors {
  background: string;
  backgroundSecondary: string;
  surface: string;
  navbar: string;
  sidebar: string;
  sidebarActive: string;
  primary: string;
  primaryHover: string;
  textPrimary: string;
  textSecondary: string;
  border: string;
  inputBackground: string;
  inputBorder: string;
  inputFocus: string;
  success: string;
  warning: string;
  error: string;
  info: string;
  /** Used as the rgba() value inside the elevation box-shadow for this mode */
  shadow: string;
}

export interface ChartColors {
  blue: string;
  green: string;
  purple: string;
  orange: string;
  red: string;
  cyan: string;
}

// Chart colors are mode-independent by design (per the brief).
export const chartColors: ChartColors = {
  blue: '#3B82F6',
  green: '#22C55E',
  purple: '#8B5CF6',
  orange: '#F59E0B',
  red: '#EF4444',
  cyan: '#06B6D4',
};

export const lightColors: ThemeColors = {
  background: '#F8FAFC',
  backgroundSecondary: '#F1F5F9',
  surface: '#FFFFFF',
  navbar: '#FFFFFF',
  sidebar: '#FFFFFF',
  sidebarActive: '#DBEAFE',
  primary: '#2563EB',
  primaryHover: '#1D4ED8',
  textPrimary: '#0F172A',
  textSecondary: '#64748B',
  border: '#E2E8F0',
  inputBackground: '#FFFFFF',
  inputBorder: '#CBD5E1',
  inputFocus: '#2563EB',
  success: '#22C55E',
  warning: '#F59E0B',
  error: '#EF4444',
  info: '#0EA5E9',
  shadow: 'rgba(15, 23, 42, 0.08)',
};

export const darkColors: ThemeColors = {
  background: '#0F172A',
  backgroundSecondary: '#111827',
  surface: '#1E293B',
  navbar: '#111827',
  sidebar: '#111827',
  sidebarActive: '#192442',
  primary: '#3B82F6',
  primaryHover: '#60A5FA',
  textPrimary: '#F8FAFC',
  textSecondary: '#94A3B8',
  border: '#152235',
  inputBackground: '#0F172A',
  inputBorder: '#334155',
  inputFocus: '#3B82F6',
  success: '#22C55E',
  warning: '#F59E0B',
  error: '#EF4444',
  info: '#0EA5E9',
  shadow: 'rgba(0, 0, 0, 0.35)',
};

export const themeColors: Record<ThemeMode, ThemeColors> = {
  light: lightColors,
  dark: darkColors,
};

export const getColors = (mode: ThemeMode): ThemeColors => themeColors[mode];

/**
 * Converts a camelCase token key into a kebab-case CSS variable name.
 * e.g. "textPrimary" -> "--text-primary", "backgroundSecondary" -> "--background-secondary"
 */
const toCssVarName = (key: string): string =>
  `--${key.replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase()}`;

/**
 * Applies a theme's colors (plus chart colors and the elevation shadow) as
 * CSS custom properties on the given element — defaults to the document root,
 * so every stylesheet in the app can read var(--primary), var(--surface), etc.
 * Also stamps data-theme on the element for any [data-theme="dark"] CSS hooks.
 */
export const applyCssVariables = (
  mode: ThemeMode,
  target?: HTMLElement
): void => {
  if (typeof document === 'undefined') return;
  const el = target ?? document.documentElement;
  const colors = getColors(mode);

  Object.entries(colors).forEach(([key, value]) => {
    if (key === 'shadow') return; // handled below as a composed box-shadow
    el.style.setProperty(toCssVarName(key), value);
  });

  Object.entries(chartColors).forEach(([key, value]) => {
    el.style.setProperty(`--chart-${key}`, value);
  });

  el.style.setProperty(
    '--shadow-elevation',
    mode === 'light'
      ? `0 4px 12px ${lightColors.shadow}`
      : `0 4px 16px ${darkColors.shadow}`
  );

  el.setAttribute('data-theme', mode);
};
