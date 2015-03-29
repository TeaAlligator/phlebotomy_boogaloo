using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Ui
{
    public class DragDropContainer : MonoBehaviour, IDropHandler
    {
        public int NumberOfObjects;
        
        public GameObject FirstItem
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

            if (transform.childCount < NumberOfObjects)
            {
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
            }
            else
            {
                var child = FirstItem;
                child.transform.position = obj.GetComponent<DragHandler>().StartPosition;
                child.transform.SetParent(obj.GetComponent<DragHandler>().StartParent);
            
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
            }
        }
    }
}
