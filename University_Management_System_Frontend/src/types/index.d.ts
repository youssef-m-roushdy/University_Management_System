// TypeScript type definitions for the AYA-UIS application

// User related types
export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface RegisterData {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  role?: string;
}

export interface AuthResponse {
  token: string;
  refreshToken?: string;
  user: User;
  expiresIn: number;
}

// Department related types
export interface Department {
  id: number;
  name: string;
  code: string;
  description?: string;
  createdAt: string;
  updatedAt: string;
}

export interface DepartmentFormData {
  name: string;
  code: string;
  description?: string;
}

// Course related types
export interface Course {
  id: number;
  code: string;
  name: string;
  credits: number;
  description?: string;
  departmentId?: number;
  department?: Department;
  createdAt: string;
  updatedAt: string;
}

export interface CourseFormData {
  code: string;
  name: string;
  credits: number;
  description?: string;
  departmentId?: number;
}

// Fee related types
export interface Fee {
  id: number;
  type: string;
  amount: number;
  description?: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface FeeFormData {
  type: string;
  amount: number;
  description?: string;
  isActive: boolean;
}

// Schedule related types
export interface AcademicSchedule {
  id: number;
  title: string;
  description?: string;
  startDate: string;
  endDate: string;
  type: ScheduleType;
  courseId?: number;
  course?: Course;
  createdAt: string;
  updatedAt: string;
}

export type ScheduleType = 'class' | 'exam' | 'event' | 'holiday';

export interface ScheduleFormData {
  title: string;
  description?: string;
  startDate: string;
  endDate: string;
  type: ScheduleType;
  courseId?: number;
}

// API Response types
export interface ApiResponse<T = any> {
  success: boolean;
  data: T;
  message?: string;
  errors?: string[];
}

export interface PaginatedResponse<T = any> {
  data: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface ErrorResponse {
  success: false;
  message: string;
  errors?: string[];
  statusCode?: number;
}

// Form and UI types
export interface FormField {
  name: string;
  type: 'text' | 'email' | 'password' | 'number' | 'textarea' | 'select' | 'date';
  label: string;
  placeholder?: string;
  required?: boolean;
  validation?: ValidationRule[];
  options?: SelectOption[];
}

export interface SelectOption {
  value: string | number;
  label: string;
}

export interface ValidationRule {
  type: 'required' | 'email' | 'minLength' | 'maxLength' | 'pattern' | 'custom';
  value?: any;
  message: string;
}

// State management types
export interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  status: LoadingStatus;
  error: string | null;
}

export interface AppState {
  auth: AuthState;
  departments: DepartmentState;
  courses: CourseState;
  fees: FeeState;
  schedules: ScheduleState;
}

export interface DepartmentState {
  departments: Department[];
  currentDepartment: Department | null;
  status: LoadingStatus;
  error: string | null;
}

export interface CourseState {
  courses: Course[];
  currentCourse: Course | null;
  status: LoadingStatus;
  error: string | null;
}

export interface FeeState {
  fees: Fee[];
  currentFee: Fee | null;
  status: LoadingStatus;
  error: string | null;
}

export interface ScheduleState {
  schedules: AcademicSchedule[];
  currentSchedule: AcademicSchedule | null;
  status: LoadingStatus;
  error: string | null;
}

export type LoadingStatus = 'idle' | 'loading' | 'success' | 'error';

// Component prop types
export interface TableColumn {
  key: string;
  label: string;
  sortable?: boolean;
  render?: (value: any, row: any) => React.ReactNode;
}

export interface TableProps {
  data: any[];
  columns: TableColumn[];
  loading?: boolean;
  pagination?: PaginationProps;
  onRowClick?: (row: any) => void;
  className?: string;
}

export interface PaginationProps {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  onPageChange: (page: number) => void;
  onPageSizeChange: (pageSize: number) => void;
}

export interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  size?: 'small' | 'medium' | 'large';
  children: React.ReactNode;
}

export interface ToastMessage {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  message: string;
  duration?: number;
}

// Navigation and routing types
export interface NavItem {
  label: string;
  path: string;
  icon?: React.ReactNode;
  children?: NavItem[];
  roles?: string[];
}

export interface BreadcrumbItem {
  label: string;
  path?: string;
}

// Utility types
export type Optional<T, K extends keyof T> = Omit<T, K> & Partial<Pick<T, K>>;
export type RequiredFields<T, K extends keyof T> = T & Required<Pick<T, K>>;
export type PartialExcept<T, K extends keyof T> = Partial<T> & Pick<T, K>;

// Generic CRUD operations
export interface CrudOperations<T, TFormData = Partial<T>> {
  getAll: () => Promise<T[]>;
  getById: (id: number | string) => Promise<T>;
  create: (data: TFormData) => Promise<T>;
  update: (id: number | string, data: TFormData) => Promise<T>;
  delete: (id: number | string) => Promise<void>;
}

// Generic paginated operations
export interface PaginatedOperations<T> {
  getPaginated: (
    page?: number,
    pageSize?: number,
    filters?: Record<string, any>
  ) => Promise<PaginatedResponse<T>>;
  search: (
    query: string,
    filters?: Record<string, any>
  ) => Promise<PaginatedResponse<T>>;
}