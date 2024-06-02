import { Link } from 'react-router-dom';

import { IonIcon } from '@ionic/react';
import { personOutline, logoGoogle } from 'ionicons/icons';

import classes from './AccountsList.module.css';

const AccountIcon = ({ type }) => <IonIcon icon={logoGoogle} />;

const AccountsList = ({ accounts }) => (
  <div className={classes.accounts}>
    <div>
      <h1 className={classes['accounts--heading ']}>
        Please select an account to continue
      </h1>
    </div>

    <ul className={classes['accounts-list']}>
      {accounts.map(({ id, emailAddress, type }) => (
        <li className={classes['account-item']} key={id}>
          <AccountIcon type={type} />
          <Link
            className={classes['account-item-link']}
            to={`/accounts/${id}`}
            state={{ selectedAccount: { id, emailAddress, type } }}
          >
            {emailAddress}
          </Link>
        </li>
      ))}
      <li className={classes['account-item']}>
        <IonIcon icon={personOutline} />
        <Link className={classes['account-item-link']} to='add-account'>
          Add new account!
        </Link>
      </li>
    </ul>
  </div>
);

export default AccountsList;
