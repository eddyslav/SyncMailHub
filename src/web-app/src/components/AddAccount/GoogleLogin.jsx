import { GoogleOAuthProvider, useGoogleLogin } from '@react-oauth/google';
import Button from '../UI/Button';

const CLIENT_ID = import.meta.env.VITE_GOOGLE_CLIENT_ID;
const REDIRECT_URL = import.meta.env.VITE_GOOGLE_REDIRECT_URL;
const SCOPE = import.meta.env.VITE_GOOGLE_SCOPE;

const GoogleLoginImpl = ({ onCode }) => {
  const login = useGoogleLogin({
    onSuccess: ({ code }) => onCode(code),
    onError: alert,
    flow: 'auth-code',
    redirect_uri: REDIRECT_URL,
    ux_mode: 'redirect',
    scope: SCOPE,
  });

  return <Button onClick={() => login()}>Add google account!</Button>;
};

const GoogleLogin = ({ onCode }) => (
  <GoogleOAuthProvider clientId={CLIENT_ID}>
    <GoogleLoginImpl onCode={onCode} />
  </GoogleOAuthProvider>
);

export default GoogleLogin;
