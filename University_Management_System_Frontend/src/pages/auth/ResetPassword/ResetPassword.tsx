// pages/auth/ResetPassword/ResetPassword.tsx

import React, { useState, useEffect, ChangeEvent, FormEvent } from 'react';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import {
  MdLock,
  MdVisibility,
  MdVisibilityOff,
  MdArrowBack,
  MdErrorOutline,
  MdCheckCircleOutline,
} from 'react-icons/md';
import apiService from '../../../services/apiService';
import { API_ENDPOINTS, ROUTES } from '../../../constants';
import logo from '../../../assets/images/logo.svg';
import './ResetPassword.css';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface ResetPasswordResponse {
  success: boolean;
  statusCode: number;
  message: string;
  data: string;
  errors?: Array<{
    code: string;
    message: string;
    field?: string;
  }>;
  timestamp: string;
}

interface PasswordFieldProps {
  label: string;
  value: string;
  onChange: (e: ChangeEvent<HTMLInputElement>) => void;
  showPassword: boolean;
  onToggleShow: () => void;
  placeholder: string;
  autoComplete: string;
  icon?: React.ElementType;
  disabled?: boolean;
}

// ──────────────────────────────────────────────────────────────────────────────
// PASSWORD FIELD COMPONENT
// ──────────────────────────────────────────────────────────────────────────────

const PasswordField: React.FC<PasswordFieldProps> = ({
  label,
  value,
  onChange,
  showPassword,
  onToggleShow,
  placeholder,
  autoComplete,
  icon: Icon = MdLock,
  disabled = false,
}) => (
  <div className="auth-input-group">
    <label>{label}</label>
    <div className="auth-input-wrapper">
      <Icon className="auth-field-icon" />
      <input
        type={showPassword ? 'text' : 'password'}
        value={value}
        onChange={onChange}
        placeholder={placeholder}
        required
        autoComplete={autoComplete}
        disabled={disabled}
      />
      <button
        type="button"
        className="auth-toggle-pass"
        onClick={onToggleShow}
        aria-label={`Toggle ${label} visibility`}
        tabIndex={-1}
        disabled={disabled}
      >
        {showPassword ? <MdVisibilityOff /> : <MdVisibility />}
      </button>
    </div>
  </div>
);

// ──────────────────────────────────────────────────────────────────────────────
// MAIN COMPONENT
// ──────────────────────────────────────────────────────────────────────────────

export default function ResetPassword(): React.ReactElement {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const [newPassword, setNewPassword] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');
  const [showNewPassword, setShowNewPassword] = useState<boolean>(false);
  const [showConfirmPassword, setShowConfirmPassword] =
    useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(false);
  const [success, setSuccess] = useState<boolean>(false);
  const [error, setError] = useState<string>('');

  const email = searchParams.get('email') || '';
  const token = searchParams.get('token') || '';

  const isValidLink = email && email.includes('@') && token;

  useEffect(() => {
    if (!isValidLink) {
      setError('Invalid or expired reset link. Please request a new one.');
    }
  }, [isValidLink]);

  // ─── Handlers ──────────────────────────────────────────────────────────────

  const handleNewPasswordChange = (e: ChangeEvent<HTMLInputElement>): void => {
    setError('');
    setNewPassword(e.target.value);
  };

  const handleConfirmPasswordChange = (
    e: ChangeEvent<HTMLInputElement>
  ): void => {
    setError('');
    setConfirmPassword(e.target.value);
  };

  const toggleShowNewPassword = (): void => {
    setShowNewPassword(prev => !prev);
  };

  const toggleShowConfirmPassword = (): void => {
    setShowConfirmPassword(prev => !prev);
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();
    setError('');

    if (newPassword !== confirmPassword) {
      setError('Passwords do not match.');
      return;
    }

    if (newPassword.length < 8) {
      setError('Password must be at least 8 characters.');
      return;
    }

    if (!/[A-Z]/.test(newPassword)) {
      setError('Password must contain at least one uppercase letter.');
      return;
    }

    if (!/[0-9]/.test(newPassword)) {
      setError('Password must contain at least one number.');
      return;
    }

    setLoading(true);

    try {
      await apiService.post<ResetPasswordResponse>(
        API_ENDPOINTS.AUTH.RESET_PASSWORD,
        {
          email,
          token,
          newPassword,
          confirmPassword,
        }
      );
      setSuccess(true);
      setTimeout(() => navigate(ROUTES.LOGIN), 3000);
    } catch (err: any) {
      setError(
        err?.errorMessage ||
          err?.message ||
          'Reset failed. The link may have expired.'
      );
    } finally {
      setLoading(false);
    }
  };

  // ─── Password Strength Checks ─────────────────────────────────────────────

  const hasMinLength = newPassword.length >= 8;
  const hasUppercase = /[A-Z]/.test(newPassword);
  const hasNumber = /[0-9]/.test(newPassword);
  const isStrongPassword = hasMinLength && hasUppercase && hasNumber;

  // ─── Render ──────────────────────────────────────────────────────────────────

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="auth-logo-wrapper">
          <img src={logo} alt="AYA Academy" className="auth-logo" />
        </div>

        {success ? (
          <div className="auth-success">
            <div className="auth-success-icon auth-success-icon--green">
              <MdCheckCircleOutline />
            </div>
            <h2>Password Reset!</h2>
            <p>Your password has been successfully updated.</p>
            <p className="auth-hint">Redirecting to sign in...</p>
            <Link to={ROUTES.LOGIN} className="auth-back-link">
              <MdArrowBack /> Go to Sign In
            </Link>
          </div>
        ) : (
          <>
            <div className="auth-header">
              <h2>Set New Password</h2>
              <p>Choose a strong password for your account.</p>
            </div>

            {error && (
              <div className="auth-error">
                <MdErrorOutline />
                <span>{error}</span>
              </div>
            )}

            <form onSubmit={handleSubmit}>
              <PasswordField
                label="New Password"
                value={newPassword}
                onChange={handleNewPasswordChange}
                showPassword={showNewPassword}
                onToggleShow={toggleShowNewPassword}
                placeholder="Enter new password"
                autoComplete="new-password"
                icon={MdLock}
                disabled={loading}
              />

              <PasswordField
                label="Confirm Password"
                value={confirmPassword}
                onChange={handleConfirmPasswordChange}
                showPassword={showConfirmPassword}
                onToggleShow={toggleShowConfirmPassword}
                placeholder="Confirm new password"
                autoComplete="new-password"
                icon={MdLock}
                disabled={loading}
              />

              {newPassword && (
                <div className="auth-password-rules">
                  <span className={hasMinLength ? 'rule-pass' : 'rule-fail'}>
                    {hasMinLength ? '✓' : '✗'} At least 8 characters
                  </span>
                  <span className={hasUppercase ? 'rule-pass' : 'rule-fail'}>
                    {hasUppercase ? '✓' : '✗'} One uppercase letter
                  </span>
                  <span className={hasNumber ? 'rule-pass' : 'rule-fail'}>
                    {hasNumber ? '✓' : '✗'} One number
                  </span>
                  {newPassword && isStrongPassword && (
                    <span
                      className="rule-pass"
                      style={{ marginTop: '4px', fontWeight: 'bold' }}
                    >
                      ✓ Strong password!
                    </span>
                  )}
                </div>
              )}

              <button
                type="submit"
                className="auth-submit-btn"
                disabled={loading || !isValidLink || !isStrongPassword}
              >
                <span className="button-content">
                  {loading ? (
                    <>
                      <span className="spinner" /> Resetting...
                    </>
                  ) : (
                    'Reset Password'
                  )}
                </span>
              </button>
            </form>

            <Link to={ROUTES.LOGIN} className="auth-back-link">
              <MdArrowBack /> Back to Sign In
            </Link>
          </>
        )}
      </div>
    </div>
  );
}
