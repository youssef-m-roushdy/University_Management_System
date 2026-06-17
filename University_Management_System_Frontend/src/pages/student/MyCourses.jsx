import React, { useEffect, useState } from 'react';
import { FiBook, FiCheck, FiX } from 'react-icons/fi';
import registrationService from '../../services/registrationService';
import { GRADE_LABELS } from '../../constants';
import { toast } from 'react-toastify';

export default function MyCourses() {
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      try {
        const res = await registrationService.getMyCourses();
        setCourses(res?.data || res || []);
      } catch (e) {
        toast.error('Failed to load courses');
      }
      setLoading(false);
    };
    load();
  }, []);

  const statusBadge = s => {
    const map = {
      Approved: 'badge-success',
      Pending: 'badge-warning',
      Rejected: 'badge-danger',
      Suspended: 'badge-neutral',
    };
    return <span className={`badge ${map[s] || 'badge-neutral'}`}>{s}</span>;
  };

  const progressBadge = p => {
    const map = {
      Completed: 'badge-success',
      InProgress: 'badge-info',
      NotStarted: 'badge-neutral',
    };
    const labels = {
      Completed: 'Completed',
      InProgress: 'In Progress',
      NotStarted: 'Not Started',
    };
    return (
      <span className={`badge ${map[p] || 'badge-neutral'}`}>
        {labels[p] || p}
      </span>
    );
  };

  if (loading)
    return (
      <div className="page-container">
        <div className="spinner" />
      </div>
    );

  return (
    <div className="page-container">
      <div className="page-header">
        <h1>
          <FiBook style={{ marginRight: 8 }} />
          My Courses
        </h1>
        <p>View all your registered courses</p>
      </div>

      {courses.length === 0 ? (
        <div className="card empty-state">
          <h3>No courses registered yet</h3>
          <p>Register for courses to see them here</p>
        </div>
      ) : (
        <div
          style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fill, minmax(340px, 1fr))',
            gap: 20,
          }}
        >
          {courses.map(r => (
            <div className="card" key={r.id}>
              <div
                style={{
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'start',
                  marginBottom: 12,
                }}
              >
                <div>
                  <h3 style={{ fontSize: '1rem' }}>{r.course?.name}</h3>
                  <small style={{ color: 'var(--text-light)' }}>
                    {r.course?.code} · {r.course?.credits} Credits
                  </small>
                </div>
                {r.isPassed ? (
                  <FiCheck size={20} color="var(--success)" />
                ) : null}
              </div>
              <div style={{ display: 'flex', gap: 8, flexWrap: 'wrap' }}>
                {statusBadge(r.status)}
                {progressBadge(r.progress)}
                {r.grade && r.grade !== 'F' && (
                  <span className="badge badge-info">
                    {GRADE_LABELS[r.grade] || r.grade}
                  </span>
                )}
                {r.grade === 'F' && (
                  <span className="badge badge-danger">F</span>
                )}
              </div>
              {r.reason && (
                <p
                  style={{
                    marginTop: 8,
                    fontSize: '0.85rem',
                    color: 'var(--text-light)',
                  }}
                >
                  Note: {r.reason}
                </p>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
