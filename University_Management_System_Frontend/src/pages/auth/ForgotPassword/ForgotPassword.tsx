// pages/auth/ForgotPassword/ForgotPassword.tsx

import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../../contexts/AuthContext';
import { ROUTES, STATUS } from '../../../constants';
import { useTheme } from '../../../contexts/ThemeContext';
import {
  MailIcon,
  ChevronLeftIcon,
  SunIcon,
  MoonIcon,
  AlertIcon,
} from '../../../components/icons/Icons';
import UniversityIllustration from '../../../components/illustration/UniversityIllustration';
import './ForgotPassword.css';

export default function ForgotPassword() {
  const [email, setEmail] = useState('');
  const [submitted, setSubmitted] = useState(false);
  const { status, error, clearError } = useAuth();
  const { mode, toggleTheme } = useTheme();
  const navigate = useNavigate();

  const isLoading = status === STATUS.LOADING;

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    clearError();
    // TODO: Implement password reset API call
    // For now, just simulate success
    setSubmitted(true);
  };

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    clearError();
    setEmail(e.target.value);
  };

  return (
    <div className="forgot-password-page">
      <div className="forgot-password-card">
        {/* Theme Toggle */}
        <button
          type="button"
          className="theme-toggle"
          onClick={toggleTheme}
          aria-label={
            mode === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'
          }
          title={
            mode === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'
          }
        >
          {mode === 'dark' ? <SunIcon /> : <MoonIcon />}
        </button>

        {/* Back Button */}
        <button
          type="button"
          className="back-button"
          onClick={() => navigate(ROUTES.LOGIN)}
          aria-label="Back to login"
        >
          <ChevronLeftIcon width={20} height={20} />
          Back to Login
        </button>

        {/* Left: brand panel */}
        <div className="forgot-panel forgot-panel--brand">
          <div className="brand">
            <p className="brand__name">Akhbar Alyoum Academy</p>
            <p className="brand__tagline">University Information System</p>
          </div>

          <div className="brand-copy">
            <h1 className="brand-heading">
              Reset <span className="brand-heading__accent">Password</span>
            </h1>
            <p className="brand-subtext">
              Don't worry! Enter your email address and we'll send you a link to
              reset your password.
            </p>
          </div>

          <div className="illustration">
            <UniversityIllustration />
          </div>
        </div>

        {/* Right: form panel */}
        <div className="forgot-panel forgot-panel--form">
          {!submitted ? (
            <>
              <div className="form-header">
                <h2 className="form-heading">Forgot Password?</h2>
                <p className="form-subtext">
                  Enter the email address associated with your account.
                </p>
              </div>

              {error && (
                <div className="error-alert" role="alert">
                  <AlertIcon className="error-alert__icon" />
                  <span>{error}</span>
                </div>
              )}

              <form className="forgot-form" onSubmit={handleSubmit}>
                <div className="field">
                  <label htmlFor="email" className="field__label">
                    Email Address
                  </label>
                  <div className="field__control">
                    <MailIcon className="field__icon" />
                    <input
                      id="email"
                      name="email"
                      type="email"
                      placeholder="you@example.com"
                      value={email}
                      onChange={handleEmailChange}
                      autoComplete="email"
                      disabled={isLoading}
                      required
                    />
                  </div>
                </div>

                <button
                  type="submit"
                  className="submit-button"
                  disabled={isLoading}
                >
                  {isLoading ? (
                    <span className="submit-button__content">
                      <span className="spinner" />
                      Sending...
                    </span>
                  ) : (
                    'Send Reset Link'
                  )}
                </button>
              </form>

              <p className="back-to-login">
                Remember your password?{' '}
                <Link
                  to={ROUTES.LOGIN}
                  className="link-button link-button--inline"
                >
                  Sign In
                </Link>
              </p>
            </>
          ) : (
            <div className="success-container">
              <div className="success-icon">
                <svg
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                >
                  <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14" />
                  <polyline points="22 4 12 14.01 9 11.01" />
                </svg>
              </div>
              <h2 className="success-heading">Check Your Email</h2>
              <p className="success-message">
                We've sent a password reset link to <strong>{email}</strong>
              </p>
              <p className="success-subtext">
                If you don't see the email, check your spam folder.
              </p>
              <button
                type="button"
                className="submit-button"
                onClick={() => navigate(ROUTES.LOGIN)}
              >
                Back to Login
              </button>
            </div>
          )}
        </div>
      </div>

      <p className="forgot-footer">
        © {new Date().getFullYear()} AYA Academy. All rights reserved.
      </p>
    </div>
  );
}
