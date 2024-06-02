import classes from './PrimaryHeading.module.css';

const PrimaryHeading = ({ children }) => (
  <h1 className={classes['heading-primary']}>{children}</h1>
);

export default PrimaryHeading;
