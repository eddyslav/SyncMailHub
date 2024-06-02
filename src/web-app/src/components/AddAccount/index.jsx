import classes from './AddAccount.module.css';

import GoogleLogin from './GoogleLogin';

const AddAccount = () => (
  <div className={classes['add-page']}>
    <GoogleLogin />
  </div>
);

export default AddAccount;
