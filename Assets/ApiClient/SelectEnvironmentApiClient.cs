using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dto_s;
using TMPro;
using UnityEngine;

namespace ApiClient
{
    public static class SelectEnvironmentApiClient
    {
        public static async Task<bool> Create(EnvironmentCreateDto environmentCreateDto, TMP_Text environmentName)
        {
            if (AuthManager.IsTokenExpired())
            {
                bool success = await AuthManager.RefreshAccessTokenAsync();
                if (!success)
                {
                    throw new Exception("Authentication required. Refresh token failed.");
                }
            }
            string url = "https://avansict2224479.azurewebsites.net/Environment";
            var json = JsonUtility.ToJson(environmentCreateDto);
            var response = await ApiUtilities.PerformApiCall(url, "POST", json, AuthManager.AccessToken);

            switch (response.StatusCode)
            {
                case 201: // Created
                    Debug.Log("Environment successfully created.");
                    return true;

                case 401: // Unauthorized
                    environmentName.text = "Unauthorized: Please log in.";
                    Debug.LogWarning("401 - Unauthorized.");
                    return false;

                case 403: // Max environments reached
                    environmentName.text = "You have reached the environment limit.";
                    Debug.LogWarning("403 - Limit reached.");
                    return false;

                case 500: // Server error
                    environmentName.text = "Server error occurred. Try again later.";
                    Debug.LogError("500 - Server error: " + response.Body);
                    return false;

                default: // Unknown error
                    environmentName.text = $"Unexpected error ({response.StatusCode})";
                    Debug.LogError("Unhandled response: " + response.Body);
                    return false;
            }
        }
        
        public static async Task<List<EnvironmentReadDto>> Read()
        {
            if (AuthManager.IsTokenExpired())
            {
                bool success = await AuthManager.RefreshAccessTokenAsync();
                if (!success)
                {
                    throw new Exception("Authentication required. Refresh token failed.");
                }
            }
            string url = "https://avansict2224479.azurewebsites.net/Environment";
            var response = await ApiUtilities.PerformApiCall(url, "Get", token: AuthManager.AccessToken);
            EnvironmentReadDtoListWrapper environmentReadDtoListWrapper = JsonUtility.FromJson<EnvironmentReadDtoListWrapper>(response.Body);
            return environmentReadDtoListWrapper.environmentReadDtoList;
        }

        public static async Task Delete(string environmentId, TMP_Text errorMessage)
        {
            if (AuthManager.IsTokenExpired())
            {
                bool success = await AuthManager.RefreshAccessTokenAsync();
                if (!success)
                {
                    throw new Exception("Authentication required. Refresh token failed.");
                }
            }
            string url = $"https://avansict2224479.azurewebsites.net/Environment?environmentId={environmentId}";
            var response = await ApiUtilities.PerformApiCall(url, "Delete", token: AuthManager.AccessToken);
            switch (response.StatusCode)
            {
                case 200:
                    Debug.Log("Environment deleted successfully.");
                    break;

                case 404:
                    errorMessage.text = "The environment was not found.";
                    break;

                case 500:
                    if (response.Body.Contains("Database error"))
                        errorMessage.text = "A database error occurred while deleting the environment.";
                    else if (response.Body.Contains("Unexpected error"))
                        errorMessage.text = "An unexpected server error occurred.";
                    else
                        errorMessage.text = "Server error. Please try again later.";
                    break;

                default:
                    errorMessage.text = $"Unknown error occurred. Code: {response.StatusCode}";
                    break;
            }
        }
    }
}