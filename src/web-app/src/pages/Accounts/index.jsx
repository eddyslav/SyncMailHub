import { useGetServiceAccounts } from '../../lib/react-query/queries';

import Loader from '../../components/UI/Loader';

import AccountsList from '../../components/AccountsList';

const AccountsPage = () => {
  const { data: accounts, isLoading } = useGetServiceAccounts();

  return (isLoading && <Loader />) || <AccountsList accounts={accounts} />;
};

export default AccountsPage;
