import { useContext } from 'react';
import { useMutation, useInfiniteQuery, useQuery } from '@tanstack/react-query';

import AuthContext from '../../store/AuthContext';
import AccountContext from '../../store/AccountContext';

import {
  signIn,
  signUp,
  addGoogleAccount,
  getServiceAccounts,
  getEmailsConversationFolders,
  getEmailsConversations,
  getEmails,
  deleteEmailsConversation,
  deleteEmail,
  sendEmail,
} from '../api';

import QUERY_KEYS from './queryKeys';

export const useSignIn = () => {
  const authContext = useContext(AuthContext);

  return useMutation({
    mutationFn: signIn,
    onSuccess: (data) => {
      const { token, expiresAt: rawExpiresAt, ...userInfo } = data;

      authContext.signIn(
        {
          token,
          expiresAt: new Date(rawExpiresAt),
        },
        {
          ...userInfo,
          id: userInfo.userId,
        }
      );
    },
  });
};

export const useSignUp = () => {
  const authContext = useContext(AuthContext);

  return useMutation({
    mutationFn: signUp,
    onSuccess: (data) => {
      const { token, expiresAt: rawExpiresAt, ...userInfo } = data;

      authContext.signIn(
        {
          token,
          expiresAt: new Date(rawExpiresAt),
        },
        {
          ...userInfo,
          id: userInfo.userId,
        }
      );
    },
  });
};

export const useAddGoogleAccount = () =>
  useMutationWithToken((request, token) => addGoogleAccount(request, token));

export const useGetServiceAccounts = () =>
  useQueryWithToken([QUERY_KEYS.GET_SERVICE_ACCOUNTS], (token) =>
    getServiceAccounts(token)
  );

export const useGetEmailsConversationFolders = () =>
  useAccountQuery(
    [QUERY_KEYS.GET_EMAIL_CONVERSATION_FOLDERS],
    ({ id }, token) => getEmailsConversationFolders(id, token)
  );

export const useGetEmailsConversations = (folderId) =>
  useAccountInfiniteQuery(
    [QUERY_KEYS.GET_EMAIL_CONVERSATIONS, folderId],
    ({ id }, pageToken, token) =>
      getEmailsConversations(id, folderId, pageToken, token),
    {
      initialPageParam: null,
      getNextPageParam: (lastPage) => lastPage.nextPageToken,
      enabled: !!folderId,
      select: ({ pages, pageParams }) => {
        return {
          pages: pages.flatMap(({ results }) => results),
          pageParams: [pageParams.at(-1)],
        };
      },
    }
  );

export const useGetEmails = (conversationId) =>
  useAccountQuery(
    [QUERY_KEYS.GET_EMAILS, conversationId],
    ({ id }, token) => getEmails(id, conversationId, token),
    {
      enabled: !!conversationId,
    }
  );

export const useDeleteEmailsConversation = () =>
  useAccountMutation(
    ({ id: conversationId, force }, { id: accountId }, token) =>
      deleteEmailsConversation(accountId, conversationId, force, token)
  );

export const useDeleteEmail = () =>
  useAccountMutation(({ id: emailId, force }, { id: accountId }, token) =>
    deleteEmail(accountId, emailId, force, token)
  );

export const useSendEmail = () =>
  useAccountMutation((request, { id: accountId }, token) =>
    sendEmail(accountId, request, token)
  );

export const useSendReply = () =>
  useAccountMutation(({ id: conversationId, body }, { id: accountId }, token) =>
    sendEmail(accountId, conversationId, { body }, token)
  );

function useAccountInfiniteQuery(queryKey, fetcher, options = {}) {
  const authContext = useContext(AuthContext);
  const accountContext = useContext(AccountContext);

  return useInfiniteQuery({
    queryKey,
    queryFn: ({ pageParam }) => {
      const { token } = authContext;
      const { selectedAccount } = accountContext;

      return fetcher(selectedAccount, pageParam, token);
    },
    ...options,
  });
}

function useAccountMutation(mutationFn, options = {}) {
  const accountContext = useContext(AccountContext);

  return useMutationWithToken(
    (data, token) => mutationFn(data, accountContext.selectedAccount, token),
    options
  );
}

function useAccountQuery(queryKey, fetcher, options = {}) {
  const accountContext = useContext(AccountContext);

  return useQueryWithToken(
    queryKey,
    (token) => fetcher(accountContext.selectedAccount, token),
    options
  );
}

function useMutationWithToken(mutationFn, options = {}) {
  const authContext = useContext(AuthContext);

  return useMutation({
    mutationFn: (data) => {
      const { token } = authContext;

      return mutationFn(data, token);
    },
    ...options,
  });
}

function useQueryWithToken(queryKey, fetcher, options = {}) {
  const authContext = useContext(AuthContext);

  return useQuery({
    queryKey,
    queryFn: () => {
      const { token } = authContext;
      return fetcher(token);
    },
    ...options,
  });
}
