using UnityEngine;
using UnityEngine.Events;

public class ObjectBehaviour : MonoBehaviour
{
    [Tooltip("Constant velocity the object moves on every frame")]
    public Vector3 ConstantVelocity;
    [Tooltip("Distance threshold before destroying the object")]
    public float DistanceThreshold = 2f;
    [Tooltip("Event invoked when the object gets destroyed")]
    public UnityEvent<string> OnDestroy = new UnityEvent<string>();

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        transform.position += ConstantVelocity * Time.deltaTime;

        // Destroy object after moving DistanceThreshold meters
        if (Vector3.Distance(_startPosition, transform.position) >= DistanceThreshold)
        {
            OnDestroy.Invoke(gameObject.name);
            Destroy(gameObject);
        }
    }
}
