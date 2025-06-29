using ApiClient;
using Dto_s;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
   public TMP_InputField email;
   public TMP_InputField password;
   public TMP_InputField confirmPassword;
   public Button signUp;
   public Button goToLogin;
   public TMP_Text errorMessage;

   public void Start()
   {
      signUp.onClick.AddListener(HandleSignUp);
      goToLogin.onClick.AddListener(GoToLogin);
   }

   public async void HandleSignUp()
   {
       if (password.text == confirmPassword.text)
       {
           var successful = await RegisterApiClient.Register(new PostRegisterRequestDto 
           {
                email = email.text,
                password = password.text,
           }, errorMessage);

           Debug.Log($"{successful}");
           if (successful)
           {
               successful = await LoginApiClient.Login(new PostLoginRequestDto
               {
                   email = email.text,
                   password = password.text
               }, errorMessage);

               if (successful)
               {
                   await SceneManager.LoadSceneAsync("SelectEnvironment");
                   return;
               }
           }
       }
       errorMessage.text = "Passwords do not match";
   }

   public async void GoToLogin()
   {
       await SceneManager.LoadSceneAsync("Login");
   }
}
