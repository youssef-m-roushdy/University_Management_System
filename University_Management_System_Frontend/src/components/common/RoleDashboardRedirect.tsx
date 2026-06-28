// components/common/RoleDashboardRedirect.tsx

import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { USER_ROLES } from '../../constants';

const RoleDashboardRedirect: React.FC = () => {
  const { user, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!isAuthenticated || !user) {
      navigate('/login');
      return;
    }

    const roles = user.roles || [];

    if (roles.includes(USER_ROLES.ADMIN)) {
      navigate('/dashboard/admin');
    } else if (roles.includes(USER_ROLES.STUDENT)) {
      navigate('/dashboard/student');
    } else if (roles.includes(USER_ROLES.INSTRUCTOR)) {
      navigate('/dashboard/instructor');
    } else if (roles.includes(USER_ROLES.ASSISTANT)) {
      navigate('/dashboard/assistant');
    } else {
      navigate('/dashboard');
    }
  }, [user, isAuthenticated, navigate]);

  return (
    <div className="loading-container">
      <div className="loading-spinner"></div>
      <p>Redirecting to your dashboard...</p>
    </div>
  );
};

export default RoleDashboardRedirect;
