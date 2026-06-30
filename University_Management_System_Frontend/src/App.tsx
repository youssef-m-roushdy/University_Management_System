// App.tsx

import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import { AuthProvider } from './contexts/AuthContext';
import { ThemeProvider } from './contexts/ThemeContext';
import ProtectedRoute from './components/common/ProtectedRoute';
import Layout from './components/layout/Layout';
import RoleDashboardRedirect from './components/common/RoleDashboardRedirect';

import { ROUTES } from './constants';
import { publicRoutes, protectedRoutes } from './routes';

import './styles/globals.css';

const App: React.FC = () => {
  return (
    <ThemeProvider>
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
            {/* Public routes */}
            {publicRoutes.map(({ path, element }) => (
              <Route key={path} path={path} element={element} />
            ))}

            {/* Protected app shell */}
            <Route
              path="/"
              element={
                <ProtectedRoute>
                  <Layout />
                </ProtectedRoute>
              }
            >
              {/* Visiting "/" while authenticated resolves to the
                  user's own dashboard based on role */}
              <Route index element={<RoleDashboardRedirect />} />

              {protectedRoutes.map(({ path, element, roles }) => (
                <Route
                  key={path}
                  path={path}
                  element={
                    roles && roles.length > 0 ? (
                      <ProtectedRoute roles={roles}>{element}</ProtectedRoute>
                    ) : (
                      element // ← CompositeDashboard hits THIS branch (no roles = no extra guard)
                    )
                  }
                />
              ))}

              {/* Unmatched paths inside the authenticated shell */}
              <Route path="*" element={<RoleDashboardRedirect />} />
            </Route>

            {/* Unmatched paths outside the shell */}
            <Route path="*" element={<Navigate to={ROUTES.LOGIN} replace />} />
          </Routes>
        </AuthProvider>
      </BrowserRouter>
    </ThemeProvider>
  );
};

export default App;
