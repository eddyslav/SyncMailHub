import classes from './SecondaryHeading.module.css';

const SecondaryHeading = ({ isError, children }) => (
  <h2
    className={`${classes['heading-secondary']} ${
      isError && classes['heading-secondary--error']
    } ma-bt-lg`}
  >
    {children}
  </h2>
);

export default SecondaryHeading;
