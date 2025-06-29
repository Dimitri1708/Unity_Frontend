using System;
using System.Threading.Tasks;
using Dto_s;
using TMPro;
using UnityEngine;

namespace ApiClient
{
    public class LoginApiClient
    {
        public static async Task<bool> Login(PostLoginRequestDto postLoginRequestDto, TMP_Text errorMessage)
        {
            var url = "https://avansict2224479.azurewebsites.net/account/login";
            var json = JsonUtility.ToJson(postLoginRequestDto);
            var response = await ApiUtilities.PerformApiCall(url, "POST", json);
            LoginResponseDto loginResponse = JsonUtility.FromJson<LoginResponseDto>(response.Body);
            AuthManager.AccessToken = loginResponse.accessToken;
            AuthManager.RefreshToken = loginResponse.refreshToken;
            AuthManager.TokenExpiration = DateTime.UtcNow.AddSeconds(loginResponse.expiresIn);
            if (response.StatusCode == 200 || response.StatusCode == 201)
            {
                return true;
            }
            errorMessage.text = response.Body;
            return false;
        }
    }
}