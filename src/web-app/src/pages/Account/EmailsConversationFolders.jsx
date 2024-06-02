import { useState } from 'react';

import { IonIcon } from '@ionic/react';
import { folderOutline, folderOpenOutline } from 'ionicons/icons';

import classes from './EmailsConversationFolders.module.css';

import { useGetEmailsConversationFolders } from '../../lib/react-query/queries';

import Loader from '../../components/UI/Loader';

const FolderNode = ({ folder, isSelected, onSelected }) => {
  return (
    <li
      className={`${classes.folder} ${
        isSelected && classes['folder--selected']
      }`}
      onClick={() => onSelected(folder.id)}
    >
      <span className={classes.folder__header}>
        <IonIcon icon={isSelected ? folderOpenOutline : folderOutline} />
        {folder.name}
      </span>

      {folder.children && (
        <ul className={classes['subFolder-list']}>
          {folder.children.map((subFolder) => (
            <FolderNode key={subFolder.id} folder={subFolder} />
          ))}
        </ul>
      )}
    </li>
  );
};

const FolderList = ({ folders, onFolderSelected }) => {
  const [selectedFolderId, setSelectedFolderId] = useState(null);

  const handleFolderClick = (folderId) => {
    if (selectedFolderId === folderId) {
      return;
    }

    setSelectedFolderId(folderId);
    onFolderSelected(folderId);
  };

  return (
    <ul className={classes['folder-list']}>
      {folders.map((folder) => (
        <FolderNode
          key={folder.id}
          folder={folder}
          isSelected={folder.id === selectedFolderId}
          onSelected={handleFolderClick}
        />
      ))}
    </ul>
  );
};

const EmailsConversationFolders = ({ onFolderSelected }) => {
  const { data: folders, isLoading } = useGetEmailsConversationFolders();

  if (isLoading) {
    return <Loader />;
  }

  return <FolderList folders={folders} onFolderSelected={onFolderSelected} />;
};

export default EmailsConversationFolders;
