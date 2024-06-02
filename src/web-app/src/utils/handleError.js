import { toast } from 'react-toastify';
import { ApiError, ApiValidationError } from '../lib/api/errors';

const handleError = (error) => {
  console.error(error);

  if (error instanceof ApiError) {
    toast.error(error.message);
  }

  if (error instanceof ApiValidationError) {
    error.errors.forEach(
      (error) => console.error(error) || toast.error(error.message)
    );

    return;
  }

  throw error;
};

export default handleError;
