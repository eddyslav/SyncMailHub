import classes from './ErrorBoundary.module.css';

import SecondaryHeading from '../Headings/SecondaryHeading';

const ErrorBoundary = ({ error }) => (
  <div className={classes.error}>
    <div className={classes.error__title}>
      <SecondaryHeading isError={true}>
        Uh oh! Something went wrong!
      </SecondaryHeading>
    </div>

    <div className={classes.error__msg}>
      {error?.response?.data && (
        <>
          {error.response.data.status}
          <br />
          {error.response.data.messages.map((x) => (
            <p>{x}</p>
          ))}
        </>
      )}
      {Array.isArray(error?.message)
        ? error.message.map((x) => <p>{x}</p>)
        : error || 'Unknown error occured in the system'}
    </div>
  </div>
);

export default ErrorBoundary;
