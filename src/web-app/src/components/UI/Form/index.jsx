import { Form as FormRouter } from 'react-router-dom';

import classes from './Form.module.css';

import Button from '../Button';

const Label = (props) => (
  <label className={classes.form__label} {...props}>
    {props.children}
  </label>
);

const Input = (props) => <input className={classes.form__input} {...props} />;

const Group = ({ children }) => (
  <div className={classes.form__group}>{children}</div>
);

const Submit = (props) => (
  <Button btnType='full' {...props}>
    {props.children}
  </Button>
);

const Form = (props) => <FormRouter {...props}>{props.children}</FormRouter>;

Form.Group = Group;
Form.Label = Label;
Form.Input = Input;
Form.Submit = Submit;

export default Form;
