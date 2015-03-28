using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject ItemBeingDragged;
    public Vector3 StartPosition;
    public Transform StartParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartPosition = transform.position;
        StartParent = transform.parent;
        ItemBeingDragged = gameObject;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == StartParent)
            transform.position = StartPosition;
    }
}
