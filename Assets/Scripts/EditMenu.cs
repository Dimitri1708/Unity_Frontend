using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiClient;
using Dto_s;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EditMenu : MonoBehaviour
    {
        public Button dragging;
        public Button rotate;
        public Button scale;
        public Button close;
        public bool isDraggingEnabled = false;
        public bool isRotating = false;
        public int scaleIs = 1;
        public Color color = Color.yellow;
        public Button saveWorld;
        public ObjectCreateDtoListWrapper objectCreateDtoListWrapper;
        public ObjectUpdateDtoListWrapper objectUpdateDtoListWrapper;
        public ObjectIdListWrapper objectIdListWrapper;
        public GameObject prefabSquare;
        public GameObject prefabCircle;
        public GameObject prefabTriangle;
        private List<ObjectReadDto> objectDtoList;
        public GameObject environment;
        public Button delete;

        public void Start()
        {
            GetEnvironment();
            GetAllObjects();
            delete.onClick.AddListener(Delete);
            dragging.onClick.AddListener(Dragging);
            rotate.onClick.AddListener(Rotate);
            scale.onClick.AddListener(Scale);
            close.onClick.AddListener(Close);
            saveWorld.onClick.AddListener(SaveWorld);
        }

        public void Update()
        {
            if (isRotating)
            {
                PrefabObject.selectedObject.transform.Rotate(0, 0, 90 * Time.deltaTime);
            }
        }

        public void GetEnvironment()
        {
            environment.transform.localScale = new Vector3(Environment.environmentXScale, Environment.environmentYScale, 1);
        }
        public async void GetAllObjects()
        {
            Debug.Log("GetAllObjects called");
            var response = await ObjectApiClient.Read(Environment.environmentId);
            objectDtoList = response;

            foreach (var obj in response)
            {
                Debug.Log(obj.objectId);
                GameObject prefabToUse = null;

                if (obj.shape == "Square") prefabToUse = prefabSquare;
                else if (obj.shape == "Circle") prefabToUse = prefabCircle;
                else if (obj.shape == "Triangle") prefabToUse = prefabTriangle;

                if (prefabToUse == null)
                {
                    Debug.LogWarning($"Unknown shape type: {obj.shape}");
                    continue;
                }

                float posX = (float)obj.positionX;
                float posY = (float)obj.positionY;
                float scaleX = (float)obj.scaleX;
                float scaleY = (float)obj.scaleY;
                float rotationZ = (float)obj.rotation;

                GameObject instanceObject = Instantiate(
                    prefabToUse,
                    new Vector3(posX, posY, 89f),
                    Quaternion.Euler(0, 0, rotationZ)
                );

                instanceObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);
                instanceObject.name = obj.objectId;   // Now string// So we know shape later
            }
        }

        public async void SaveWorld()
{
    Debug.Log("i got in Save World function");
    
    if (objectCreateDtoListWrapper == null)
        objectCreateDtoListWrapper = new ObjectCreateDtoListWrapper();
    if (objectCreateDtoListWrapper.objectCreateDtoList == null)
        objectCreateDtoListWrapper.objectCreateDtoList = new List<ObjectCreateDto>();

    if (objectUpdateDtoListWrapper == null)
        objectUpdateDtoListWrapper = new ObjectUpdateDtoListWrapper();
    if (objectUpdateDtoListWrapper.objectUpdateDtoList == null)
        objectUpdateDtoListWrapper.objectUpdateDtoList = new List<ObjectUpdateDto>();

    if (objectIdListWrapper == null)
        objectIdListWrapper = new ObjectIdListWrapper();
    if (objectIdListWrapper.objectIdList == null)
        objectIdListWrapper.objectIdList = new List<string>();
    
    GameObject[] objectList = GameObject.FindGameObjectsWithTag("Prefab");

    if (objectDtoList == null)
    {
        Debug.LogWarning("objectDtoList is null — creating all objects as new.");
        foreach (GameObject obj in objectList)
        {
            objectCreateDtoListWrapper.objectCreateDtoList.Add(new ObjectCreateDto
            {
                scaleX = Mathf.RoundToInt(obj.transform.localScale.x),
                scaleY = Mathf.RoundToInt(obj.transform.localScale.y),
                positionX = Mathf.RoundToInt(obj.transform.position.x),
                positionY = Mathf.RoundToInt(obj.transform.position.y),
                rotation = Mathf.RoundToInt(obj.transform.rotation.eulerAngles.z),
                shape = obj.GetComponent<SpriteRenderer>().sprite.name,
                environmentId = Environment.environmentId
            });
        }
    }
    else
    {
        // 🔁 Existing logic
        foreach (GameObject obj in objectList)
        {
            bool isNew = true;
            foreach (var readObj in objectDtoList)
            {
                if (obj.name == readObj.objectId)
                {
                    isNew = false;
                    break;
                }
            }

            if (isNew)
            {
                objectCreateDtoListWrapper.objectCreateDtoList.Add(new ObjectCreateDto
                {
                    scaleX = Mathf.RoundToInt(obj.transform.localScale.x),
                    scaleY = Mathf.RoundToInt(obj.transform.localScale.y),
                    positionX = Mathf.RoundToInt(obj.transform.position.x),
                    positionY = Mathf.RoundToInt(obj.transform.position.y),
                    rotation = Mathf.RoundToInt(obj.transform.rotation.eulerAngles.z),
                    shape = obj.GetComponent<SpriteRenderer>().sprite.name,
                    environmentId = Environment.environmentId
                });
            }
            else
            {
                objectUpdateDtoListWrapper.objectUpdateDtoList.Add(new ObjectUpdateDto
                {
                    objectId = obj.name,
                    scaleX = Mathf.RoundToInt(obj.transform.localScale.x),
                    scaleY = Mathf.RoundToInt(obj.transform.localScale.y),
                    positionX = Mathf.RoundToInt(obj.transform.position.x),
                    positionY = Mathf.RoundToInt(obj.transform.position.y),
                    rotation = Mathf.RoundToInt(obj.transform.rotation.eulerAngles.z),
                });
            }
        }

        foreach (var readObj in objectDtoList)
        {
            bool isGone = true;
            if (objectList == null || !objectList.Any())
            {
                Debug.Log("objectList is null");
            }
            foreach (var obj in objectList)
            {
                Debug.Log($"this is the if {readObj.objectId}{obj.name}");
                if (readObj.objectId == obj.name)
                {
                    Debug.Log($"this is the if {readObj.objectId}{obj.name}");
                    isGone = false;
                    break;
                }
            }
            Debug.Log($"this is the if {isGone}");

            if (isGone)
            {
                objectIdListWrapper.objectIdList.Add(readObj.objectId);
            }
        }

        SceneManager.LoadSceneAsync("SelectEnvironment");
    }

    // 🔁 Save data to server
    if (objectCreateDtoListWrapper.objectCreateDtoList.Count > 0)
        await ObjectApiClient.Create(objectCreateDtoListWrapper);
    if (objectUpdateDtoListWrapper.objectUpdateDtoList.Count > 0)
        await ObjectApiClient.Update(objectUpdateDtoListWrapper);

    if (objectIdListWrapper.objectIdList.Count > 0)
        await ObjectApiClient.Delete(objectIdListWrapper);
}


        public void Dragging()
        {
            isDraggingEnabled = !isDraggingEnabled;
            TMP_Text buttonText = dragging.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = isDraggingEnabled ? "Disable Dragging" : "Enable Dragging";
            }
        }

        public void Rotate() => isRotating = !isRotating;

        public void Scale()
        {
            if (scaleIs == 1)
            {
                PrefabObject.selectedObject.transform.localScale = new Vector3(4f, 4f, 1f);
                scaleIs = 2;
            }
            else if (scaleIs == 2)
            {
                PrefabObject.selectedObject.transform.localScale = new Vector3(1f, 1f, 1f);
                scaleIs = 3;
            }
            else if (scaleIs == 3)
            {
                PrefabObject.selectedObject.transform.localScale = new Vector3(2f, 2f, 1f);
                scaleIs = 1;
            }
        }

        public void Delete()
        {
            isDraggingEnabled = false;
            RectTransform rt = GetComponent<RectTransform>();
            Destroy(PrefabObject.selectedObject);
            PrefabObject.selectedObject = null;
            Vector2 offsetMin = rt.offsetMin;
            offsetMin.x = 3000f;
            rt.offsetMin = offsetMin;
        }

        public void Close()
        {
            isDraggingEnabled = false;

            if (PrefabObject.selectedObject != null)
            {
                SpriteRenderer sr = PrefabObject.selectedObject.GetComponent<SpriteRenderer>();
                sr.color = color;
                PrefabObject.selectedObject = null;
            }
            RectTransform rt = GetComponent<RectTransform>();
            Vector2 offsetMin = rt.offsetMin;
            offsetMin.x = 3000f;
            rt.offsetMin = offsetMin;
        }
    }
}