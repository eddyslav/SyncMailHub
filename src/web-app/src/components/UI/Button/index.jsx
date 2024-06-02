import classes from './Button.module.css';

const Button = ({ btnType = 'full', children, className, ...props }) => (
  <button
    className={`${classes.btn} ${classes[`btn--${btnType}`]} ${
      className || ''
    }`}
    {...props}
  >
    {children}
  </button>
);

export default Button;
