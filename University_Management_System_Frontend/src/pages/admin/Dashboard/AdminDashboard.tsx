import React, { useMemo } from 'react';
import { useAuth } from '../../../contexts/AuthContext';
import { GraduationCapIcon } from '../../../components/icons/Icons';
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
} from 'recharts';
import './AdminDashboard.css';

// Using constants from design specs

// Missing SVG icons from Icons.tsx that are generic enough can be created inline or imported.
// A megaphone and book and user icons are not strictly defined, so I am going to make some stub components below if they are missing.
const UsersIconSVG = (props: React.SVGProps<SVGSVGElement>) => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="1.8"
    strokeLinecap="round"
    strokeLinejoin="round"
    {...props}
  >
    <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path>
    <circle cx="9" cy="7" r="4"></circle>
    <path d="M23 21v-2a4 4 0 0 0-3-3.87"></path>
    <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
  </svg>
);

const BookIconSVG = (props: React.SVGProps<SVGSVGElement>) => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="1.8"
    strokeLinecap="round"
    strokeLinejoin="round"
    {...props}
  >
    <path d="M4 19.5v-15A2.5 2.5 0 0 1 6.5 2H20v20H6.5a2.5 2.5 0 0 1 0-5H20"></path>
  </svg>
);

const DocumentIconSVG = (props: React.SVGProps<SVGSVGElement>) => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="1.8"
    strokeLinecap="round"
    strokeLinejoin="round"
    {...props}
  >
    <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path>
    <polyline points="14 2 14 8 20 8"></polyline>
    <line x1="16" y1="13" x2="8" y2="13"></line>
    <line x1="16" y1="17" x2="8" y2="17"></line>
    <polyline points="10 9 9 9 8 9"></polyline>
  </svg>
);

const CalendarIconSVG = (props: React.SVGProps<SVGSVGElement>) => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="1.8"
    strokeLinecap="round"
    strokeLinejoin="round"
    {...props}
  >
    <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect>
    <line x1="16" y1="2" x2="16" y2="6"></line>
    <line x1="8" y1="2" x2="8" y2="6"></line>
    <line x1="3" y1="10" x2="21" y2="10"></line>
  </svg>
);

const ArrowUpIcon = (props: React.SVGProps<SVGSVGElement>) => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    {...props}
  >
    <line x1="12" y1="19" x2="12" y2="5"></line>
    <polyline points="5 12 12 5 19 12"></polyline>
  </svg>
);

const ArrowDownIcon = (props: React.SVGProps<SVGSVGElement>) => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    {...props}
  >
    <line x1="12" y1="5" x2="12" y2="19"></line>
    <polyline points="19 12 12 19 5 12"></polyline>
  </svg>
);

const MegaphoneIconSVG = (props: React.SVGProps<SVGSVGElement>) => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="1.8"
    strokeLinecap="round"
    strokeLinejoin="round"
    {...props}
  >
    <polygon points="11 5 6 9 2 9 2 15 6 15 11 19 11 5"></polygon>
    <path d="M19.07 4.93a10 10 0 0 1 0 14.14M15.54 8.46a5 5 0 0 1 0 7.07"></path>
  </svg>
);

const MapPinSVG = (props: React.SVGProps<SVGSVGElement>) => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="1.5"
    strokeLinecap="round"
    strokeLinejoin="round"
    {...props}
  >
    <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"></path>
    <circle cx="12" cy="10" r="3"></circle>
  </svg>
);

const InfoCircleSVG = (props: React.SVGProps<SVGSVGElement>) => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="1.8"
    strokeLinecap="round"
    strokeLinejoin="round"
    {...props}
  >
    <circle cx="12" cy="12" r="10"></circle>
    <line x1="12" y1="16" x2="12" y2="12"></line>
    <line x1="12" y1="8" x2="12.01" y2="8"></line>
  </svg>
);

const MOCK_ENROLLMENT_DATA = [
  { name: 'Jan', students: 1000 },
  { name: 'Feb', students: 1300 },
  { name: 'Mar', students: 1400 },
  { name: 'Apr', students: 1600 },
  { name: 'May', students: 2000 },
  { name: 'Jun', students: 2543 },
];

const MOCK_DEPARTMENT_DATA = [
  {
    name: 'Computer Science',
    value: 925,
    color: '#3B82F6',
    percentage: '36.4%',
  },
  {
    name: 'Business Administration',
    value: 612,
    color: '#22C55E',
    percentage: '24.1%',
  },
  { name: 'Engineering', value: 502, color: '#8B5CF6', percentage: '19.7%' },
  { name: 'Medicine', value: 342, color: '#F59E0B', percentage: '13.5%' },
  { name: 'Other', value: 162, color: '#64748B', percentage: '6.3%' },
];

export default function AdminDashboard() {
  const { user } = useAuth();
  const userName = user?.displayName || user?.name || 'Ahmed';

  const todayStr = useMemo(() => {
    return new Date().toLocaleDateString('en-GB', {
      weekday: 'long',
      day: 'numeric',
      month: 'long',
      year: 'numeric',
    });
  }, []);

  return (
    <div className="admin-dashboard">
      <div className="dashboard-header">
        <div className="dashboard-header-text">
          <h1>Welcome back, {userName}! 👋</h1>
          <p>Here's what's happening at your university today.</p>
        </div>
        <div className="dashboard-header-date">
          <CalendarIconSVG width="16" height="16" />
          <span>{todayStr}</span>
        </div>
      </div>

      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#1E3A8A' }}>
            <UsersIconSVG color="#3B82F6" />
          </div>
          <div className="stat-info">
            <h3>Total Students</h3>
            <div className="stat-value">2,543</div>
            <div className="stat-trend positive">
              <ArrowUpIcon width="14" height="14" />
              12.5% <span className="stat-trend-text">from last month</span>
            </div>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#14532D' }}>
            <GraduationCapIcon color="#22C55E" />
          </div>
          <div className="stat-info">
            <h3>Total Faculty</h3>
            <div className="stat-value">186</div>
            <div className="stat-trend positive">
              <ArrowUpIcon width="14" height="14" />
              8.1% <span className="stat-trend-text">from last month</span>
            </div>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#4C1D95' }}>
            <BookIconSVG color="#8B5CF6" />
          </div>
          <div className="stat-info">
            <h3>Total Courses</h3>
            <div className="stat-value">96</div>
            <div className="stat-trend positive">
              <ArrowUpIcon width="14" height="14" />
              5.4% <span className="stat-trend-text">from last month</span>
            </div>
          </div>
        </div>

        <div className="stat-card">
          <div className="stat-icon" style={{ background: '#78350F' }}>
            <DocumentIconSVG color="#F59E0B" />
          </div>
          <div className="stat-info">
            <h3>Pending Tasks</h3>
            <div className="stat-value">24</div>
            <div className="stat-trend negative">
              <ArrowDownIcon width="14" height="14" />
              3.2% <span className="stat-trend-text">from last month</span>
            </div>
          </div>
        </div>
      </div>

      <div className="charts-grid" style={{ gridTemplateColumns: '1.5fr 1fr' }}>
        <div className="chart-card">
          <div className="chart-header">
            <div className="chart-title">Enrollment Overview</div>
            <select className="chart-action chart-action-button">
              <option>This Semester</option>
              <option>Last Semester</option>
            </select>
          </div>
          <div className="chart-container-large">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart
                data={MOCK_ENROLLMENT_DATA}
                margin={{ top: 5, right: 20, bottom: 5, left: 0 }}
              >
                <CartesianGrid
                  strokeDasharray="3 3"
                  vertical={false}
                  stroke="var(--border)"
                />
                <XAxis
                  dataKey="name"
                  axisLine={false}
                  tickLine={false}
                  tick={{ fill: 'var(--text-secondary)', fontSize: 12 }}
                  dy={10}
                />
                <YAxis
                  axisLine={false}
                  tickLine={false}
                  tick={{ fill: 'var(--text-secondary)', fontSize: 12 }}
                  dx={-10}
                />
                <Tooltip
                  contentStyle={{
                    backgroundColor: 'var(--surface)',
                    borderColor: 'var(--border)',
                    borderRadius: '8px',
                    color: 'var(--text-primary)',
                  }}
                  itemStyle={{ color: 'var(--text-primary)' }}
                />
                <Line
                  type="monotone"
                  dataKey="students"
                  stroke="#3B82F6"
                  strokeWidth={3}
                  dot={{
                    r: 4,
                    fill: 'var(--surface)',
                    stroke: '#3B82F6',
                    strokeWidth: 2,
                  }}
                  activeDot={{ r: 6 }}
                />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div
          className="chart-card"
          style={{ display: 'flex', flexDirection: 'column' }}
        >
          <div className="chart-header">
            <div className="chart-title">Recent Announcements</div>
            <button className="chart-action">View All</button>
          </div>
          <div
            className="list-container"
            style={{
              flex: 1,
              display: 'flex',
              flexDirection: 'column',
              gap: '8px',
            }}
          >
            <div className="list-item">
              <div
                className="announcement-icon"
                style={{ background: '#1E3A8A', color: '#3B82F6' }}
              >
                <MegaphoneIconSVG />
              </div>
              <div className="list-content">
                <div className="list-title">
                  <h4>Exam Schedule Published</h4>
                  <span className="list-time">2h ago</span>
                </div>
                <p className="list-desc">
                  The final exam schedule has been published. Check now.
                </p>
              </div>
            </div>

            <div className="list-item">
              <div
                className="announcement-icon"
                style={{ background: '#14532D', color: '#22C55E' }}
              >
                <CalendarIconSVG />
              </div>
              <div className="list-content">
                <div className="list-title">
                  <h4>New Course Available</h4>
                  <span className="list-time">1d ago</span>
                </div>
                <p className="list-desc">
                  Data Science 101 is now available for enrollment.
                </p>
              </div>
            </div>

            <div className="list-item">
              <div
                className="announcement-icon"
                style={{ background: '#4C1D95', color: '#8B5CF6' }}
              >
                <InfoCircleSVG />
              </div>
              <div className="list-content">
                <div className="list-title">
                  <h4>Library Hours Updated</h4>
                  <span className="list-time">2d ago</span>
                </div>
                <p className="list-desc">
                  The library will now close at 8 PM on weekdays.
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="charts-grid" style={{ gridTemplateColumns: '1.2fr 1fr' }}>
        <div
          className="chart-card"
          style={{ display: 'flex', flexDirection: 'column' }}
        >
          <div className="chart-header">
            <div className="chart-title">Students by Department</div>
          </div>
          <div style={{ display: 'flex', flex: 1, alignItems: 'center' }}>
            <div className="chart-container-medium" style={{ flex: 1 }}>
              <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                  <Pie
                    data={MOCK_DEPARTMENT_DATA}
                    cx="50%"
                    cy="50%"
                    innerRadius={70}
                    outerRadius={90}
                    paddingAngle={2}
                    dataKey="value"
                    stroke="none"
                  >
                    {MOCK_DEPARTMENT_DATA.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={entry.color} />
                    ))}
                  </Pie>
                  <Tooltip
                    contentStyle={{
                      backgroundColor: 'var(--surface)',
                      borderColor: 'var(--border)',
                      borderRadius: '8px',
                    }}
                  />
                </PieChart>
              </ResponsiveContainer>
            </div>
            <div className="donut-legend" style={{ flex: 1.2 }}>
              {MOCK_DEPARTMENT_DATA.map((item, i) => (
                <div key={i} className="legend-item">
                  <div
                    className="legend-color"
                    style={{ backgroundColor: item.color }}
                  ></div>
                  <div className="legend-label">{item.name}</div>
                  <div className="legend-value">{item.value}</div>
                  <div className="legend-percentage">{item.percentage}</div>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div
          className="chart-card"
          style={{ display: 'flex', flexDirection: 'column' }}
        >
          <div className="chart-header">
            <div className="chart-title">Upcoming Events</div>
            <button className="chart-action">View Calendar</button>
          </div>
          <div
            className="list-container"
            style={{
              flex: 1,
              display: 'flex',
              flexDirection: 'column',
              gap: '8px',
            }}
          >
            <div className="list-item">
              <div className="date-box">
                <span className="day">24</span>
                <span className="month">MAY</span>
              </div>
              <div className="list-content">
                <div className="list-title">
                  <h4>Guest Lecture: AI in Education</h4>
                  <span className="list-time">10:00 AM</span>
                </div>
                <p className="list-desc">
                  <MapPinSVG width="12" height="12" /> Main Auditorium
                </p>
              </div>
            </div>

            <div className="list-item">
              <div className="date-box">
                <span className="day">28</span>
                <span className="month">MAY</span>
              </div>
              <div className="list-content">
                <div className="list-title">
                  <h4>Midterm Exams Begin</h4>
                  <span className="list-time">09:00 AM</span>
                </div>
                <p className="list-desc">
                  <MapPinSVG width="12" height="12" /> All Departments
                </p>
              </div>
            </div>

            <div className="list-item">
              <div className="date-box">
                <span className="day">05</span>
                <span className="month">JUN</span>
              </div>
              <div className="list-content">
                <div className="list-title">
                  <h4>Engineering Project Fair</h4>
                  <span className="list-time">11:00 AM</span>
                </div>
                <p className="list-desc">
                  <MapPinSVG width="12" height="12" /> Engineering Building
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
