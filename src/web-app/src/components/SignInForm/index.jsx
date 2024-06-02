import classes from './SignInForm.module.css';

import Form from '../UI/Form';
import SecondaryHeading from '../UI/Headings/SecondaryHeading';

const SignInForm = ({ onSubmit }) => {
  const handleSubmit = (e) => {
    e.preventDefault();

    const { email, password } = e.target;

    onSubmit(email.value, password.value);
  };

  return (
    <div className={classes['signin-form']}>
      <SecondaryHeading>Sign into your user account</SecondaryHeading>

      <Form onSubmit={handleSubmit}>
        <Form.Group>
          <Form.Label htmlFor='email'>Email address</Form.Label>
          <Form.Input
            id='email'
            name='email'
            type='email'
            placeholder='test@example.com'
            required={true}
          />
        </Form.Group>

        <Form.Group>
          <Form.Label htmlFor='password'>Password</Form.Label>
          <Form.Input
            id='password'
            name='password'
            type='password'
            placeholder='••••••••'
            required={true}
          />
        </Form.Group>

        <Form.Group>
          <Form.Submit>Sign in</Form.Submit>
        </Form.Group>
      </Form>
    </div>
  );
};

export default SignInForm;
