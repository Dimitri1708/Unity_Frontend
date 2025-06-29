using UnityEngine;

public class OriginalObject : MonoBehaviour
{
    public Color hoverColor = Color.yellow;
    public GameObject objectToSpawn; // Assign the prefab to spawn
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = hoverColor;
    }

    void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }

    void OnMouseDown()
    {
        if (objectToSpawn != null && PrefabObject.selectedObject == null)
        {
            Vector3 spawnPos = transform.position;
            GameObject prefabInstance = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
            prefabInstance.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
