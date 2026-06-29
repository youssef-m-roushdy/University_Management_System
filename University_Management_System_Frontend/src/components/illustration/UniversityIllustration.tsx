// src/components/illustration/UniversityIllustration.tsx
//
// Original, hand-built SVG illustration (no external asset / no license
// needed) of a glowing campus building, echoing the brand panel of the
// UniMan login design. Pure currentColor-free vector so it renders
// consistently across light & dark themes.

import React from 'react';

const UniversityIllustration: React.FC<React.SVGProps<SVGSVGElement>> = (props) => (
  <svg viewBox="0 0 400 300" xmlns="http://www.w3.org/2000/svg" {...props}>
    <defs>
      <radialGradient id="uni-glow" cx="50%" cy="50%" r="50%">
        <stop offset="0%" stopColor="#3B82F6" stopOpacity="0.55" />
        <stop offset="100%" stopColor="#3B82F6" stopOpacity="0" />
      </radialGradient>
      <linearGradient id="uni-building" x1="0" y1="0" x2="0" y2="1">
        <stop offset="0%" stopColor="#3B5BDB" />
        <stop offset="100%" stopColor="#1E2A52" />
      </linearGradient>
      <linearGradient id="uni-roof" x1="0" y1="0" x2="0" y2="1">
        <stop offset="0%" stopColor="#5B7CFA" />
        <stop offset="100%" stopColor="#27336B" />
      </linearGradient>
      <filter id="uni-blur" x="-50%" y="-50%" width="200%" height="200%">
        <feGaussianBlur stdDeviation="14" />
      </filter>
    </defs>

    {/* ambient glow + scattered stars */}
    <ellipse cx="200" cy="190" rx="170" ry="120" fill="url(#uni-glow)" filter="url(#uni-blur)" />
    {[
      [40, 40, 0.6], [80, 70, 0.4], [130, 35, 0.5], [300, 55, 0.5],
      [340, 90, 0.35], [250, 30, 0.4], [365, 130, 0.45], [25, 120, 0.35],
    ].map(([cx, cy, o], i) => (
      <circle key={i} cx={cx} cy={cy} r="1.6" fill="#FFFFFF" opacity={o} />
    ))}

    {/* ground platform */}
    <ellipse cx="200" cy="272" rx="160" ry="18" fill="#15203F" />
    <ellipse cx="200" cy="266" rx="150" ry="14" fill="#1B2A4F" />

    {/* trees */}
    {[[55, 250], [82, 262], [318, 262], [345, 250]].map(([x, y], i) => (
      <g key={i} transform={`translate(${x} ${y})`} opacity="0.85">
        <rect x="-2" y="0" width="4" height="14" fill="#1E2A52" />
        <path d="M-14 0 L0 -28 L14 0 Z" fill="#27336B" />
        <path d="M-11 -8 L0 -34 L11 -8 Z" fill="#324384" />
      </g>
    ))}

    {/* steps */}
    <path d="M120 240 L280 240 L290 252 L110 252 Z" fill="#27336B" />
    <path d="M110 252 L290 252 L300 264 L100 264 Z" fill="#1E2A52" />

    {/* building base */}
    <rect x="125" y="150" width="150" height="92" fill="url(#uni-building)" rx="3" />

    {/* columns */}
    {[135, 158, 181, 204, 227, 250].map((x, i) => (
      <rect key={i} x={x} y="150" width="9" height="90" fill="#4A63B8" opacity="0.9" />
    ))}
    <rect x="123" y="146" width="154" height="8" fill="#5B7CFA" />

    {/* pediment / roof */}
    <path d="M115 146 L200 92 L285 146 Z" fill="url(#uni-roof)" />
    <path d="M122 142 L200 100 L278 142 Z" fill="#1B2A4F" />

    {/* dome + flag */}
    <rect x="196" y="60" width="3" height="40" fill="#5B7CFA" />
    <path d="M199 60 L222 67 L199 74 Z" fill="#60A5FA" />
    <circle cx="200" cy="92" r="10" fill="#3B5BDB" />

    {/* windows (glow) */}
    {[0, 1, 2].map((row) =>
      [0, 1, 2, 3].map((col) => (
        <rect
          key={`${row}-${col}`}
          x={140 + col * 26}
          y={164 + row * 22}
          width="12"
          height="14"
          rx="2"
          fill="#9DC4FF"
          opacity={0.35 + ((row + col) % 3) * 0.12}
        />
      ))
    )}

    {/* entrance door */}
    <path d="M188 242 L188 212 a12 12 0 0 1 24 0 L212 242 Z" fill="#0F1A38" />
    <rect x="196" y="220" width="4" height="22" fill="#FFD166" opacity="0.7" />
  </svg>
);

export default UniversityIllustration;
