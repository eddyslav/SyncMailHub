import { useContext } from 'react';
import { useLocation, Outlet, Navigate } from 'react-router-dom';

import AuthContext from '../store/AuthContext';

import Loader from '../components/UI/Loader';

const RequireAuth = () => {
  const { isAuthenticated, isLoading } = useContext(AuthContext);
  const location = useLocation();

  if (isLoading) {
    return <Loader />;
  }

  return (
    (isAuthenticated && <Outlet />) || (
      <Navigate to='/sign-in' state={{ from: location }} replace />
    )
  );
};

export default RequireAuth;
