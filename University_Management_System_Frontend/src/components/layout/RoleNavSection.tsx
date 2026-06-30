// components/layout/RoleNavSection.tsx
import React, { useState } from 'react';
import { NavLink } from 'react-router-dom';
import { FiChevronDown, FiChevronRight } from 'react-icons/fi';

interface NavItem {
  to: string;
  icon: React.ReactNode;
  label: string;
}

interface RoleNavSectionProps {
  roleName: string;
  roleIcon: React.ReactNode;
  items: NavItem[];
  defaultOpen?: boolean;
  sidebarOpen: boolean; // so we hide labels when collapsed
}

export default function RoleNavSection({
  roleName,
  roleIcon,
  items,
  defaultOpen = true,
  sidebarOpen,
}: RoleNavSectionProps) {
  const [open, setOpen] = useState(defaultOpen);

  // When sidebar is collapsed, we just show flat icons (no section headers)
  if (!sidebarOpen) {
    return (
      <div className="role-nav-section">
        {items.map(item => (
          <NavLink
            key={item.to}
            to={item.to}
            className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}
            title={item.label}
          >
            <span className="nav-icon">{item.icon}</span>
          </NavLink>
        ))}
      </div>
    );
  }

  return (
    <div className="role-nav-section">
      <button
        className="role-nav-header"
        onClick={() => setOpen(prev => !prev)}
        type="button"
      >
        <span className="role-nav-header-icon">{roleIcon}</span>
        <span className="role-nav-header-label">{roleName}</span>
        <span className="role-nav-chevron">
          {open ? <FiChevronDown /> : <FiChevronRight />}
        </span>
      </button>

      {open && (
        <div className="role-nav-items">
          {items.map(item => (
            <NavLink
              key={item.to}
              to={item.to}
              className={({ isActive }) =>
                `nav-item ${isActive ? 'active' : ''}`
              }
            >
              <span className="nav-icon">{item.icon}</span>
              <span className="nav-label">{item.label}</span>
            </NavLink>
          ))}
        </div>
      )}
    </div>
  );
}
