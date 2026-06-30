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
  ACADEMIC_SCHEDULES: {
    BASE: '/api/AcademicSchedule',
    BY_ID: (id: number) => `/api/AcademicSchedule/${id}`,
    BY_DEPARTMENT: (departmentId: number) =>
      `/api/AcademicSchedule/department/${departmentId}`,
    BY_DEPARTMENT_SEMESTER: (departmentId: number, semesterId: number) =>
      `/api/AcademicSchedule/department/${departmentId}/semester/${semesterId}`,
    SEARCH: '/api/AcademicSchedule/search', // You might need to add this endpoint
  },
  ADMINS: {
    BASE: '/api/Admin',
    BY_ID: (id: string) => `/api/Admin/${id}`,
    ADD_TO_EXISTING_USER: '/api/Admin/add-to-existing-user',
  },
  ASSISTANTS: {
    BASE: '/api/Assistant',
    BY_ID: (id: string) => `/api/Assistant/${id}`,
    BY_DEPARTMENT: (departmentId: number) =>
      `/api/Assistant/department/${departmentId}`,
    ADD_TO_EXISTING_USER: '/api/Assistant/add-to-existing-user',
  },
  INSTRUCTORS: {
    BASE: '/api/Instructor',
    BY_ID: (id: string) => `/api/Instructor/${id}`,
    BY_DEPARTMENT: (departmentId: number) =>
      `/api/Instructor/department/${departmentId}`,
    ADD_TO_EXISTING_USER: '/api/Instructor/add-to-existing-user',
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
    BASE: '/api/Roles',
    BY_ID: (roleId: string) => `/api/Roles/${roleId}`,
    BY_NAME: (roleName: string) => `/api/Roles/by-name/${roleName}`,
  },
  DEPARTMENTS: {
    BASE: `${API_BASE}/Departments`,
    BY_ID: (id: number) => `${API_BASE}/Departments/${id}`,
  },
  DEPARTMENT_COURSES: {
    BASE: '/api/DepartmentCourse',
    ALL: '/api/DepartmentCourse/all',
    BY_DEPARTMENT: (departmentId: number) =>
      `/api/DepartmentCourse/department/${departmentId}`,
    BULK: '/api/DepartmentCourse/bulk',
    UPDATE: (departmentId: number, courseId: number) =>
      `/api/DepartmentCourse/${departmentId}/${courseId}`,
    DELETE: (departmentId: number, courseId: number) =>
      `/api/DepartmentCourse/${departmentId}/${courseId}`,
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
  COURSE_PREREQUISITES: {
    PREREQUISITES: (id: number) =>
      `/api/CoursePrerequisite/${id}/prerequisites`,
    DEPENDENCIES: (id: number) => `/api/CoursePrerequisite/${id}/dependencies`,
    ADD_PREREQUISITE: '/api/CoursePrerequisite/prerequisites',
    ADD_PREREQUISITES_BULK: '/api/CoursePrerequisite/prerequisites/bulk',
    REMOVE_PREREQUISITE: (courseId: number, prerequisiteCourseId: number) =>
      `/api/CoursePrerequisite/prerequisites/${courseId}/${prerequisiteCourseId}`,
    ADD_DEPENDENCY: '/api/CoursePrerequisite/dependencies',
    ADD_DEPENDENCIES_BULK: '/api/CoursePrerequisite/dependencies/bulk',
    REMOVE_DEPENDENCY: (courseId: number, dependencyCourseId: number) =>
      `/api/CoursePrerequisite/dependencies/${courseId}/${dependencyCourseId}`,
  },
  REGISTRATIONS: {
    BASE: '/api/Registration',
    BY_ID: (id: number) => `/api/Registration/${id}`,
    STUDENT_ALL: '/api/Registration/student/all',
    BY_STUDY_YEAR: (studyYearId: number) =>
      `/api/Registration/${studyYearId}/year`,
    BY_STUDY_YEAR_SEMESTER: (studyYearId: number, semesterId: number) =>
      `/api/Registration/student/${studyYearId}/year/${semesterId}/semester`,
    BY_SEMESTER_STUDY_YEAR: (semesterId: number, studyYearId: number) =>
      `/api/Registration/semester/${semesterId}/study-year/${studyYearId}`,
    REGISTRATIONS_BY_STUDY_YEAR: (studyYearId: number) =>
      `/api/Registration/study-year/${studyYearId}/registrations`,
    UPDATE_STATUS: (registrationId: number) =>
      `/api/Registration/${registrationId}/status`,
    UPDATE_GRADE: (registrationId: number) =>
      `/api/Registration/${registrationId}/grade`,
    BULK_UPDATE_GRADES: '/api/Registration/grades/bulk',
  },
  SEMESTERS: {
    BASE: '/api/Semester',
    BY_ID: (id: number) => `/api/Semester/${id}`,
    BY_STUDY_YEAR: (studyYearId: number) =>
      `/api/Semester/study-year/${studyYearId}`,
    CREATE_FOR_STUDY_YEAR: (studyYearId: number) =>
      `/api/Semester/study-year/${studyYearId}`,
  },
  SEMESTER_GPA: {
    BY_STUDENT: (id: number) => `/api/SemesterGPA/student/${id}`,
    BY_STUDY_YEAR: (studyYearId: number) =>
      `/api/SemesterGPA/studyyear/${studyYearId}`,
    BY_SEMESTER: (semesterId: number) =>
      `/api/SemesterGPA/semester/${semesterId}`,
  },
  SPECIALIZATIONS: {
    BASE: '/api/Specialization',
    BY_ID: (id: number) => `/api/Specialization/${id}`,
    BY_DEPARTMENT: (departmentId: number) =>
      `/api/Specialization/department/${departmentId}`,
  },
  SPECIALIZATION_COURSES: {
    BASE: '/api/SpecializationCourse',
    BY_SPECIALIZATION: (specializationId: number) =>
      `/api/SpecializationCourse/specialization/${specializationId}`,
    BY_COURSE: (courseId: number) =>
      `/api/SpecializationCourse/course/${courseId}`,
    BULK: '/api/SpecializationCourse/bulk',
    UPDATE: (specializationId: number, courseId: number) =>
      `/api/SpecializationCourse/${specializationId}/${courseId}`,
    DELETE: (specializationId: number, courseId: number) =>
      `/api/SpecializationCourse/${specializationId}/${courseId}`,
  },
  STUDY_YEARS: {
    BASE: '/api/StudyYear',
    BY_ID: (id: number) => `/api/StudyYear/${id}`,
    CURRENT: '/api/StudyYear/current',
  },
  STUDENTS: {
    BASE: '/api/Student',
    BY_ID: (id: string) => `/api/Student/${id}`,
    BY_DEPARTMENT: (departmentId: number) =>
      `/api/Student/department/${departmentId}`,
    BY_ACADEMIC_CODE: (academicCode: string) =>
      `/api/Student/academic-code/${academicCode}`,
    ADD_TO_EXISTING_USER: '/api/Student/add-to-existing-user',
  },
  STUDENT_STUDY_YEARS: {
    BASE: '/api/StudentStudyYear',
    BY_ID: (id: number) => `/api/StudentStudyYear/${id}`,
    BY_STUDY_YEAR: (studyYearId: number) =>
      `/api/StudentStudyYear/study-year/${studyYearId}`,
    ME: '/api/StudentStudyYear/me',
    ME_CURRENT: '/api/StudentStudyYear/me/current',
    STUDENT_TIMELINE: (studentId: string) =>
      `/api/StudentStudyYear/student/${studentId}/timeline`,
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
} as const;

// ──────────────────────────────────────────────────────────────────────────────
// ROUTES
// ──────────────────────────────────────────────────────────────────────────────

export const ROUTES = {
  LOGIN: '/login',
  FORGOT_PASSWORD: '/forgot-password',
  RESET_PASSWORD: '/reset-password',
  DASHBOARD_ME: '/dashboard/me',
  ADMIN: {
    DASHBOARD: '/admin/dashboard',
    DEPARTMENTS: '/admin/departments',
    ADMINS: '/admin/admins',
    COURSES: '/admin/courses',
    STUDENTS: '/admin/students',
    USERS: '/admin/users',
    ASSISTANTS: '/admin/assistants',
    INSTRUCTORS: '/admin/instructors',
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
