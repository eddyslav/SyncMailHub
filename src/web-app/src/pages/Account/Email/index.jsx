import { useContextMenu, Menu, Item } from 'react-contexify';

import { toast } from 'react-toastify';

import classes from './Emails.module.css';

import { useDeleteEmail, useGetEmails } from '../../../lib/react-query/queries';

import Loader from '../../../components/UI/Loader';
import Button from '../../../components/UI/Button';

import EmailAttributes from './EmailAttributes';

const MENU_ID = 'EMAILS_CONVERSATIONS_MENU';

const formatRecipient = (name, emailAddress) =>
  name == null ? emailAddress : `${name} <${emailAddress}>`;

const EmailRecipients = ({ header, recipients }) => {
  if (!recipients || recipients.length === 0) {
    return null;
  }

  return (
    <EmailAttributes.Attribute>
      <EmailAttributes.Attribute.Header>
        {header}
      </EmailAttributes.Attribute.Header>
      <EmailAttributes.Attribute.Value>
        {recipients
          .map(({ name, emailAddress }) => formatRecipient(name, emailAddress))
          .join(' ')}
      </EmailAttributes.Attribute.Value>
    </EmailAttributes.Attribute>
  );
};

const dateFormatter = new Intl.DateTimeFormat(undefined, {
  dateStyle: 'medium',
  timeStyle: 'medium',
});

const Emails = ({ conversationId }) => {
  const { data, isLoading } = useGetEmails(conversationId);

  const { show } = useContextMenu({
    id: MENU_ID,
  });

  const { mutateAsync: deleteEmail } = useDeleteEmail();

  if (!conversationId) {
    return null;
  }

  if (isLoading) {
    return <Loader />;
  }

  const handleDelete = async ({ props: { event } }, force) => {
    const { target } = event;

    const listItem = target.closest('li');
    if (listItem == null) {
      console.warn('List item was not clicked');
      return;
    }

    await deleteEmail(
      { id: listItem.dataset.id, force },
      {
        onSuccess: (_, { force }, _1) => {
          toast.success(
            `You have successfully ${(force && 'deleted') || 'trashed'} email!`
          );
        },
      }
    );
  };

  return (
    <div className={classes['emails-page']}>
      <Menu id={MENU_ID}>
        <Item onClick={(args) => handleDelete(args, true)}>Delete</Item>
        <Item onClick={(args) => handleDelete(args, false)}>Trash</Item>
      </Menu>

      <div className={classes['emails-actions']}>
        <Button className={classes['btn-reply']}>Reply</Button>
      </div>

      <ul className={classes['emails-list']}>
        {data.map((email) => (
          <li key={email.id} data-id={email.id} className={classes.email}>
            <div className={classes.email__header}>
              <EmailAttributes>
                <EmailAttributes.Attribute>
                  <EmailAttributes.Attribute.Header>
                    {'Date: '}
                  </EmailAttributes.Attribute.Header>

                  <EmailAttributes.Attribute.Value>
                    {dateFormatter.format(new Date(email.date))}
                  </EmailAttributes.Attribute.Value>
                </EmailAttributes.Attribute>

                <EmailAttributes.Attribute>
                  <EmailAttributes.Attribute.Header>
                    From:
                  </EmailAttributes.Attribute.Header>

                  <EmailAttributes.Attribute.Value>
                    {formatRecipient(email.from.name, email.from.emailAddress)}
                  </EmailAttributes.Attribute.Value>
                </EmailAttributes.Attribute>

                <EmailRecipients header={'To:'} recipients={email.to} />
                <EmailRecipients header={'Cc:'} recipients={email.cc} />
                <EmailRecipients header={'Bcc:'} recipients={email.bcc} />
              </EmailAttributes>

              <div className={classes.email__actions}>
                <Button
                  className={classes['btn-delete']}
                  onClick={(e) => show({ event: e, props: { event: e } })}
                >
                  Delete!
                </Button>
              </div>
            </div>
            <div className={classes.email__body}>
              <iframe
                className={classes.email__body__frame}
                srcDoc={email.htmlBody}
              />
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Emails;
