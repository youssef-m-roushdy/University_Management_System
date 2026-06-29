// App.tsx

import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import { AuthProvider } from './contexts/AuthContext';
import { ThemeProvider } from './contexts/ThemeContext'; // <-- import
import ProtectedRoute from './components/common/ProtectedRoute';
import Layout from './components/layout/Layout';
import RoleDashboardRedirect from './components/common/RoleDashboardRedirect';

import { ROUTES } from './constants';
import { publicRoutes, protectedRoutes } from './routes';

import './styles/globals.css';

const App: React.FC = () => {
  return (
    <ThemeProvider>
      {' '}
      {/* ← Wrap everything */}
      <BrowserRouter>
        <AuthProvider>
          <ToastContainer
            position="top-right"
            autoClose={3000}
            hideProgressBar={false}
            newestOnTop
            closeOnClick
            pauseOnFocusLoss
            draggable
            pauseOnHover
            theme="colored"
          />

          <Routes>
            {/* Public Routes */}
            {publicRoutes.map(({ path, element }) => (
              <Route key={path} path={path} element={element} />
            ))}

            {/* Protected Routes */}
            <Route
              path={ROUTES.HOME}
              element={
                <ProtectedRoute>
                  <Layout />
                </ProtectedRoute>
              }
            >
              <Route
                index
                element={<Navigate to={ROUTES.DASHBOARD} replace />}
              />

              {/* Dashboard redirect by role */}
              <Route
                path={ROUTES.DASHBOARD}
                element={
                  <ProtectedRoute>
                    <RoleDashboardRedirect />
                  </ProtectedRoute>
                }
              />

              {/* Role-specific dashboards */}
              {protectedRoutes.map(({ path, element, roles }) => (
                <Route
                  key={path}
                  path={path}
                  element={
                    <ProtectedRoute roles={roles}>{element}</ProtectedRoute>
                  }
                />
              ))}
            </Route>

            {/* Catch-all */}
            <Route
              path="*"
              element={<Navigate to={ROUTES.DASHBOARD} replace />}
            />
          </Routes>
        </AuthProvider>
      </BrowserRouter>
    </ThemeProvider>
  );
};

export default App;
