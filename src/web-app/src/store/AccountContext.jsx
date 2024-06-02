import { createContext, useContext, useEffect, useState } from 'react';
import { useParams, useLocation, useNavigate } from 'react-router-dom';

import { getServiceAccountById } from '../lib/api';

import AuthContext from './AuthContext';

const AccountContext = createContext({
  selectedAccount: null,
  isLoading: false,
});

export const AccountContextProvider = ({ children }) => {
  const { token } = useContext(AuthContext);

  const { id } = useParams();
  const { state } = useLocation();
  const navigate = useNavigate();

  const [selectedAccount, setSelectedAccount] = useState(state.selectedAccount);

  const [isLoading, setIsLoading] = useState(selectedAccount == null);

  const fetchAccount = async () => {
    try {
      const account = await getServiceAccountById(id, token);

      setSelectedAccount(account);
    } catch (error) {
      console.error('Failed to fetch account data:', error);
      navigate('/');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    if (!selectedAccount) {
      fetchAccount();
    }
  }, []);

  const context = {
    selectedAccount,
    isLoading,
  };

  return (
    <AccountContext.Provider value={context}>
      {children}
    </AccountContext.Provider>
  );
};

export default AccountContext;
