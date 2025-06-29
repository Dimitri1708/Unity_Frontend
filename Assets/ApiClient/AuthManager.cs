using System;
using System.Threading.Tasks;
using Dto_s;
using UnityEngine;

namespace ApiClient
{
    public static class AuthManager
    {
        public static string AccessToken { get; set; }
        public static string RefreshToken { get; set; }
        public static DateTime TokenExpiration { get; set; }

        public static bool IsTokenExpired()
        {
            return DateTime.UtcNow > TokenExpiration;
        }

        public static async Task<bool> RefreshAccessTokenAsync()
        {
            string url = "https://avansict2224479.azurewebsites.net/account/refresh";
            var payload = new { refreshToken = RefreshToken };
            string json = JsonUtility.ToJson(payload);

            var response = await ApiUtilities.PerformApiCall(url, "POST", json);

            if (response.StatusCode == 200)
            {
                var loginResponse = JsonUtility.FromJson<LoginResponseDto>(response.Body);
                AccessToken = loginResponse.accessToken;
                RefreshToken = loginResponse.refreshToken;
                TokenExpiration = DateTime.UtcNow.AddSeconds(loginResponse.expiresIn);
                return true;
            }

            Debug.LogError("Failed to refresh token");
            return false;
        }
    }
}