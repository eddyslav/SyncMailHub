import {
  MutationCache,
  QueryCache,
  QueryClient,
  QueryClientProvider,
} from '@tanstack/react-query';

import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

import { toast } from 'react-toastify';

import { ApiValidationError } from '../api/errors';

const errorHandler = (error) => {
  console.error(error);

  if (!error.isOperational) {
    throw error;
  }

  toast.error(error.message);

  if (error instanceof ApiValidationError) {
    error.errors.forEach(({ message }) => toast.error(message));
  }
};

const queryClient = new QueryClient({
  queryCache: new QueryCache({
    onError(error, query) {
      errorHandler(error);
    },
  }),
  mutationCache: new MutationCache({
    onError(error, vars, ctx, mutation) {
      errorHandler(error);
    },
  }),
});

const QueryProvider = ({ children }) => (
  <QueryClientProvider client={queryClient}>
    {children}
    {/* <ReactQueryDevtools initialIsOpen={false} /> */}
  </QueryClientProvider>
);

export default QueryProvider;
