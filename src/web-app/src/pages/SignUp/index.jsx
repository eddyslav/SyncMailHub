import { useNavigate } from 'react-router-dom';

import { useSignUp } from '../../lib/react-query/queries';

import Loader from '../../components/UI/Loader';

import SignUpForm from '../../components/SignUpForm';

const SignUpPage = () => {
  const { mutateAsync, status } = useSignUp();
  const navigate = useNavigate();

  const handleSignUp = async (emailAddress, firstName, lastName, password) => {
    await mutateAsync({
      emailAddress,
      firstName,
      lastName,
      password,
    });

    navigate('/');
  };

  if (status === 'pending') {
    return <Loader />;
  }

  return <SignUpForm onSubmit={handleSignUp} />;
};

export default SignUpPage;
