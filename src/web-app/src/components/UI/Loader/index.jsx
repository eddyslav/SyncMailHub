import { ColorRing } from 'react-loader-spinner';

import classes from './Loader.module.css';

const Loader = () => (
  <div className={classes.loader}>
    <ColorRing
      visible={true}
      height='55'
      width='55'
      ariaLabel='blocks-loading'
      wrapperStyle={{ alignSelf: 'center' }}
      wrapperClass='blocks-wrapper'
      colors={['#40c057', '#40c057', '#40c057', '#40c057', '#40c057']}
    />
  </div>
);

export default Loader;
