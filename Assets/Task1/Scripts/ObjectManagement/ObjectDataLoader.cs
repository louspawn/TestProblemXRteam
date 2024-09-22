using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDataLoader : MonoBehaviour
{
    [System.Serializable]
    public class ObjectData
    {
        public string object_id;
        public float x;
        public float y;
        public float z;
        public Vector2[] contour_points;
        public float object_area;
        public float velocity;
    }

    public List<ObjectData> Objects = new List<ObjectData>();

    private void Start()
    {
        LoadJSONFiles();
    }

    private void LoadJSONFiles()
    {
        TextAsset[] textAssets = Resources.LoadAll<TextAsset>("JSON/Objects");
        foreach (TextAsset textAsset in textAssets)
        {
            if (string.IsNullOrEmpty(textAsset.text))
            {
                Debug.LogError("JSON file is empty");
                continue;
            }

            Objects.Add(JsonUtility.FromJson<ObjectData>(textAsset.text));
        }

        if (Objects.Count == 0)
        {
            Debug.LogError("No valid JSON data found");
            return;
        }
    }
}
