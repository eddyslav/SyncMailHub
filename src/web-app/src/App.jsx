import {
  createBrowserRouter,
  createRoutesFromElements,
  Outlet,
  Route,
  RouterProvider,
} from 'react-router-dom';

import { ToastContainer, Bounce } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import 'react-contexify/ReactContexify.css';

import { AuthContextProvider } from './store/AuthContext';
import { AccountContextProvider } from './store/AccountContext';

import QueryProvider from './lib/react-query/QueryProvider';

import Layout from './components/Layout';
import ErrorBoundary from './components/UI/ErrorBoundary';

import AccountsPage from './pages/Accounts';
import AccountPage from './pages/Account';
import CreateAccountPage from './pages/CreateAccount';
import GoogleCallback from './pages/GoogleCallback';
import SignOutPage from './pages/SignOut';
import SignInPage from './pages/SignIn';
import SignUpPage from './pages/SignUp';
import NotFoundPage from './pages/NotFound';

import RequireAuth from './utils/RequireAuth';

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route
      element={
        <AuthContextProvider>
          <QueryProvider>
            <Layout />
          </QueryProvider>
        </AuthContextProvider>
      }
      errorElement={<ErrorBoundary />}
    >
      <Route element={<RequireAuth />}>
        <Route path='/' element={<AccountsPage />} />
        <Route
          element={
            <AccountContextProvider>
              <Outlet />
            </AccountContextProvider>
          }
        >
          <Route path='accounts/:id' element={<AccountPage />} />
        </Route>

        <Route path='add-account' element={<CreateAccountPage />} />
        <Route path='google/callback' element={<GoogleCallback />} />
        <Route path='sign-out' element={<SignOutPage />} />
      </Route>

      <Route path='sign-in' element={<SignInPage />} />
      <Route path='sign-up' element={<SignUpPage />} />

      <Route path='*' element={<NotFoundPage />} />
    </Route>
  )
);

const App = () => {
  return (
    <>
      <ToastContainer
        position='top-right'
        style={{ fontSize: '1.4rem' }}
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={true}
        closeOnClick={true}
        pauseOnFocusLoss={false}
        pauseOnHover={true}
        theme='light'
        transition={Bounce}
      />

      <RouterProvider router={router} />
    </>
  );
};

export default App;
