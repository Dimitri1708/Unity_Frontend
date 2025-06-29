using System;
using System.Threading.Tasks;
using Dto_s;
using TMPro;
using UnityEngine;

namespace ApiClient
{
    public static class RegisterApiClient
    {
        public static async Task<bool> Register(PostRegisterRequestDto postRegisterRequestDto, TMP_Text errorMessage)
        {
            var url = "https://avansict2224479.azurewebsites.net/register";
            var json = JsonUtility.ToJson(postRegisterRequestDto);
            var response = await ApiUtilities.PerformApiCall(url, "POST", json);
            if (response.StatusCode == 200 || response.StatusCode == 201)
            {
                return true;
            }
            errorMessage.text = response.Body;
            return false;
        }
    }
}