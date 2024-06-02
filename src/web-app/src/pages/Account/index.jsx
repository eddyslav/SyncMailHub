import { useEffect, useState } from 'react';

import Modal from 'react-modal';

import classes from './Account.module.css';

import Button from '../../components/UI/Button';

import EmailsConversations from './EmailsConversations';
import EmailsConversationFolders from './EmailsConversationFolders';
import Emails from './Email';

const AccountPage = () => {
  const [selectedFolderId, setSelectedFolderId] = useState(null);
  const [selectedConversationId, setSelectedConversationId] = useState(null);

  const handleSelectFolder = (folderId) => {
    setSelectedFolderId(folderId);
    setSelectedConversationId(null);
  };
  const handleSelectConversation = (conversationId) =>
    setSelectedConversationId(conversationId);

  useEffect(() => {
    if (selectedConversationId) {
      window.scrollTo(0, 0);
    }
  }, [selectedConversationId]);

  return (
    <div className={classes['account-page']}>
      <div className={classes['account-actions']}>
        <Button className={classes['account-actions__new']}>
          Compose new email!
        </Button>
      </div>

      <div className={classes['account-details']}>
        <div className={classes['folder-tree']}>
          <EmailsConversationFolders onFolderSelected={handleSelectFolder} />
        </div>

        <div className={classes.conversations}>
          <EmailsConversations
            folderId={selectedFolderId}
            onConversationSelected={handleSelectConversation}
          />
        </div>

        <div className={classes.emails}>
          <Emails conversationId={selectedConversationId} />
        </div>
      </div>
    </div>
  );
};

export default AccountPage;
