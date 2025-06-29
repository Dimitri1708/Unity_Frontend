using System.Threading.Tasks;
using ApiClient;
using Dto_s;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;
    public Button login;
    public Button goToRegister;
    public TMP_Text errorMessage;

    public void Start()
    {
        login.onClick.AddListener(HandleLogin);
        goToRegister.onClick.AddListener(GoToRegister);
    }

    public async void HandleLogin()
    {
        if (email.text == "" || password.text == "")
        {
            errorMessage.text = "Please fill all fields";
            return;
        }
        var successful = await LoginApiClient.Login(new PostLoginRequestDto()
        {
            email = email.text,
            password = password.text
        }, errorMessage);
        
        Debug.Log(successful);
        if (successful)
        {
           await SceneManager.LoadSceneAsync("SelectEnvironment");
        }
        
    }
    public async void GoToRegister()
    {
        await SceneManager.LoadSceneAsync("Register");
    }
}
