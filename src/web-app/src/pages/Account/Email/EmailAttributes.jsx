import classes from './EmailAttributes.module.css';

const EmailAttributeHeader = ({ children }) => (
  <span className={classes['email-attribute__header']}>{children}</span>
);

const EmailAttributeValue = ({ children }) => (
  <span className={classes['email-attribute__value']}>{children}</span>
);

const EmailAttribute = ({ children }) => (
  <li className={classes['email-attribute']}>{children}</li>
);

EmailAttribute.Header = EmailAttributeHeader;
EmailAttribute.Value = EmailAttributeValue;

const EmailAttributes = ({ children }) => (
  <div className={classes['email-attributes']}>
    <ul className={classes['email-attributes__list']}>{children}</ul>
  </div>
);

EmailAttributes.Attribute = EmailAttribute;

export default EmailAttributes;
