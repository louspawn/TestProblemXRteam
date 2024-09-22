using UnityEngine;
using UnityEngine.Events;

public class ObjectSpawner : MonoBehaviour
{
    [Tooltip("Object that will be spawned on every broadcast")]
    public GameObject ObjectToSpawn;
    [Tooltip("Distance threshold before destroying the object")]
    public float DistanceThreshold = 2f;
    [Tooltip("Event broadcaster that triggers object spawning")]
    public ObjectEventBroadcaster Broadcaster;
    [Tooltip("Event invoked when an object gets spawned")]
    public UnityEvent<GameObject> OnSpawn = new UnityEvent<GameObject>();

    private void Start()
    {
        if (ObjectToSpawn == null)
        {
            Debug.LogError("ObjectToSpawn is not set in ObjectSpawner");
            return;
        }

        if (Broadcaster == null)
        {
            Debug.LogError("Broadcaster is not set in ObjectSpawner");
            return;
        }

        Broadcaster.OnRandomObjectSelected.AddListener(SpawnObject);
    }

    public void SpawnObject(ObjectDataLoader.ObjectData objectData)
    {
        Vector3 spawnPosition = new Vector3(
            objectData.x,
            objectData.y,
            objectData.z
        );

        GameObject spawnedObject = Instantiate(ObjectToSpawn, spawnPosition, Quaternion.identity);
        spawnedObject.name = objectData.object_id;

        // Add MeshGenerator component, set contour points and object area, and create mesh
        MeshGenerator meshGenerator = spawnedObject.AddComponent<MeshGenerator>();
        meshGenerator.contourPoints = objectData.contour_points;
        meshGenerator.objectArea = objectData.object_area;
        Mesh generatedMesh = meshGenerator.CreateMesh();

        // Set object position, raising it by 1 meter from the ground
        spawnedObject.transform.position = new Vector3(
            objectData.x,
            1,
            objectData.z
        );

        // Set mesh collider
        MeshCollider meshCollider = spawnedObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = generatedMesh;
        meshCollider.convex = true;

        // Get ObjectBehaviour and set distance threshold and constant velocity
        ObjectBehaviour objectBehaviour = spawnedObject.GetComponent<ObjectBehaviour>();
        objectBehaviour.DistanceThreshold = DistanceThreshold;
        objectBehaviour.ConstantVelocity = Vector3.forward * objectData.velocity;

        OnSpawn.Invoke(spawnedObject);
    }
}
