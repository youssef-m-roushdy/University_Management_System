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

// ──────────────────────────────────────────────────────────────────────────────
// ADDITIONAL ICONS FOR DEPARTMENT ADMIN PAGE
// ──────────────────────────────────────────────────────────────────────────────

export const PlusIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <line x1="12" y1="4" x2="12" y2="20" />
    <line x1="4" y1="12" x2="20" y2="12" />
  </svg>
);

export const SearchIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <circle cx="10" cy="10" r="7" />
    <line x1="15.5" y1="15.5" x2="20" y2="20" />
  </svg>
);

export const EditIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M17 3a2.8 2.8 0 0 1 4 4L7.5 20.5 2 22l1.5-5.5L17 3Z" />
  </svg>
);

export const TrashIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M3 6h18" />
    <path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2" />
    <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6" />
    <line x1="10" y1="11" x2="10" y2="17" />
    <line x1="14" y1="11" x2="14" y2="17" />
  </svg>
);

export const ChevronLeftIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <polyline points="15 18 9 12 15 6" />
  </svg>
);

export const ChevronRightIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <polyline points="9 6 15 12 9 18" />
  </svg>
);

export const BuildingIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <rect x="4" y="2" width="16" height="20" rx="2" ry="2" />
    <line x1="9" y1="6" x2="15" y2="6" />
    <line x1="9" y1="10" x2="15" y2="10" />
    <line x1="9" y1="14" x2="15" y2="14" />
    <line x1="9" y1="18" x2="13" y2="18" />
  </svg>
);

export const UsersIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
    <circle cx="9" cy="7" r="4" />
    <path d="M23 21v-2a4 4 0 0 0-3-3.87" />
    <path d="M16 3.13a4 4 0 0 1 0 7.75" />
  </svg>
);

export const BookIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M4 19.5v-15A2.5 2.5 0 0 1 6.5 2H20v20H6.5a2.5 2.5 0 0 1 0-5H20" />
  </svg>
);

export const DocumentIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" />
    <polyline points="14 2 14 8 20 8" />
    <line x1="16" y1="13" x2="8" y2="13" />
    <line x1="16" y1="17" x2="8" y2="17" />
    <polyline points="10 9 9 9 8 9" />
  </svg>
);

export const CalendarIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <rect x="3" y="4" width="18" height="18" rx="2" ry="2" />
    <line x1="16" y1="2" x2="16" y2="6" />
    <line x1="8" y1="2" x2="8" y2="6" />
    <line x1="3" y1="10" x2="21" y2="10" />
  </svg>
);

export const ArrowUpIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <line x1="12" y1="19" x2="12" y2="5" />
    <polyline points="5 12 12 5 19 12" />
  </svg>
);

export const ArrowDownIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <line x1="12" y1="5" x2="12" y2="19" />
    <polyline points="19 12 12 19 5 12" />
  </svg>
);

export const MegaphoneIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <polygon points="11 5 6 9 2 9 2 15 6 15 11 19 11 5" />
    <path d="M19.07 4.93a10 10 0 0 1 0 14.14" />
    <path d="M15.54 8.46a5 5 0 0 1 0 7.07" />
  </svg>
);

export const MapPinIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)} strokeWidth="1.5">
    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
    <circle cx="12" cy="10" r="3" />
  </svg>
);

export const InfoCircleIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <circle cx="12" cy="12" r="10" />
    <line x1="12" y1="16" x2="12" y2="12" />
    <line x1="12" y1="8" x2="12.01" y2="8" />
  </svg>
);

export const XIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <line x1="4" y1="4" x2="20" y2="20" />
    <line x1="20" y1="4" x2="4" y2="20" />
  </svg>
);

// ─── Role Section Icons (for sidebar grouping) ───

// Student — graduation cap (already exists as GraduationCapIcon above,
// but here's a stroke-based version that matches the Feather style)
export const StudentIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M12 3L1 9l11 5 9-4.5" />
    <path d="M1 9v6c0 3 4.5 5 11 5s11-2 11-5V9" />
    <path d="M5 13v5c0 1 2.5 3 7 3s7-2 7-3v-5" />
  </svg>
);

// Assistant — clipboard with checklist
export const ClipboardListIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <rect x="8" y="2" width="8" height="4" rx="1" />
    <path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2" />
    <line x1="8" y1="10" x2="9" y2="10" />
    <line x1="12" y1="10" x2="16" y2="10" />
    <line x1="8" y1="14" x2="9" y2="14" />
    <line x1="12" y1="14" x2="16" y2="14" />
    <line x1="8" y1="18" x2="9" y2="18" />
    <line x1="12" y1="18" x2="16" y2="18" />
  </svg>
);

// Admin — CPU / server (management)
export const CpuIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <rect x="4" y="4" width="16" height="16" rx="2" />
    <rect x="9" y="9" width="6" height="6" />
    <line x1="9" y1="1" x2="9" y2="4" />
    <line x1="15" y1="1" x2="15" y2="4" />
    <line x1="9" y1="20" x2="9" y2="23" />
    <line x1="15" y1="20" x2="15" y2="23" />
    <line x1="20" y1="9" x2="23" y2="9" />
    <line x1="20" y1="14" x2="23" y2="14" />
    <line x1="1" y1="9" x2="4" y2="9" />
    <line x1="1" y1="14" x2="4" y2="14" />
  </svg>
);

// Instructor — book-open (teaching)
export const BookOpenIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M12 6.25A8.5 8.5 0 0 0 2 4v14a8.5 8.5 0 0 1 10 2.25" />
    <path d="M12 6.25A8.5 8.5 0 0 1 22 4v14a8.5 8.5 0 0 0-10 2.25" />
    <line x1="12" y1="6.25" x2="12" y2="20.25" />
  </svg>
);

// Add to Icons.tsx:

// ─── Admin Management Icons ──────────────────────────────────────────────────

export const UserIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
    <circle cx="12" cy="7" r="4" />
  </svg>
);

export const PhoneIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72c.127.96.361 1.903.7 2.81a2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45c.907.339 1.85.573 2.81.7A2 2 0 0 1 22 16.92z" />
  </svg>
);

// Add to Icons.tsx

export const CheckCircleIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14" />
    <polyline points="22 4 12 14.01 9 11.01" />
  </svg>
);

export const XCircleIcon: React.FC<IconProps> = props => (
  <svg {...stroke(props)}>
    <circle cx="12" cy="12" r="10" />
    <line x1="15" y1="9" x2="9" y2="15" />
    <line x1="9" y1="9" x2="15" y2="15" />
  </svg>
);
