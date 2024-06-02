import classes from './SignUpForm.module.css';

import SecondaryHeading from '../UI/Headings/SecondaryHeading';

import Form from '../UI/Form';

const SignUp = ({ onSubmit }) => {
  const handleSubmit = (e) => {
    e.preventDefault();

    const { email, firstName, lastName, password } = e.target;
    onSubmit(email.value, firstName.value, lastName.value, password.value);
  };

  return (
    <div className={classes['signup-form']}>
      <SecondaryHeading>Create your user account!</SecondaryHeading>

      <Form onSubmit={handleSubmit}>
        <Form.Group>
          <Form.Label htmlFor='email'>Your primary email address</Form.Label>
          <Form.Input
            id='email'
            name='email'
            type='email'
            placeholder='john@example.com'
            required={true}
          />
        </Form.Group>

        <Form.Group>
          <Form.Label htmlFor='firstName'>Your first name</Form.Label>
          <Form.Input
            id='firstName'
            name='firstName'
            type='text'
            placeholder='John'
            required={true}
          />
        </Form.Group>

        <Form.Group>
          <Form.Label htmlFor='lastName'>Your last name</Form.Label>
          <Form.Input
            id='lastName'
            name='lastName'
            type='text'
            placeholder='John'
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
            minLength={8}
          />
        </Form.Group>

        <Form.Group>
          <Form.Submit>Sign up</Form.Submit>
        </Form.Group>
      </Form>
    </div>
  );
};

export default SignUp;
