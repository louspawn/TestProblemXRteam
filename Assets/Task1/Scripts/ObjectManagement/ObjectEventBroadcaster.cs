using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectEventBroadcaster : MonoBehaviour
{
    [Tooltip("Time in seconds before the first broadcast")]
    public float StartTime = 0;
    [Tooltip("Time in seconds between each broadcast")]
    public float RepeatTime = 5;
    [Tooltip("ObjectDataLoader that contains the loaded JSON objects")]
    public ObjectDataLoader ObjectLoader;
    [Tooltip("Event invoked when a random object gets broadcasted")]
    public UnityEvent<ObjectDataLoader.ObjectData> OnRandomObjectSelected = new UnityEvent<ObjectDataLoader.ObjectData>();

    private void Start()
    {
        if (ObjectLoader == null)
        {
            Debug.LogError("ObjectDataHandler is not set in EventBroadcaster");
            return;
        }

        InvokeRepeating("Broadcast", StartTime, RepeatTime);
    }

    public void Broadcast()
    {
        if (ObjectLoader.Objects == null)
        {
            return;
        }

        if (ObjectLoader.Objects.Count > 0)
        {
            int randomIndex = Random.Range(0, ObjectLoader.Objects.Count);
            ObjectDataLoader.ObjectData randomObjectData = ObjectLoader.Objects[randomIndex];

            OnRandomObjectSelected.Invoke(randomObjectData);
        }
    }
}
