import { useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import { useAddGoogleAccount } from '../../lib/react-query/queries';

import Loader from '../../components/UI/Loader';

const GoogleCallback = () => {
  const { search } = useLocation();
  const navigate = useNavigate();
  const { mutateAsync, status, error } = useAddGoogleAccount();

  const queryParams = new URLSearchParams(search);
  const code = queryParams.get('code');

  useEffect(() => {
    if (!code) {
      alert('Failed authentication');
      navigate('/add-account');
    } else {
      mutateAsync({ code });
    }
  }, [code, mutateAsync, navigate]);

  useEffect(() => {
    if (status === 'success') {
      navigate('/');
    } else if (status === 'error') {
      navigate('/add-account');
    }
  }, [status, error, navigate]);

  return <Loader />;
};

export default GoogleCallback;
