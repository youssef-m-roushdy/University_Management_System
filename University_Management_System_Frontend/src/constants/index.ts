// constants/index.ts

export const API_BASE =
  process.env.REACT_APP_API_URL || 'http://localhost:5282';

// ──────────────────────────────────────────────────────────────────────────────
// API ENDPOINTS
// ──────────────────────────────────────────────────────────────────────────────

export const API_ENDPOINTS = {
  AUTH: {
    LOGIN: '/api/auth/login',
    LOGOUT: '/api/auth/logout',
    REFRESH_TOKEN: '/api/auth/refresh-token',
    REVOKE_TOKEN: '/api/auth/revoke-token',
    REVOKE_ALL_TOKENS: '/api/auth/revoke-all-tokens',
    FORGOT_PASSWORD: '/api/auth/forgot-password',
    RESET_PASSWORD: '/api/auth/reset-password',
    CHANGE_PASSWORD: '/api/auth/change-password',
    VERIFY_EMAIL: '/api/auth/verify-email',
    RESEND_VERIFICATION: '/api/auth/resend-verification',
  },
  USERS: {
    BASE: `${API_BASE}/User`,
    MY_PROFILE: `${API_BASE}/User/profile`,
    PROFILE_BY_ID: (userId: string) => `${API_BASE}/User/profile/${userId}`,
    PROFILE: (academicCode: string) =>
      `${API_BASE}/User/${academicCode}/academic`,
    BY_ID: (userId: string) => `${API_BASE}/User/${userId}`,
    BY_EMAIL: (email: string) => `${API_BASE}/User/by-email/${email}`,
    UPDATE_PROFILE_PICTURE: `${API_BASE}/User/profile-picture`,
    DELETE_PROFILE_PICTURE: `${API_BASE}/User/profile-picture`,
    UPDATE_SPECIALIZATION: `${API_BASE}/User/update-student-specialization`,
    ACTIVATE: (userId: string) => `${API_BASE}/User/activate/${userId}`,
    DEACTIVATE: (userId: string) => `${API_BASE}/User/deactivate/${userId}`,
  },
  ROLES: {
    BASE: `${API_BASE}/Roles`,
    BY_ID: (id: number) => `${API_BASE}/Roles/${id}`,
    BY_NAME: (name: string) => `${API_BASE}/Roles/by-name/${name}`,
    UPDATE_BY_EMAIL: `${API_BASE}/Roles/update-user-role-by-email`,
    UPDATE_BY_CODE: `${API_BASE}/Roles/update-user-role`,
    USER_ROLE_INFO: (code: string) =>
      `${API_BASE}/Roles/user-role-info/${code}`,
  },
  DEPARTMENTS: {
    BASE: `${API_BASE}/Departments`,
    BY_ID: (id: number) => `${API_BASE}/Departments/${id}`,
  },
  COURSES: {
    BASE: `${API_BASE}/Course`,
    BY_ID: (id: number) => `${API_BASE}/Course/${id}`,
    UPLOADS: (id: number) => `${API_BASE}/Course/${id}/uploads`,
    REGISTRATIONS: (id: number, yearId: number) =>
      `${API_BASE}/Course/${id}/registrations/${yearId}`,
    UPLOAD_FILE: (courseId: number) => `${API_BASE}/Course/${courseId}/upload`,
    DEPT_COURSES: (deptId: number) => `${API_BASE}/Course/department/${deptId}`,
    PREREQUISITES: (id: number) => `${API_BASE}/Course/prequisites/${id}`,
    DEPENDENCIES: (id: number) => `${API_BASE}/Course/dependencies/${id}`,
    OPEN_BY_DEPT: (deptId: number) =>
      `${API_BASE}/Course/open/department/${deptId}`,
    UPDATE_STATUS: (id: number) => `${API_BASE}/Course/${id}/status`,
    SEARCH: `${API_BASE}/Course/search`,
    STUDENT_REGISTRATION_COURSES: (studyYearId: number, semesterId: number) =>
      `${API_BASE}/Course/student-registration/study-year/${studyYearId}/semester/${semesterId}`,
  },
  REGISTRATIONS: {
    BASE: `${API_BASE}/Registration`,
    BY_ID: (id: number) => `${API_BASE}/Registration/${id}`,
    BY_YEAR: (yearId: number) => `${API_BASE}/Registration/${yearId}/year`,
    BY_SEMESTER: (yearId: number, semId: number) =>
      `${API_BASE}/Registration/student/${yearId}/year/${semId}/semester`,
    PENDING: (studyYearId: number, semesterId: number) =>
      `${API_BASE}/Registration/${studyYearId}/year/${semesterId}/semester`,
  },
  SEMESTERS: {
    BY_YEAR: (yearId: number) => `${API_BASE}/Semester/${yearId}/study-year`,
  },
  STUDY_YEARS: {
    BASE: `${API_BASE}/StudyYear`,
    BY_ID: (id: number) => `${API_BASE}/StudyYear/${id}`,
  },
  FEES: {
    BASE: `${API_BASE}/Fee`,
    BY_ID: (id: number) => `${API_BASE}/Fee/${id}`,
    BY_STUDY_YEAR: (studyYearId: number) =>
      `${API_BASE}/Fee/study-year/${studyYearId}`,
    BY_DEPT_AND_YEAR: (deptId: number, studyYearId: number) =>
      `${API_BASE}/Fee/department/${deptId}/study-year/${studyYearId}`,
  },
  USER_STUDY_YEARS: {
    BASE: `${API_BASE}/UserStudyYear`,
    BY_ID: (id: number) => `${API_BASE}/UserStudyYear/${id}`,
    MY_STUDY_YEARS: `${API_BASE}/UserStudyYear/my-study-years`,
    MY_TIMELINE: `${API_BASE}/UserStudyYear/my-timeline`,
    MY_CURRENT: `${API_BASE}/UserStudyYear/my-current`,
    BY_USER: (userId: string) => `${API_BASE}/UserStudyYear/user/${userId}`,
    USER_TIMELINE: (userId: string) =>
      `${API_BASE}/UserStudyYear/user/${userId}/timeline`,
    PROMOTE_ALL: `${API_BASE}/UserStudyYear/promote-all`,
    PROMOTE_STUDENT: (code: string) =>
      `${API_BASE}/UserStudyYear/promote-student/${code}`,
  },
  DEPARTMENT_FEES: {
    BASE: `${API_BASE}/DepartmentFees`,
    BY_DEPT_GRADE: (name: string, grade: string) =>
      `${API_BASE}/DepartmentFees/${name}/${grade}`,
  },
  SCHEDULES: {
    BASE: `${API_BASE}/AcademicSchedule`,
    BY_ID: (id: number) => `${API_BASE}/AcademicSchedule/${id}`,
    CREATE: (yearId: number, deptId: number, semId: number) =>
      `${API_BASE}/AcademicSchedule/study-year/${yearId}/department/${deptId}/semester/${semId}`,
    BY_SEMESTER: (semId: number) =>
      `${API_BASE}/AcademicSchedule/semester/${semId}`,
  },
} as const;

// ──────────────────────────────────────────────────────────────────────────────
// ROUTES
// ──────────────────────────────────────────────────────────────────────────────

export const ROUTES = {
  HOME: '/',
  LOGIN: '/login',
  FORGOT_PASSWORD: '/forgot-password',
  RESET_PASSWORD: '/reset-password',
  DASHBOARD: '/dashboard',
  ADMIN: {
    DASHBOARD: '/admin/dashboard',
    DEPARTMENTS: '/admin/departments',
    COURSES: '/admin/courses',
    STUDENTS: '/admin/students',
    USERS: '/admin/users',
    STUDY_YEARS: '/admin/study-years',
    STUDY_YEAR_MANAGE: '/admin/study-year/:studyYearId/manage',
    SEMESTER_DETAIL:
      '/admin/study-year/:studyYearId/semester/:semesterId/detail',
    REGISTRATIONS: '/admin/registrations',
    FEES: '/admin/fees',
    SCHEDULES: '/admin/schedules',
    ROLES: '/admin/roles',
    PROMOTE_STUDENTS: '/admin/promote-students',
    STUDY_YEAR_FEES: '/admin/study-years/:studyYearId/fees',
  },
  STUDENT: {
    DASHBOARD: '/student/dashboard',
    MY_COURSES: '/student/my-courses',
    REGISTER_COURSES: '/student/register',
    MY_TIMELINE: '/student/timeline',
    MY_STUDY_YEARS: '/student/my-study-years',
    STUDY_YEAR_DETAILS: '/student/study-year/:studyYearId',
    SEMESTER_COURSES:
      '/student/study-year/:studyYearId/semester/:semesterId/courses',
    DEPARTMENT_COURSES: '/student/courses',
    STUDY_YEAR_SEMESTERS: '/student/study-year/:studyYearId/semesters',
    COURSE_UPLOADS: '/student/course/:courseId/uploads',
    PROFILE: '/student/profile',
    SCHEDULES: '/student/schedules',
    CHANGE_PASSWORD: '/student/change-password',
  },
  INSTRUCTOR: {
    DASHBOARD: '/instructor/dashboard',
    COURSES: '/instructor/courses',
    STUDENTS: '/instructor/students',
    ASSIGNMENTS: '/instructor/assignments',
  },
  ASSISTANT: {
    DASHBOARD: '/assistant/dashboard',
    COURSES: '/assistant/courses',
    STUDENTS: '/assistant/students',
    GRADING: '/assistant/grading',
  },
} as const;

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

export type RoutePath = (typeof ROUTES)[keyof typeof ROUTES];
export type AdminRoutePath = (typeof ROUTES.ADMIN)[keyof typeof ROUTES.ADMIN];
export type StudentRoutePath =
  (typeof ROUTES.STUDENT)[keyof typeof ROUTES.STUDENT];
export type InstructorRoutePath =
  (typeof ROUTES.INSTRUCTOR)[keyof typeof ROUTES.INSTRUCTOR];
export type AssistantRoutePath =
  (typeof ROUTES.ASSISTANT)[keyof typeof ROUTES.ASSISTANT];

// ──────────────────────────────────────────────────────────────────────────────
// ENUMS & CONSTANTS
// ──────────────────────────────────────────────────────────────────────────────

export const GENDER_OPTIONS = {
  MALE: 'Male',
  FEMALE: 'Female',
} as const;

export type Gender = (typeof GENDER_OPTIONS)[keyof typeof GENDER_OPTIONS];

export const USER_ROLES = {
  ADMIN: 'Admin',
  INSTRUCTOR: 'Instructor',
  ASSISTANT: 'Assistant',
  STUDENT: 'Student',
} as const;

export type UserRole = (typeof USER_ROLES)[keyof typeof USER_ROLES];

export const STORAGE_KEYS = {
  ACCESS_TOKEN: 'access_token',
  REFRESH_TOKEN: 'refresh_token',
  TOKEN_EXPIRY: 'token_expiry',
  USER: 'user',
} as const;

export type StorageKey = (typeof STORAGE_KEYS)[keyof typeof STORAGE_KEYS];

export const STATUS = {
  IDLE: 'idle',
  LOADING: 'loading',
  SUCCESS: 'success',
  ERROR: 'error',
} as const;

export type Status = (typeof STATUS)[keyof typeof STATUS];

export const REGISTRATION_STATUS = {
  PENDING: 'Pending',
  APPROVED: 'Approved',
  SUSPENDED: 'Suspended',
  REJECTED: 'Rejected',
} as const;

export type RegistrationStatus =
  (typeof REGISTRATION_STATUS)[keyof typeof REGISTRATION_STATUS];

export const COURSE_PROGRESS = {
  COMPLETED: 'Completed',
  IN_PROGRESS: 'InProgress',
  NOT_STARTED: 'NotStarted',
} as const;

export type CourseProgress =
  (typeof COURSE_PROGRESS)[keyof typeof COURSE_PROGRESS];

export const LEVELS = {
  PREPARATORY: 'Preparatory_Year',
  FIRST: 'First_Year',
  SECOND: 'Second_Year',
  THIRD: 'Third_Year',
  FOURTH: 'Fourth_Year',
  GRADUATE: 'Graduate',
} as const;

export type Level = (typeof LEVELS)[keyof typeof LEVELS];

export const LEVEL_LABELS: Record<Level, string> = {
  Preparatory_Year: 'Preparatory',
  First_Year: '1st Year',
  Second_Year: '2nd Year',
  Third_Year: '3rd Year',
  Fourth_Year: '4th Year',
  Graduate: 'Graduate',
} as const;

export const SEMESTER_TYPES = {
  FIRST: 'First_Semester',
  SECOND: 'Second_Semester',
  SUMMER: 'Summer',
} as const;

export type SemesterType = (typeof SEMESTER_TYPES)[keyof typeof SEMESTER_TYPES];

export const GRADE_LABELS = {
  A_Plus: 'A+',
  A: 'A',
  A_Minus: 'A-',
  B_Plus: 'B+',
  B: 'B',
  B_Minus: 'B-',
  C_Plus: 'C+',
  C: 'C',
  C_Minus: 'C-',
  D_Plus: 'D+',
  D: 'D',
  D_Minus: 'D-',
  F: 'F',
} as const;

export type GradeLabel = (typeof GRADE_LABELS)[keyof typeof GRADE_LABELS];

// ──────────────────────────────────────────────────────────────────────────────
// HELPER FUNCTIONS
// ──────────────────────────────────────────────────────────────────────────────

export const getLevelLabel = (level: Level): string => {
  return LEVEL_LABELS[level] || level;
};

export const getGradeLabel = (grade: string): string => {
  return GRADE_LABELS[grade as keyof typeof GRADE_LABELS] || grade;
};

export const getSemesterTypeLabel = (semester: SemesterType): string => {
  const labels: Record<SemesterType, string> = {
    First_Semester: 'First Semester',
    Second_Semester: 'Second Semester',
    Summer: 'Summer Semester',
  };
  return labels[semester] || semester;
};

export const isAdmin = (role: string): boolean => role === USER_ROLES.ADMIN;
export const isInstructor = (role: string): boolean =>
  role === USER_ROLES.INSTRUCTOR;
export const isAssistant = (role: string): boolean =>
  role === USER_ROLES.ASSISTANT;
export const isStudent = (role: string): boolean => role === USER_ROLES.STUDENT;

export const getRoleLabel = (role: UserRole): string => {
  const labels: Record<UserRole, string> = {
    Admin: 'Administrator',
    Instructor: 'Instructor',
    Assistant: 'Teaching Assistant',
    Student: 'Student',
  };
  return labels[role] || role;
};
