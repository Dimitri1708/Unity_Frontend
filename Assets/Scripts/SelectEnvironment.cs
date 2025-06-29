using System.Collections.Generic;
using System.Threading.Tasks;
using ApiClient;
using Dto_s;
using UnityEngine;

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // For TextMeshPro support

public class SelectEnvironment : MonoBehaviour
{
    [Header("Prefab and Parent")]
    public GameObject selectWorldObject;   // Your prefab with two buttons
    public RectTransform parentTransform;  // UI container (e.g. ScrollView Content), must be a RectTransform
    public TMP_Text errorMessage;
    public Button createEnvironment;
    private List<EnvironmentReadDto> environmentReadDtoList;

    public static string environmentId;
      // Space in pixels between each instantiated prefab

    private async void Start()
    {
        Debug.Log("i got into environmentSelect Start method");
        environmentReadDtoList = await SelectEnvironmentApiClient.Read();
        if (environmentReadDtoList.Count == 0)
        {
            SceneManager.LoadScene("CreateEnvironment");
            return;
        }
        await GetEnvironments();
        createEnvironment.onClick.AddListener(CreateEnvironment);
    }

    public async Task GetEnvironments()
    {
        for (int i = 0; i < environmentReadDtoList.Count; i++)
        {
            var environment = environmentReadDtoList[i];

            // Instantiate prefab as child of parentTransform, worldPositionStays = false to keep UI layout correct
            GameObject instance = Instantiate(selectWorldObject, parentTransform, false);

            instance.transform.localScale = new Vector3(1.21f, 1f, 1f);
            // Get RectTransform and set pivot & anchored position for top-center anchoring layout
            RectTransform rt = instance.GetComponent<RectTransform>();

            // Make sure pivot is top-center (0.5, 1)
            rt.pivot = new Vector2(0.5f, 1f);

            // Position: X=0 (center), Y stacked downward by index

            // -------- Setup Buttons --------

            // Select Button
            Transform selectBtnTransform = instance.transform.Find("SelectButton");
            if (selectBtnTransform != null)
            {
                Button selectButton = selectBtnTransform.GetComponent<Button>();
                TMP_Text selectBtnText = selectBtnTransform.GetComponentInChildren<TMP_Text>();
                if (selectBtnText != null)
                    selectBtnText.text = $"{environment.environmentName}";

                if (selectButton != null)
                    selectButton.onClick.AddListener(() => OnSelect(environment));
            }
            else
            {
                Debug.LogWarning("SelectButton not found in prefab.");
            }

            // Delete Button
            Transform deleteBtnTransform = instance.transform.Find("DeleteButton");
            if (deleteBtnTransform != null)
            {
                Button deleteButton = deleteBtnTransform.GetComponent<Button>();
                TMP_Text deleteBtnText = deleteBtnTransform.GetComponentInChildren<TMP_Text>();
                if (deleteBtnText != null)
                    deleteBtnText.text = "Delete";

                if (deleteButton != null)
                    deleteButton.onClick.AddListener(() => OnDelete(environment.environmentId, instance));
            }
            else
            {
                Debug.LogWarning("DeleteButton not found in prefab.");
            }
        }
    }

    private async void OnSelect(EnvironmentReadDto environment)
    {
        Environment.environmentId = environment.environmentId;
        Environment.environmentXScale = environment.environmentXScale;
        Environment.environmentYScale = environment.environmentYScale;
        await SceneManager.LoadSceneAsync("Environment");
    }

    public async void CreateEnvironment()
    {
        await SceneManager.LoadSceneAsync("CreateEnvironment");
    }

    private async void OnDelete(string environmentId, GameObject instance)
    {   
        await SelectEnvironmentApiClient.Delete(environmentId, errorMessage);
        Destroy(instance);
    }
}
