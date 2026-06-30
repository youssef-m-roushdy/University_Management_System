// pages/dashboard/CompositeDashboard.tsx

import React from 'react';
import { useAuth } from '../../contexts/AuthContext';
import { USER_ROLES } from '../../constants';

/**
 * Composite dashboard for multi-role users.
 * Shows each role's widgets in a labeled section so the user
 * sees ALL their relevant information on one page.
 *
 * IMPORTANT: Each role's existing Dashboard page should export
 * a "Widgets" sub-component for embedding here. See the
 * refactored AdminDashboard example below.
 */
export default function CompositeDashboard() {
  const { roles } = useAuth();

  return (
    <div className="composite-dashboard">
      {/* ─── Student Section ─── */}
      {roles?.includes(USER_ROLES.STUDENT) && (
        <section className="dashboard-section student-section">
          <h2 className="section-heading">🎓 Student Overview</h2>
          <p style={{ color: 'var(--text-secondary)', fontSize: '0.85rem' }}>
            Your enrolled courses, timeline, and academic progress.
          </p>
          {/* Replace with <StudentDashboardWidgets /> once you refactor */}
          <div className="composite-placeholder">
            <p>Student dashboard widgets will render here.</p>
            <p>
              Refactor your <code>StudentDashboard.tsx</code> to export a{' '}
              <code>StudentDashboardWidgets</code> component, then import it
              above.
            </p>
          </div>
        </section>
      )}

      {/* ─── Assistant Section ─── */}
      {roles?.includes(USER_ROLES.ASSISTANT) && (
        <section className="dashboard-section assistant-section">
          <h2 className="section-heading">📋 Assistant Overview</h2>
          <p style={{ color: 'var(--text-secondary)', fontSize: '0.85rem' }}>
            Courses you assist, student lists, and grading tasks.
          </p>
          <div className="composite-placeholder">
            <p>Assistant dashboard widgets will render here.</p>
          </div>
        </section>
      )}

      {/* ─── Instructor Section ─── */}
      {roles?.includes(USER_ROLES.INSTRUCTOR) && (
        <section className="dashboard-section instructor-section">
          <h2 className="section-heading">📚 Instructor Overview</h2>
          <p style={{ color: 'var(--text-secondary)', fontSize: '0.85rem' }}>
            Your courses and student rosters.
          </p>
          <div className="composite-placeholder">
            <p>Instructor dashboard widgets will render here.</p>
          </div>
        </section>
      )}

      {/* ─── Admin Section ─── */}
      {roles?.includes(USER_ROLES.ADMIN) && (
        <section className="dashboard-section admin-section">
          <h2 className="section-heading">⚙️ Admin Overview</h2>
          <p style={{ color: 'var(--text-secondary)', fontSize: '0.85rem' }}>
            System stats, departments, and management tools.
          </p>
          <div className="composite-placeholder">
            <p>Admin dashboard widgets will render here.</p>
          </div>
        </section>
      )}
    </div>
  );
}
