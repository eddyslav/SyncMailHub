import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useSignIn } from '../../lib/react-query/queries';

import Loader from '../../components/UI/Loader';

import SignInForm from '../../components/SignInForm';

const SignInPage = () => {
  const { mutateAsync, status } = useSignIn();
  const navigate = useNavigate();

  const handleSignIn = (emailAddress, password) =>
    mutateAsync({ emailAddress, password });

  if (status === 'success') {
    toast('You have successfully logged in!', {
      type: 'success',
    });

    return navigate('/');
  }

  if (status === 'pending') {
    return <Loader />;
  }

  return <SignInForm onSubmit={handleSignIn} />;
};

export default SignInPage;
