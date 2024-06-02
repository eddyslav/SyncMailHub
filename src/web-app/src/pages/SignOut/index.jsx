import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';

import AuthContext from '../../store/AuthContext';

const SignOutPage = () => {
  const { signOut } = useContext(AuthContext);
  const navigate = useNavigate();

  signOut();

  return navigate('/sign-in');
};

export default SignOutPage;
