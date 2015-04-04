using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Ui
{
    [RequireComponent (typeof (CanvasGroup))]

    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Transform _dragParentLimbo;

        void Start()
        {
            _dragParentLimbo = GameObject.Find("DragParentLimbo").transform;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }

        //public static GameObject ItemBeingDragged;
        //private static Transform _dragParentLimbo;
        //public Vector3 StartPosition;
        //public Transform StartParent;
        //private GameObject _duplicate;

        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    if (!_dragParentLimbo)
        //         _dragParentLimbo = GameObject.Find("DragParentLimbo").transform;

        //    if (!_duplicate)
        //    {
        //        // Create a duplicate to drag
        //        _duplicate = Instantiate(gameObject);
        //        _duplicate.transform.SetParent(transform.parent);
        //        _duplicate.transform.position = transform.position;
        //        _duplicate.transform.localScale = transform.localScale;
        //    }
        //    else
        //    {
                
        //    }

        //    StartPosition = transform.position;
        //    StartParent = transform.parent;
        //    transform.SetParent(_dragParentLimbo);
        //    ItemBeingDragged = gameObject;
        //    GetComponent<CanvasGroup>().blocksRaycasts = false;
        //}

        //public void OnDrag(PointerEventData eventData)
        //{
        //    transform.position = eventData.position;
        //}

        //public void OnEndDrag(PointerEventData eventData)
        //{
        //    ItemBeingDragged = null;
        //    GetComponent<CanvasGroup>().blocksRaycasts = true;
        //    if (transform.parent == _dragParentLimbo)
        //    {
        //        transform.SetParent(StartParent);
        //        transform.position = StartPosition;
        //        Destroy(_duplicate);
        //    }
        //}
    }
}
