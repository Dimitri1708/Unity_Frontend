using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dto_s;
using JetBrains.Annotations;
using UnityEngine;

namespace ApiClient
{
    public class ObjectApiClient
    {
        public static async Task Create(ObjectCreateDtoListWrapper objectCreateDtoListWrapper)
        {
            Debug.Log("i got in ObjectApiClient function");
            if (AuthManager.IsTokenExpired())
            {
                bool success = await AuthManager.RefreshAccessTokenAsync();
                if (!success)
                {
                    throw new Exception("Authentication required. Refresh token failed.");
                }
            }
            string url = "https://avansict2224479.azurewebsites.net/Object";
            foreach (var obj in objectCreateDtoListWrapper.objectCreateDtoList)
            {
                Debug.Log(obj.environmentId);
            }
            var json = JsonUtility.ToJson(objectCreateDtoListWrapper);
            Debug.Log($"hi im jason {json}");
            var response = await ApiUtilities.PerformApiCall(url, "POST", json, token: AuthManager.AccessToken);
            Debug.Log(response.Body);
        }

        [ItemCanBeNull]
        [CanBeNull]
        public static async Task<List<ObjectReadDto>> Read(string environmentId)
        {
            Debug.Log($"i got in ObjectApiClient function environmentID={environmentId}");
            if (AuthManager.IsTokenExpired())
            {
                bool success = await AuthManager.RefreshAccessTokenAsync();
                if (!success)
                {
                    throw new Exception("Authentication required. Refresh token failed.");
                }
            }
            string url = $"https://avansict2224479.azurewebsites.net/Object?environmentId={environmentId}";
            Debug.Log($"this is the token{AuthManager.AccessToken}");
            var response = await ApiUtilities.PerformApiCall(url, "GET", token: AuthManager.AccessToken);
            Debug.Log($" this is response {response.Body}{response.StatusCode}");
            ObjectReadDtoListWrapper objectReadDtoListWrapper = JsonUtility.FromJson<ObjectReadDtoListWrapper>(response.Body);
            foreach (var obj in objectReadDtoListWrapper.objectReadDtoList)
            {
                Debug.Log("this is just empty");
            }
            return objectReadDtoListWrapper.objectReadDtoList;
        }

        public static async Task Update(ObjectUpdateDtoListWrapper objectUpdateDtoListWrapper)
        {
            Debug.Log("i got in ObjectApiClient function");
            if (AuthManager.IsTokenExpired())
            {
                bool success = await AuthManager.RefreshAccessTokenAsync();
                if (!success)
                {
                    throw new Exception("Authentication required. Refresh token failed.");
                }
            }
            string url = $"https://avansict2224479.azurewebsites.net/Object";
            var json = JsonUtility.ToJson(objectUpdateDtoListWrapper);
            await ApiUtilities.PerformApiCall(url, "PUT", json, token: AuthManager.AccessToken);
        }

        public static async Task Delete(ObjectIdListWrapper objectIdListWrapper)
        {
            Debug.Log("i got in ObjectApiClient function");
            if (AuthManager.IsTokenExpired())
            {
                bool success = await AuthManager.RefreshAccessTokenAsync();
                if (!success)
                {
                    throw new Exception("Authentication required. Refresh token failed.");
                }
            }
            string url = $"https://avansict2224479.azurewebsites.net/Object";
            var json = JsonUtility.ToJson(objectIdListWrapper);
            await ApiUtilities.PerformApiCall(url, "DELETE", json, token: AuthManager.AccessToken);
        }
    }
}