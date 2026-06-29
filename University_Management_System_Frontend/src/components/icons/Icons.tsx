// src/components/icons/Icons.tsx
//
// Lightweight, dependency-free icon set (no icon library required).
// Each icon is a plain inline SVG that inherits color via `currentColor`,
// so they automatically follow var(--text-secondary) / var(--primary) etc.
// when styled from CSS.

import React from 'react';

type IconProps = React.SVGProps<SVGSVGElement>;

const stroke = (props: IconProps): IconProps => ({
  viewBox: '0 0 24 24',
  fill: 'none',
  stroke: 'currentColor',
  strokeWidth: 1.8,
  strokeLinecap: 'round',
  strokeLinejoin: 'round',
  ...props,
});

export const MailIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <rect x="3" y="5" width="18" height="14" rx="2" />
    <path d="m3 7 9 6 9-6" />
  </svg>
);

export const LockIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <rect x="4" y="11" width="16" height="9" rx="2" />
    <path d="M8 11V7a4 4 0 0 1 8 0v4" />
  </svg>
);

export const EyeIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M1.5 12S5 5 12 5s10.5 7 10.5 7-3.5 7-10.5 7S1.5 12 1.5 12Z" />
    <circle cx="12" cy="12" r="3" />
  </svg>
);

export const EyeOffIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M3 3l18 18" />
    <path d="M10.6 5.1A10.8 10.8 0 0 1 12 5c7 0 10.5 7 10.5 7a14.6 14.6 0 0 1-3.1 3.8M6.5 6.6C3.7 8.5 1.5 12 1.5 12s3.5 7 10.5 7c1.3 0 2.5-.2 3.6-.6" />
    <path d="M9.9 9.9a3 3 0 0 0 4.2 4.2" />
  </svg>
);

export const ShieldIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M12 3l8 3v6c0 5-3.5 7.5-8 9-4.5-1.5-8-4-8-9V6l8-3Z" />
    <path d="m9 12 2 2 4-4" />
  </svg>
);

export const ChartIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M4 19V5M4 19h16M8 16v-4M12 16V9M16 16v-6" />
  </svg>
);

export const ClockIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <circle cx="12" cy="12" r="9" />
    <path d="M12 7v5l3 3" />
  </svg>
);

export const SunIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <circle cx="12" cy="12" r="4" />
    <path d="M12 2v2.5M12 19.5V22M4.2 4.2l1.8 1.8M18 18l1.8 1.8M2 12h2.5M19.5 12H22M4.2 19.8l1.8-1.8M18 6l1.8-1.8" />
  </svg>
);

export const MoonIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M20 14.5A8.5 8.5 0 1 1 9.5 4a7 7 0 0 0 10.5 10.5Z" />
  </svg>
);

export const GraduationCapIcon: React.FC<IconProps> = props => (
  <svg viewBox="0 0 24 24" fill="currentColor" {...props}>
    <path d="M12 3 1 8l11 5 9-4.1V17h2V8L12 3Z" />
    <path
      d="M5 11.2V16c0 2 3.1 4 7 4s7-2 7-4v-4.8l-7 3.2-7-3.2Z"
      opacity={0.85}
    />
  </svg>
);

export const GoogleIcon: React.FC<IconProps> = props => (
  <svg viewBox="0 0 48 48" {...props}>
    <path
      fill="#FFC107"
      d="M43.6 20.5H42V20H24v8h11.3C33.7 32.6 29.3 35.5 24 35.5c-6.9 0-12.5-5.6-12.5-12.5S17.1 10.5 24 10.5c3.2 0 6 .1 8 2l5.7-5.7C34.5 3.6 29.6 1.5 24 1.5 11.6 1.5 1.5 11.6 1.5 24S11.6 46.5 24 46.5 46.5 36.4 46.5 24c0-1.2-.1-2.3-.3-3.5Z"
    />
    <path
      fill="#FF3D00"
      d="m6.3 14.7 6.6 4.8C14.6 15.9 18.9 13 24 13c3.2 0 6 1.1 8.2 3l5.7-5.7C34.5 7.1 29.6 5 24 5c-7.7 0-14.3 4.4-17.7 10.7Z"
    />
    <path
      fill="#4CAF50"
      d="M24 46.5c5.5 0 10.4-1.8 14-5l-6.5-5.4c-2 1.4-4.6 2.4-7.5 2.4-5.3 0-9.7-3.4-11.4-8.1l-6.6 5C9.7 41.9 16.3 46.5 24 46.5Z"
    />
    <path
      fill="#1976D2"
      d="M43.6 20.5H42V20H24v8h11.3c-.8 2.2-2.2 4.1-4 5.5l6.5 5.4C41.5 36.4 46.5 31 46.5 24c0-1.2-.1-2.3-.3-3.5Z"
    />
  </svg>
);

export const MicrosoftIcon: React.FC<IconProps> = props => (
  <svg viewBox="0 0 23 23" {...props}>
    <rect x="0" y="0" width="10" height="10" fill="#F35325" />
    <rect x="12" y="0" width="10" height="10" fill="#81BC06" />
    <rect x="0" y="12" width="10" height="10" fill="#05A6F0" />
    <rect x="12" y="12" width="10" height="10" fill="#FFBA08" />
  </svg>
);

export const AlertIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <circle cx="12" cy="12" r="9" />
    <path d="M12 7.5v6" />
    <circle cx="12" cy="16.3" r="0.9" fill="currentColor" stroke="none" />
  </svg>
);
