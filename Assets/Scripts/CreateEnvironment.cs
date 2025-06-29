using ApiClient;
using Dto_s;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateEnvironment : MonoBehaviour
{
    public TMP_InputField environmentName;
    public TMP_InputField xScale;
    public TMP_InputField yScale;
    public Button createEnvironment;
    public Button goToSelectEnvironment;
    public TMP_Text errorMessage;

    public void Start()
    {
        createEnvironment.onClick.AddListener(HandleCreateEnvironment);
        goToSelectEnvironment.onClick.AddListener(GoToSelectEnvironment);
    }

    public async void HandleCreateEnvironment()
    {
        int xValue = int.Parse(xScale.text);
        int yValue = int.Parse(yScale.text);
        if (environmentName.text == null || xScale.text == null || yScale.text == null)
        {
            errorMessage.text = "Please fill all the fields!";
            return;
        }

        if (xValue < 20 || xValue > 200)
        {
            errorMessage.text = "The X scale must be a number between 20 and 200!";
            return;
        }

        if (yValue < 10 || yValue > 100)
        {
            errorMessage.text = "The Y scale must be a number between 10 and 100!";
            return;
        }

        if (environmentName.text.Length < 1 || environmentName.text.Length > 25)
        {
            errorMessage.text = "The environment name must be between 1 and 25 characters!";
            return;
        }

        bool successful = await SelectEnvironmentApiClient.Create(new EnvironmentCreateDto
        {
            environmentName = environmentName.text,
            environmentXScale = xValue,
            environmentYScale = yValue,
        }, errorMessage);

        if (successful)
        {
            await SceneManager.LoadSceneAsync("SelectEnvironment");
        }
    }

    public async void GoToSelectEnvironment()
    {
        await SceneManager.LoadSceneAsync("SelectEnvironment");
    }
}
