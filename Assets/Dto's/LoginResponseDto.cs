namespace Dto_s
{
    [System.Serializable]
    public class LoginResponseDto
    {
        public string tokenType;
        public string accessToken;
        public int expiresIn;
        public string refreshToken;
    }
}