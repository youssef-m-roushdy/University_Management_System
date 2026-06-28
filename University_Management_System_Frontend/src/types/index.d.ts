// src/types/index.ts

export interface User {
  id: string;
  email: string;
  name: string;
  roles: string[];
  profilePicture?: string;
}

export interface StudentProfile {
  academicCode: string;
  level: string;
  totalGPA: number;
  departmentId: number;
  departmentName: string;
  specializationId: number;
  specializationName: string;
}

export interface InstructorProfile {
  departmentId: number;
  departmentName: string;
}

export interface AssistantProfile {
  departmentId: number;
  departmentName: string;
}

export interface AuthData {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiry: string;
  userId: string;
  email: string;
  name: string;
  profilePicture?: string;
  roles: string[];
  studentProfile?: StudentProfile;
  adminProfile?: any;
  instructorProfile?: InstructorProfile;
  assistantProfile?: AssistantProfile;
}

export interface ApiError {
  code: string;
  message: string;
  field?: string;
}

export interface ApiResponse<T = any> {
  success: boolean;
  statusCode: number;
  message: string;
  data: T;
  errors?: ApiError[];
  timestamp: string;
}

export interface AuthResponse extends ApiResponse<AuthData> {}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface RefreshTokenRequest {
  accessToken: string;
  refreshToken: string;
}

export interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
  confirmPassword: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export interface VerifyEmailRequest {
  email: string;
  token: string;
}

export interface LogoutRequest {
  refreshToken: string;
}

export interface RevokeTokenRequest {
  refreshToken: string;
} // types/index.ts

export interface User {
  id: string;
  userId: string;
  email: string;
  name: string;
  roles: UserRole[];
  profilePicture?: string;
  studentProfile?: StudentProfile;
  instructorProfile?: InstructorProfile;
  assistantProfile?: AssistantProfile;
  adminProfile?: any;
  academicCode?: string;
  [key: string]: any;
}

export interface StudentProfile {
  academicCode: string;
  level: string;
  totalGPA: number;
  departmentId: number;
  departmentName: string;
  specializationId: number;
  specializationName: string;
}

export interface InstructorProfile {
  departmentId: number;
  departmentName: string;
}

export interface AssistantProfile {
  departmentId: number;
  departmentName: string;
}

export interface AuthData {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiry: string;
  userId: string;
  email: string;
  name: string;
  profilePicture?: string;
  roles: UserRole[];
  studentProfile?: StudentProfile;
  adminProfile?: any;
  instructorProfile?: InstructorProfile;
  assistantProfile?: AssistantProfile;
}

export interface ApiError {
  code: string;
  message: string;
  field?: string;
}

export interface ApiResponse<T = any> {
  success: boolean;
  statusCode: number;
  message: string;
  data: T;
  errors?: ApiError[];
  timestamp: string;
}

export interface AuthResponse extends ApiResponse<AuthData> {
  // AuthResponse has a 'data' property of type AuthData
  // The user is accessed via response.data
}

// Helper type for login response
export interface LoginResponse {
  success: boolean;
  statusCode: number;
  message: string;
  data: AuthData;
  errors?: ApiError[];
  timestamp: string;
}

// Type guard to check if response has user data
export const isAuthResponse = (response: any): response is AuthResponse => {
  return response && response.success && response.data && response.data.userId;
};
