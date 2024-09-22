

using System.Collections.Generic;
using UnityEngine;

public abstract class WebRequestService : MonoBehaviour
{
    [System.Serializable]
    public class ModelData
    {
        public string[] attributes;
        public string model_name;
        public Vector3 position;
    }

    public abstract void GetModelByID(string id, System.Action<ModelData> callback);
}
