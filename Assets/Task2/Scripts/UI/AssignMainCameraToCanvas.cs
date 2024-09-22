using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Canvas))]
public class AssignMainCameraToCanvas : MonoBehaviour
{
    [Tooltip("Enable the LookAtConstraint to make the canvas face the camera.")]
    public bool EnableLookAtCamera = true;

    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        // Add LookAtConstraint to make the canvas face the camera
        if (EnableLookAtCamera)
        {
            LookAtConstraint lookAtConstraint = gameObject.AddComponent<LookAtConstraint>();
            lookAtConstraint.constraintActive = true;
            lookAtConstraint.AddSource(new ConstraintSource
            {
                sourceTransform = Camera.main.transform,
                weight = 1
            });
        }
    }
}
