using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class PrefabObject : MonoBehaviour
{
    public GameObject buttonPrefab;       // UI button prefab (inside a world-space canvas)
    private Canvas dedicatedCanvas;       // Canvas (world-space)
    private GameObject spawnedButton;
    public SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color hoverColor = Color.yellow;
    private Vector3 offset;
    private float zCoord;
    public static GameObject selectedObject;
    private GameObject editMenu;
    public EditMenu editMenuScript;
    void Start()
    {
        editMenuScript = GameObject.Find("EditMenu").GetComponent<EditMenu>();
        // Find a world-space canvas by tag or fallback
        GameObject canvasObj = GameObject.FindWithTag("UIButtonCanvas");
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        editMenu = GameObject.Find("EditMenu");
        if (canvasObj != null)
        {
            dedicatedCanvas = canvasObj.GetComponent<Canvas>();
        }
        else
        {
            dedicatedCanvas = FindObjectOfType<Canvas>();
            Debug.LogWarning("Canvas not tagged. Using first Canvas found.");
        }

        if (dedicatedCanvas == null)
        {
            Debug.LogError("No suitable Canvas found in scene!");
        }
        else if (dedicatedCanvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogError("Assigned canvas is not in World Space!");
        }
    }

    void OnMouseEnter()
    {
        ShowButton();
    }

    void OnMouseExit()
    {
        DestroyButton();
    }

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        Debug.Log($"{selectedObject.name}, {editMenuScript.isDraggingEnabled}");
        if (editMenuScript.isDraggingEnabled && selectedObject == this.gameObject)
        {
            transform.position = GetMouseWorldPos() + offset;

            if (spawnedButton != null)
            {
                UpdateButtonPosition();
            }
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void ShowButton()
    {

        if (spawnedButton == null && buttonPrefab != null && dedicatedCanvas != null && selectedObject == null)
        {
            spawnedButton = Instantiate(buttonPrefab, dedicatedCanvas.transform);
            spawnedButton.transform.SetAsLastSibling(); // render on top
            UpdateButtonPosition();

            Button btn = spawnedButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnButtonClick);
            }
            else
            {
                Debug.LogWarning("Button prefab is missing a Button component!");
            }
        }
    }

    void UpdateButtonPosition()
    {
        if (spawnedButton == null || dedicatedCanvas == null) return;

        // Directly position the button at the prefab's world position
        // Add slight z offset so it renders in front
        Vector3 targetPosition = transform.position + new Vector3(0f, 0f, -0.01f); 
        spawnedButton.transform.position = targetPosition;

        // Optional: face the canvas toward the camera
        spawnedButton.transform.rotation = dedicatedCanvas.transform.rotation;
    }

    void DestroyButton()
    {
        if (spawnedButton != null)
        {
            Destroy(spawnedButton);
            spawnedButton = null;
        }
    }

    void OnButtonClick()
    {
        RectTransform rt = editMenu.GetComponent<RectTransform>();
        selectedObject = this.gameObject;
        spriteRenderer.color = hoverColor;
        Vector2 offsetMin = rt.offsetMin; // left & bottom offsets
        offsetMin.x = 1490f;              // set left offset to 1490 pixels
        rt.offsetMin = offsetMin;         // apply new offsetMin
        DestroyButton();
    }
}