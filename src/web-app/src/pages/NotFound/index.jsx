import ErrorBoundary from '../../components/UI/ErrorBoundary';

const NotFoundPage = () => (
  <ErrorBoundary error={`Target page route was not found!`} />
);

export default NotFoundPage;
