import { useContext } from 'react';
import { Link } from 'react-router-dom';

import AuthContext from '../../store/AuthContext';

import classes from './Header.module.css';
import Loader from '../UI/Loader';

const Header = () => {
  const { isLoading, isAuthenticated, userInfo } = useContext(AuthContext);

  return (
    <header className={classes.header}>
      <div className={classes.header__logo}>
        <img src='/logo.png' alt='SyncMailHub logo' />
      </div>

      <nav className={classes['main-nav']}>
        <ul className={classes['main-nav-list']}>
          {isLoading && <Loader />}
          {isAuthenticated && (
            <>
              <li>
                <Link
                  className={`${classes['main-nav-link']} ${classes['main-nav-link--logout']}`}
                  to='/sign-out'
                >
                  Sign out
                </Link>
              </li>

              <li>
                <Link className={classes['main-nav-link']} to='/'>
                  <span>Welcome back, {userInfo.firstName}!</span>
                </Link>
              </li>
            </>
          )}
          {!isLoading && !isAuthenticated && (
            <>
              <li>
                <Link className={classes['main-nav-link']} to='/sign-in'>
                  Sign in
                </Link>
              </li>
              <li>
                <Link
                  className={`${classes['main-nav-link']} ${classes['nav-cta']}`}
                  to='/sign-up'
                >
                  Sign up
                </Link>
              </li>
            </>
          )}
        </ul>
      </nav>
    </header>
  );
};

export default Header;
