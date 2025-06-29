
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;

    namespace ApiClient
    {
        public static class ApiUtilities
        {
            public class ApiResponse
            {
                public long StatusCode { get; set; }
                public string Body { get; set; }
            }

            public static async Task<ApiResponse> PerformApiCall(string url, string method, string jsonData = null, string token = null)
            {
                Debug.Log("the code went into api client");
                using UnityWebRequest request = new UnityWebRequest(url, method);

                if (!string.IsNullOrEmpty(jsonData))
                {
                    byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                    request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                }

                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                if (!string.IsNullOrEmpty(token))
                {
                    request.SetRequestHeader("Authorization", "Bearer " + token);
                }

                await request.SendWebRequest();

                return new ApiResponse
                {
                    StatusCode = (long)request.responseCode,
                    Body = request.downloadHandler.text
                };
            }
        }
    }