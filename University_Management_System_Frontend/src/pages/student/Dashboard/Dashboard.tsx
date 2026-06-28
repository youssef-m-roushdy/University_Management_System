// pages/student/Dashboard/StudentDashboard.tsx

import React, { useState, useEffect, useCallback } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../../contexts/AuthContext';
import {
  FiBook,
  FiCalendar,
  FiClock,
  FiTrendingUp,
  FiAward,
  FiCheckCircle,
  FiAlertCircle,
  FiArrowRight,
  FiBarChart2,
  FiUser,
  FiDollarSign,
} from 'react-icons/fi';
import { ROUTES } from '../../../constants';
import './StudentDashboard.css';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface Course {
  id: number;
  code: string;
  name: string;
  credits: number;
  status: 'InProgress' | 'Completed' | 'NotStarted';
  progress: number;
  grade?: string;
  instructor: string;
  schedule: string;
}

interface StudyYear {
  id: number;
  title: string;
  startYear: number;
  endYear: number;
  status: 'Current' | 'Completed' | 'Upcoming';
  semesters: Semester[];
}

interface Semester {
  id: number;
  title: string;
  status: 'Current' | 'Completed' | 'Upcoming';
  gpa?: number;
  courses: Course[];
}

interface Announcement {
  id: number;
  title: string;
  message: string;
  date: string;
  type: 'info' | 'warning' | 'success';
}

interface UpcomingEvent {
  id: number;
  title: string;
  date: string;
  time: string;
  type: 'assignment' | 'exam' | 'lecture' | 'deadline';
}

// ──────────────────────────────────────────────────────────────────────────────
// COMPONENT
// ──────────────────────────────────────────────────────────────────────────────

export default function StudentDashboard(): React.ReactElement {
  const { user } = useAuth();

  // ─── State ──────────────────────────────────────────────────────────────────

  const [loading, setLoading] = useState<boolean>(true);
  const [currentGPA, setCurrentGPA] = useState<number>(3.75);
  const [totalCredits, setTotalCredits] = useState<number>(45);
  const [completedCourses, setCompletedCourses] = useState<number>(12);
  const [inProgressCourses, setInProgressCourses] = useState<number>(4);

  const [recentCourses, setRecentCourses] = useState<Course[]>([
    {
      id: 1,
      code: 'CS301',
      name: 'Data Structures and Algorithms',
      credits: 3,
      status: 'InProgress',
      progress: 75,
      instructor: 'Dr. Ahmed Mohamed',
      schedule: 'Mon/Wed 10:00 AM - 11:30 AM',
    },
    {
      id: 2,
      code: 'CS302',
      name: 'Database Systems',
      credits: 3,
      status: 'InProgress',
      progress: 60,
      instructor: 'Dr. Sara Ali',
      schedule: 'Tue/Thu 2:00 PM - 3:30 PM',
    },
    {
      id: 3,
      code: 'CS305',
      name: 'Operating Systems',
      credits: 3,
      status: 'Completed',
      progress: 100,
      grade: 'A',
      instructor: 'Dr. Khaled Hassan',
      schedule: 'Mon/Wed 1:00 PM - 2:30 PM',
    },
  ]);

  const [studyYear, setStudyYear] = useState<StudyYear>({
    id: 1,
    title: 'Third Year',
    startYear: 2024,
    endYear: 2025,
    status: 'Current',
    semesters: [
      {
        id: 1,
        title: 'First Semester',
        status: 'Current',
        gpa: 3.8,
        courses: [],
      },
      {
        id: 2,
        title: 'Second Semester',
        status: 'Upcoming',
        courses: [],
      },
    ],
  });

  const [announcements, setAnnouncements] = useState<Announcement[]>([
    {
      id: 1,
      title: 'Midterm Exams Schedule',
      message:
        'Midterm exams will start from November 15th. Please check your schedule.',
      date: '2024-10-28',
      type: 'info',
    },
    {
      id: 2,
      title: 'Course Registration Open',
      message:
        'Registration for next semester courses is now open. Deadline: December 15th.',
      date: '2024-10-25',
      type: 'warning',
    },
    {
      id: 3,
      title: 'Project Submission Deadline',
      message: 'Final project submissions are due on November 30th.',
      date: '2024-10-20',
      type: 'success',
    },
  ]);

  const [upcomingEvents, setUpcomingEvents] = useState<UpcomingEvent[]>([
    {
      id: 1,
      title: 'Data Structures Quiz',
      date: '2024-11-05',
      time: '10:00 AM',
      type: 'exam',
    },
    {
      id: 2,
      title: 'Database Project Submission',
      date: '2024-11-10',
      time: '11:59 PM',
      type: 'deadline',
    },
    {
      id: 3,
      title: 'Algorithm Lecture',
      date: '2024-11-08',
      time: '10:00 AM',
      type: 'lecture',
    },
  ]);

  // ─── Effects ──────────────────────────────────────────────────────────────────

  useEffect(() => {
    // Simulate loading data
    const timer = setTimeout(() => {
      setLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, []);

  // ─── Helpers ──────────────────────────────────────────────────────────────────

  const getGreeting = (): string => {
    const hour = new Date().getHours();
    if (hour < 12) return 'Good Morning';
    if (hour < 17) return 'Good Afternoon';
    return 'Good Evening';
  };

  const getStatusColor = (status: string): string => {
    switch (status) {
      case 'Completed':
        return 'status-completed';
      case 'InProgress':
        return 'status-inprogress';
      case 'Upcoming':
        return 'status-upcoming';
      default:
        return '';
    }
  };

  const getProgressColor = (progress: number): string => {
    if (progress >= 80) return 'progress-high';
    if (progress >= 50) return 'progress-medium';
    return 'progress-low';
  };

  const getEventIcon = (type: string): React.ReactElement => {
    switch (type) {
      case 'exam':
        return <FiAlertCircle className="event-icon exam" />;
      case 'deadline':
        return <FiClock className="event-icon deadline" />;
      case 'assignment':
        return <FiBook className="event-icon assignment" />;
      default:
        return <FiCalendar className="event-icon lecture" />;
    }
  };

  const getAnnouncementIcon = (type: string): React.ReactElement => {
    switch (type) {
      case 'info':
        return <FiAlertCircle className="announcement-icon info" />;
      case 'warning':
        return <FiAlertCircle className="announcement-icon warning" />;
      case 'success':
        return <FiCheckCircle className="announcement-icon success" />;
      default:
        return <FiAlertCircle className="announcement-icon info" />;
    }
  };

  // ─── Render Loading ──────────────────────────────────────────────────────────

  if (loading) {
    return (
      <div className="student-dashboard">
        <div className="loading-container">
          <div className="loading-spinner"></div>
          <p>Loading your dashboard...</p>
        </div>
      </div>
    );
  }

  // ─── Render ──────────────────────────────────────────────────────────────────

  return (
    <div className="student-dashboard">
      {/* Header */}
      <div className="dashboard-header">
        <div className="header-left">
          <h1>
            {getGreeting()}, {user?.displayName || user?.name || 'Student'}! 👋
          </h1>
          <p className="header-subtitle">
            Welcome back to your dashboard. Here's what's happening with your
            studies.
          </p>
        </div>
        <div className="header-right">
          <Link to={ROUTES.STUDENT.PROFILE} className="profile-btn">
            <FiUser />
            <span>Profile</span>
          </Link>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-icon gpa">
            <FiTrendingUp />
          </div>
          <div className="stat-info">
            <span className="stat-value">{currentGPA.toFixed(2)}</span>
            <span className="stat-label">Current GPA</span>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon credits">
            <FiBook />
          </div>
          <div className="stat-info">
            <span className="stat-value">{totalCredits}</span>
            <span className="stat-label">Total Credits</span>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon completed">
            <FiCheckCircle />
          </div>
          <div className="stat-info">
            <span className="stat-value">{completedCourses}</span>
            <span className="stat-label">Completed Courses</span>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon inprogress">
            <FiClock />
          </div>
          <div className="stat-info">
            <span className="stat-value">{inProgressCourses}</span>
            <span className="stat-label">In Progress</span>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="dashboard-grid">
        {/* Left Column */}
        <div className="dashboard-left">
          {/* Current Courses */}
          <div className="dashboard-section">
            <div className="section-header">
              <h2>
                <FiBook className="section-icon" />
                Current Courses
              </h2>
              <Link to={ROUTES.STUDENT.MY_COURSES} className="view-all-link">
                View All <FiArrowRight />
              </Link>
            </div>

            <div className="course-list">
              {recentCourses
                .filter(course => course.status === 'InProgress')
                .map(course => (
                  <div key={course.id} className="course-item">
                    <div className="course-info">
                      <div className="course-header">
                        <span className="course-code">{course.code}</span>
                        <span
                          className={`course-status ${getStatusColor(course.status)}`}
                        >
                          {course.status}
                        </span>
                      </div>
                      <h3 className="course-name">{course.name}</h3>
                      <div className="course-meta">
                        <span className="course-credits">
                          {course.credits} Credits
                        </span>
                        <span className="course-instructor">
                          {course.instructor}
                        </span>
                      </div>
                      <div className="course-schedule">
                        <FiClock className="schedule-icon" />
                        {course.schedule}
                      </div>
                    </div>
                    <div className="course-progress">
                      <div className="progress-info">
                        <span className="progress-label">Progress</span>
                        <span className="progress-value">
                          {course.progress}%
                        </span>
                      </div>
                      <div className="progress-bar">
                        <div
                          className={`progress-fill ${getProgressColor(course.progress)}`}
                          style={{ width: `${course.progress}%` }}
                        />
                      </div>
                    </div>
                  </div>
                ))}
            </div>
          </div>

          {/* Completed Courses */}
          <div className="dashboard-section">
            <div className="section-header">
              <h2>
                <FiAward className="section-icon" />
                Recent Completed
              </h2>
            </div>

            <div className="completed-list">
              {recentCourses
                .filter(course => course.status === 'Completed')
                .slice(0, 3)
                .map(course => (
                  <div key={course.id} className="completed-item">
                    <div className="completed-info">
                      <span className="completed-code">{course.code}</span>
                      <span className="completed-name">{course.name}</span>
                    </div>
                    <div className="completed-grade">
                      <span className="grade-badge">{course.grade || 'A'}</span>
                    </div>
                  </div>
                ))}
            </div>
          </div>
        </div>

        {/* Right Column */}
        <div className="dashboard-right">
          {/* Study Year */}
          <div className="dashboard-section">
            <div className="section-header">
              <h2>
                <FiCalendar className="section-icon" />
                {studyYear.title} ({studyYear.startYear} - {studyYear.endYear})
              </h2>
              <span
                className={`study-year-status ${getStatusColor(studyYear.status)}`}
              >
                {studyYear.status}
              </span>
            </div>

            <div className="semester-list">
              {studyYear.semesters.map(semester => (
                <div key={semester.id} className="semester-item">
                  <div className="semester-header">
                    <span className="semester-title">{semester.title}</span>
                    <span
                      className={`semester-status ${getStatusColor(semester.status)}`}
                    >
                      {semester.status}
                    </span>
                  </div>
                  {semester.gpa && (
                    <div className="semester-gpa">
                      <FiTrendingUp className="gpa-icon" />
                      GPA: {semester.gpa.toFixed(2)}
                    </div>
                  )}
                </div>
              ))}
            </div>

            <Link to={ROUTES.STUDENT.MY_STUDY_YEARS} className="view-all-link">
              View All Study Years <FiArrowRight />
            </Link>
          </div>

          {/* Upcoming Events */}
          <div className="dashboard-section">
            <div className="section-header">
              <h2>
                <FiCalendar className="section-icon" />
                Upcoming Events
              </h2>
            </div>

            <div className="events-list">
              {upcomingEvents.slice(0, 4).map(event => (
                <div key={event.id} className="event-item">
                  {getEventIcon(event.type)}
                  <div className="event-info">
                    <span className="event-title">{event.title}</span>
                    <span className="event-date">
                      {new Date(event.date).toLocaleDateString('en-US', {
                        month: 'short',
                        day: 'numeric',
                      })}{' '}
                      at {event.time}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Announcements */}
          <div className="dashboard-section">
            <div className="section-header">
              <h2>
                <FiAlertCircle className="section-icon" />
                Announcements
              </h2>
            </div>

            <div className="announcements-list">
              {announcements.slice(0, 2).map(announcement => (
                <div key={announcement.id} className="announcement-item">
                  <div className="announcement-header">
                    {getAnnouncementIcon(announcement.type)}
                    <span className="announcement-title">
                      {announcement.title}
                    </span>
                    <span className="announcement-date">
                      {new Date(announcement.date).toLocaleDateString('en-US', {
                        month: 'short',
                        day: 'numeric',
                      })}
                    </span>
                  </div>
                  <p className="announcement-message">{announcement.message}</p>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
