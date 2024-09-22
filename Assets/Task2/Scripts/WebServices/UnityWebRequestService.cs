using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWebRequestService : WebRequestService
{
    private string baseUrl = "https://www.mysite.com/api/";
    private string modelEndpoint = "models/";

    public override void GetModelByID(string id, System.Action<ModelData> callback)
    {
        string url = baseUrl + modelEndpoint + id;
        StartCoroutine(GetRequest(url, callback));
    }

    IEnumerator GetRequest(string url, System.Action<ModelData> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Received: " + request.downloadHandler.text);
                ModelData modelData = JsonUtility.FromJson<ModelData>(request.downloadHandler.text);
                callback(modelData);
            }
            else
            {
                Debug.Log("Error: " + request.error);
                callback(null);
            }
        }
    }
}
