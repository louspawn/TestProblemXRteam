using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Event to trigger when the object is clicked.")]
    public UnityEvent<PointerEventData> OnClick = new UnityEvent<PointerEventData>();

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(eventData);
    }
}
