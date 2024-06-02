import { useState, useEffect } from 'react';
import { useInView } from 'react-intersection-observer';

import { useContextMenu, Menu, Item } from 'react-contexify';

import { toast } from 'react-toastify';

import classes from './EmailsConversations.module.css';

import {
  useDeleteEmailsConversation,
  useGetEmailsConversations,
} from '../../lib/react-query/queries';

import Loader from '../../components/UI/Loader';
import Button from '../../components/UI/Button';

const MENU_ID = 'EMAILS_CONVERSATIONS_MENU';

const EmailsConversations = ({ folderId, onConversationSelected }) => {
  const [selectedConversationId, setSelectedConversationId] = useState(null);
  const { ref, inView } = useInView();

  const { data, isLoading, refetch, fetchNextPage, isFetchingNextPage } =
    useGetEmailsConversations(folderId);

  const { mutateAsync: deleteConversation } = useDeleteEmailsConversation();

  const { show } = useContextMenu({
    id: MENU_ID,
  });

  useEffect(() => {
    setSelectedConversationId(null);
  }, [folderId]);

  useEffect(() => {
    if (inView) {
      fetchNextPage();
    }
  }, [fetchNextPage, inView]);

  if (folderId == null) {
    return null;
  }

  const handleConversationClick = (conversationId) => {
    if (selectedConversationId === conversationId) {
      return;
    }

    setSelectedConversationId(conversationId);
    onConversationSelected(conversationId);
  };

  const handleDeleteConversation = async ({ props: { event } }, force) => {
    const { target } = event;

    const listItem = target.closest('li');
    if (listItem == null) {
      console.warn('List item was not clicked');
      return;
    }

    await deleteConversation(
      { id: listItem.dataset.id, force },
      {
        onSuccess: (_, { force }, _1) => {
          toast.success(
            `You have successfully ${
              (force && 'deleted') || 'trashed'
            } conversation!`
          );
        },
      }
    );

    setSelectedConversationId(null);
    onConversationSelected(null);
    refetch();
  };

  if (isLoading) {
    return <Loader />;
  }

  const isAnyAvailable = data.pages.length !== 0;

  return (
    <div className={classes['conversations-page']}>
      <Menu id={MENU_ID}>
        <Item onClick={(args) => handleDeleteConversation(args, true)}>
          Delete
        </Item>
        <Item onClick={(args) => handleDeleteConversation(args, false)}>
          Trash
        </Item>
      </Menu>

      {isAnyAvailable && (
        <div className={classes['conversations__actions']}>
          <Button className={classes['btn-reload']} onClick={() => refetch()}>
            Reload!
          </Button>
        </div>
      )}

      <div className={classes['conversations-content']}>
        {!isAnyAvailable && (
          <p className={classes['conversations--empty']}>
            No data in this conversations folder. Try select another one...
          </p>
        )}

        {isAnyAvailable && (
          <>
            <ul
              className={classes['conversations-list']}
              key={folderId}
              onContextMenu={(e) => show({ event: e, props: { event: e } })}
            >
              {data.pages.map(
                ({
                  id,
                  subject,
                  from: { name: fromName, emailAddress: fromEmailAddress },
                }) => (
                  <li
                    key={id}
                    className={`${classes.conversation} ${
                      id === selectedConversationId &&
                      classes['conversation--active']
                    }`}
                    data-id={id}
                    onClick={() => handleConversationClick(id)}
                  >
                    <span className={classes.conversation__from}>
                      {fromName || fromEmailAddress}
                    </span>
                    {subject || '<NO SUBJECT>'}
                  </li>
                )
              )}
            </ul>
            <div ref={ref}>{isFetchingNextPage && <Loader />}</div>
          </>
        )}
      </div>
    </div>
  );
};

export default EmailsConversations;
