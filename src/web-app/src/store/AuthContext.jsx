import { createContext, useEffect, useState } from 'react';

import { getCurrentUser } from '../lib/api';

const AuthContext = createContext({
  isLoading: false,
  isAuthenticated: false,
  token: null,
  expiresAt: null,
  userInfo: null,
  signIn: (token, user) => {},
  signOut: () => {},
});

const TOKEN_LOCALSTORAGE_KEYNAME = 'token';
const EXPIRESAT_LOCALSTORAGE_KEYNAME = 'expiresAt';

export const AuthContextProvider = ({ children }) => {
  const [currentUserToken, setCurrentUserToken] = useState(
    localStorage.getItem(TOKEN_LOCALSTORAGE_KEYNAME)
  );

  const [currentUserTokenExpiresAt, setCurrentUserTokenExpiresAt] = useState(
    new Date(
      parseInt(localStorage.getItem(EXPIRESAT_LOCALSTORAGE_KEYNAME) || 0)
    )
  );

  const [currentUserInfo, setCurrentUserInfo] = useState(null);

  const [isLoading, setIsLoading] = useState(
    currentUserToken && currentUserTokenExpiresAt > Date.now()
  );
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    if (!currentUserToken || !currentUserTokenExpiresAt) {
      localStorage.removeItem(TOKEN_LOCALSTORAGE_KEYNAME);
      localStorage.removeItem(EXPIRESAT_LOCALSTORAGE_KEYNAME);
      return;
    }

    localStorage.setItem(TOKEN_LOCALSTORAGE_KEYNAME, currentUserToken);

    localStorage.setItem(
      EXPIRESAT_LOCALSTORAGE_KEYNAME,
      currentUserTokenExpiresAt.getTime().toString()
    );
  }, [currentUserToken, currentUserTokenExpiresAt]);

  const signIn = ({ token, expiresAt }, user) => {
    setIsAuthenticated(true);
    setCurrentUserToken(token);
    setCurrentUserTokenExpiresAt(expiresAt);
    setCurrentUserInfo(user);
  };

  const signOut = () => {
    setIsAuthenticated(false);
    setCurrentUserToken(null);
    setCurrentUserTokenExpiresAt(null);
    setCurrentUserInfo(null);

    localStorage.clear();
  };

  const fetchUser = async () => {
    try {
      const currentUser = await getCurrentUser(currentUserToken);

      setCurrentUserInfo(currentUser);
      setIsAuthenticated(true);
    } catch (error) {
      console.error('Failed to fetch user data:', error);
      signOut();
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    if (currentUserToken && currentUserTokenExpiresAt > Date.now()) {
      fetchUser();
    }
  }, []);

  const context = {
    isLoading,
    isAuthenticated,
    userInfo: currentUserInfo,
    token: currentUserToken,
    expiresAt: currentUserTokenExpiresAt,
    signIn,
    signOut,
  };

  return (
    <AuthContext.Provider value={context}>{children}</AuthContext.Provider>
  );
};

export default AuthContext;
