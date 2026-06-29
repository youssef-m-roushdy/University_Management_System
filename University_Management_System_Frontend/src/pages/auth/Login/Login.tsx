// src/pages/auth/Login/Login.tsx

import React, { useState, FormEvent, ChangeEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../../contexts/AuthContext';
import { useTheme } from '../../../contexts/ThemeContext';
import { ROUTES, STATUS } from '../../../constants';
import {
  MailIcon,
  LockIcon,
  EyeIcon,
  EyeOffIcon,
  ShieldIcon,
  ChartIcon,
  ClockIcon,
  GoogleIcon,
  MicrosoftIcon,
  SunIcon,
  MoonIcon,
  AlertIcon,
} from '../../../components/icons/Icons';
import UniversityIllustration from '../../../components/illustration/UniversityIllustration';
import './Login.css';

const features = [
  {
    icon: ShieldIcon,
    title: 'Secure & Reliable',
    description: 'Your data is safe with enterprise-grade security.',
  },
  {
    icon: ChartIcon,
    title: 'Smart Management',
    description: 'Manage students, faculty, courses and more in one place.',
  },
  {
    icon: ClockIcon,
    title: 'Save Time',
    description: 'Automate tasks and focus on what matters most.',
  },
];

export default function Login(): React.ReactElement {
  const { mode, toggleTheme } = useTheme();
  const { login, status, error, clearError } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [rememberMe, setRememberMe] = useState<boolean>(false);

  const isLoading = status === STATUS.LOADING;

  const handleSubmit = async (e: FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();
    clearError();
    try {
      await login(email, password);
      navigate(ROUTES.DASHBOARD);
    } catch (_) {
      // Error is surfaced via the `error` value from AuthContext
    }
  };

  const handleEmailChange = (e: ChangeEvent<HTMLInputElement>): void => {
    clearError();
    setEmail(e.target.value);
  };

  const handlePasswordChange = (e: ChangeEvent<HTMLInputElement>): void => {
    clearError();
    setPassword(e.target.value);
  };

  const handleRememberMeChange = (e: ChangeEvent<HTMLInputElement>): void => {
    setRememberMe(e.target.checked);
  };

  const togglePasswordVisibility = (): void => {
    setShowPassword(prev => !prev);
  };

  return (
    <div className="login-page">
      <div className="login-card">
        <button
          type="button"
          className="theme-toggle"
          onClick={toggleTheme}
          disabled={isLoading}
          aria-label={
            mode === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'
          }
          title={
            mode === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'
          }
        >
          {mode === 'dark' ? <SunIcon /> : <MoonIcon />}
        </button>

        {/* Left: brand panel */}
        <div className="login-panel login-panel--brand">
          <div className="brand">
            <div>
              <p className="brand__name">Akhbar Alyoum Academy</p>
              <p className="brand__tagline">University Information System</p>
            </div>
          </div>

          <div className="brand-copy">
            <h1 className="brand-heading">
              Welcome <span className="brand-heading__accent">Back!</span>
            </h1>
            <p className="brand-subtext">
              Sign in to continue managing your university with ease and
              efficiency.
            </p>
          </div>

          <div className="illustration">
            <UniversityIllustration />
          </div>

          <ul className="feature-list">
            {features.map(({ icon: Icon, title, description }) => (
              <li className="feature-item" key={title}>
                <span className="feature-item__icon">
                  <Icon />
                </span>
                <div>
                  <p className="feature-item__title">{title}</p>
                  <p className="feature-item__description">{description}</p>
                </div>
              </li>
            ))}
          </ul>
        </div>

        {/* Right: form panel */}
        <div className="login-panel login-panel--form">
          <div className="form-header">
            <h2 className="form-heading">Sign in to your account</h2>
            <p className="form-subtext">
              Enter your credentials to access your dashboard.
            </p>
          </div>

          {error && (
            <div className="error-alert" role="alert">
              <AlertIcon className="error-alert__icon" />
              <span>{error}</span>
            </div>
          )}

          <form className="login-form" onSubmit={handleSubmit}>
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

            <div className="field">
              <label htmlFor="password" className="field__label">
                Password
              </label>
              <div className="field__control">
                <LockIcon className="field__icon" />
                <input
                  id="password"
                  name="password"
                  type={showPassword ? 'text' : 'password'}
                  placeholder="Enter your password"
                  value={password}
                  onChange={handlePasswordChange}
                  autoComplete="current-password"
                  disabled={isLoading}
                  required
                />
                <button
                  type="button"
                  className="field__action"
                  onClick={togglePasswordVisibility}
                  disabled={isLoading}
                  aria-label={showPassword ? 'Hide password' : 'Show password'}
                >
                  {showPassword ? <EyeOffIcon /> : <EyeIcon />}
                </button>
              </div>
            </div>

            <div className="form-row">
              <label className="checkbox">
                <input
                  type="checkbox"
                  checked={rememberMe}
                  onChange={handleRememberMeChange}
                  disabled={isLoading}
                />
                <span>Remember me</span>
              </label>
              <button
                type="button"
                className="link-button"
                onClick={() => navigate(ROUTES.FORGOT_PASSWORD)}
                disabled={isLoading}
              >
                Forgot Password?
              </button>
            </div>

            <button
              type="submit"
              className="submit-button"
              disabled={isLoading}
            >
              {isLoading ? (
                <span className="submit-button__content">
                  <span className="spinner" />
                  Signing in...
                </span>
              ) : (
                'Sign In'
              )}
            </button>
          </form>

          <div className="divider">
            <span>or continue with</span>
          </div>

          <div className="oauth-row">
            <button
              type="button"
              className="oauth-button"
              disabled
              title="Coming soon"
            >
              <GoogleIcon className="oauth-button__icon" />
              Google
            </button>
            <button
              type="button"
              className="oauth-button"
              disabled
              title="Coming soon"
            >
              <MicrosoftIcon className="oauth-button__icon" />
              Microsoft
            </button>
          </div>

          <p className="signup-hint">
            Don&apos;t have an account?{' '}
            <button
              type="button"
              className="link-button link-button--inline"
              disabled
              title="Contact your administrator to request access"
            >
              Contact Administrator
            </button>
          </p>
        </div>
      </div>

      <p className="login-footer">
        © {new Date().getFullYear()} AYA Academy. All rights reserved.
      </p>
    </div>
  );
}
