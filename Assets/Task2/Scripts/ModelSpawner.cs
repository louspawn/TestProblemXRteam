using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelSpawner : MonoBehaviour
{
    [Tooltip("The ObjectSpawner component to listen to for spawned objects.")]
    public ObjectSpawner ObjSpawner;
    [Tooltip("The WebRequestService component to use for fetching model data.")]
    public WebRequestService WebRequestService;
    [Tooltip("The canvas to display a base panel for the model.")]
    public GameObject ModelBaseCanvas;
    [Tooltip("The canvas to display attributes for the model.")]
    public GameObject ModelAttributesCanvas;

    private Dictionary<string, GameObject> _modelDictionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        if (ObjSpawner == null)
        {
            Debug.LogError("ObjectSpawner is not set in ModelSpawner");
            return;
        }

        if (WebRequestService == null)
        {
            Debug.LogError("WebRequestService is not set in ModelSpawner");
            return;
        }

        if (ModelBaseCanvas == null)
        {
            Debug.LogError("ModelBaseCanvas is not set in ModelSpawner");
            return;
        }

        if (ModelAttributesCanvas == null)
        {
            Debug.LogError("ModelAttributesCanvas is not set in ModelSpawner");
            return;
        }

        ObjSpawner.OnSpawn.AddListener(ObjectSpawned);
    }

    private void ObjectSpawned(GameObject spawnedObject)
    {
        spawnedObject.GetComponent<ObjectBehaviour>().OnDestroy.AddListener(ObjectDestroyed);
    }

    private void ObjectDestroyed(string objectID)
    {
        WebRequestService.GetModelByID(objectID, ModelDataReceived);
    }

    private void ModelDataReceived(WebRequestService.ModelData modelData)
    {
        if (modelData == null)
        {
            Debug.LogError("Model data is null");
            return;
        }

        GameObject model = Resources.Load<GameObject>("Models/" + modelData.model_name);

        if (model && !_modelDictionary.ContainsKey(modelData.model_name))
        {
            GameObject spawnedModel = Instantiate(model, modelData.position, Quaternion.identity);
            spawnedModel.name = modelData.model_name;

            _modelDictionary.Add(modelData.model_name, spawnedModel);

            ScaleModelToFitBounds(spawnedModel, new Vector3(0.5f, 0.5f, 0.5f));

            Vector3 modelCanvasPosition = new Vector3(
                spawnedModel.transform.position.x,
                spawnedModel.transform.position.y - 0.25f,
                spawnedModel.transform.position.z
            );
            GameObject spawnedModelBaseCanvas = Instantiate(ModelBaseCanvas, modelCanvasPosition, ModelBaseCanvas.transform.rotation);
            spawnedModelBaseCanvas.transform.SetParent(spawnedModel.transform);

            GameObject spawnedModelAttributesCanvas = Instantiate(ModelAttributesCanvas, modelCanvasPosition, Quaternion.identity);
            spawnedModelAttributesCanvas.transform.SetParent(spawnedModel.transform);

            AddAttributesToModel(modelData, spawnedModelAttributesCanvas);

            spawnedModel.AddComponent<BoxCollider>();
            EventClick eventClick = spawnedModel.AddComponent<EventClick>();

            eventClick.OnClick.AddListener(ModelClicked);
        }
    }

    private void ScaleModelToFitBounds(GameObject model, Vector3 bounds)
    {
        // Get the current bounds of the model
        Renderer modelRenderer = model.GetComponentInChildren<Renderer>();
        if (modelRenderer == null)
        {
            Debug.LogError("Model does not have a Renderer component.");
            return;
        }

        Vector3 modelSize = modelRenderer.bounds.size;

        // Calculate the scaling factor needed to fit the model within the bounds
        float scaleX = bounds.x / modelSize.x;
        float scaleY = bounds.y / modelSize.y;
        float scaleZ = bounds.z / modelSize.z;

        // Use the smallest scaling factor to ensure the model fits within the bounds
        float scaleFactor = Mathf.Min(scaleX, scaleY, scaleZ);

        // Apply the scaling factor to the model
        model.transform.localScale *= scaleFactor;
    }

    private void AddAttributesToModel(WebRequestService.ModelData modelData, GameObject spawnedModelAttributesCanvas)
    {
        // Update the text of the attributes canvas to display the model attributes
        for (int i = 0; i < spawnedModelAttributesCanvas.transform.childCount; i++)
        {
            // If there are more attributes than text elements, hide the extra text elements
            if (i >= modelData.attributes.Length)
            {
                spawnedModelAttributesCanvas.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                spawnedModelAttributesCanvas.transform.GetChild(i).GetComponent<TMPro.TextMeshProUGUI>().text = modelData.attributes[i];
            }
        }
    }

    private void ModelClicked(PointerEventData eventData)
    {
        string modelName = eventData.pointerPress.name;

        if (_modelDictionary.ContainsKey(modelName))
        {
            Destroy(_modelDictionary[modelName]);
            _modelDictionary.Remove(modelName);
        }
    }
}
