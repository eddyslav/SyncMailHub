import { Outlet } from 'react-router-dom';

import Header from '../Header';
import Footer from '../Footer';

import classes from './Layout.module.css';

const Layout = () => (
  <>
    <Header />

    <main className={classes.main}>
      <Outlet />
    </main>

    {/* <Footer /> */}
  </>
);

export default Layout;
