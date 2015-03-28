using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragDropContainer : MonoBehaviour, IDropHandler {
    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var obj = DragHandler.ItemBeingDragged;
        if (!obj)
            return;

        var child = Item;

        if (!child)
        {
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
        }
        else
        {
            child.transform.position = obj.GetComponent<DragHandler>().StartPosition;
            child.transform.SetParent(obj.GetComponent<DragHandler>().StartParent);
            
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
        }
    }
}
