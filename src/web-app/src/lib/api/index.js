import axios from 'axios';

import { ApiError, ApiValidationError } from './errors';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

const apiClient = axios.create({
  baseURL: API_BASE_URL,
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    const errorResponse = error.response;

    if (!errorResponse?.data) {
      return Promise.reject(
        new ApiError(
          'Unknown Error!',
          'Some unknown error occured while processing your request!'
        )
      );
    }

    const { type } = errorResponse.data;

    if (type === 'ValidationError') {
      return Promise.reject(
        ApiValidationError.fromValidationResponse(errorResponse)
      );
    }

    return Promise.reject(ApiError.fromResponse(errorResponse));
  }
);

export const signIn = async (request) =>
  (await apiClient.post('/api/users/sign-in', request)).data;

export const signUp = async (request) =>
  (await apiClient.post('/api/users/sign-up', request)).data;

export const addGoogleAccount = async (request, token) =>
  (
    await apiClient.post('/api/service-accounts/google', request, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
  ).data;

export const getCurrentUser = async (token) =>
  (
    await apiClient.get('/api/users/me', {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
  ).data;

export const getServiceAccounts = async (token) =>
  (
    await apiClient.get('/api/service-accounts', {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
  ).data;

export const getServiceAccountById = async (id, token) =>
  (
    await apiClient.get(`/api/service-accounts/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
  ).data;

export const getEmailsConversationFolders = async (accountId, token) =>
  (
    await apiClient.get(`/api/service-accounts/${accountId}/folders`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
  ).data;

export const getEmailsConversations = async (
  accountId,
  folderId,
  pageToken,
  token
) => {
  let relativePath = `/api/service-accounts/${accountId}/folders/${folderId}/conversations`;
  if (pageToken != null) {
    relativePath += `?pageToken=${pageToken}`;
  }

  return (
    await apiClient.get(relativePath, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
  ).data;
};

export const getEmails = async (accountId, conversationId, token) =>
  (
    await apiClient.get(
      `/api/service-accounts/${accountId}/conversations/${conversationId}/emails`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    )
  ).data;

export const deleteEmailsConversation = async (
  accountId,
  conversationId,
  force,
  token
) =>
  await apiClient.delete(
    `/api/service-accounts/${accountId}/conversations/${conversationId}?force=${force}`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

export const deleteEmail = async (accountId, emailId, force, token) =>
  await apiClient.delete(
    `/api/service-accounts/${accountId}/emails/${emailId}?force=${force}`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

export const sendEmail = async (accountId, request, token) =>
  (
    await apiClient.post(`/api/service-accounts/${accountId}`, request, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
  ).data;

export const sendReply = async (accountId, conversationId, request, token) =>
  (
    await apiClient.post(
      `/api/service-accounts/${accountId}/conversations/${conversationId}`,
      request,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    )
  ).data;

export default apiClient;
