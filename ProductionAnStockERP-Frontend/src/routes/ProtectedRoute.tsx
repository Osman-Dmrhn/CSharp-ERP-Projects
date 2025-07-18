import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';


const useAuth = () => {
  
  const token = localStorage.getItem('token');

  const isAuthenticated = !!token; 

  return { isAuthenticated };
};

interface ProtectedRouteProps {
  children: React.ReactElement;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  const location = useLocation();
  if (!isAuthenticated) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }
  return children;
};

export default ProtectedRoute;