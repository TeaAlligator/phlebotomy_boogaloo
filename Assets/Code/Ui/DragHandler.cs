using System.Collections.Generic;
using System.Linq;
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
            SortChildrenByName(transform.parent);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var duplicate = Instantiate(gameObject);
            duplicate.transform.SetParent(transform.parent);
            duplicate.transform.localScale = transform.localScale;
            duplicate.name = gameObject.name;

            transform.SetParent(_dragParentLimbo);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (transform.parent == _dragParentLimbo)
                Destroy(gameObject);
        }

        public static void SortChildrenByName(Transform parent)
        {
            var children = parent.Cast<Transform>().ToList();

            // Un-parent
            foreach (var child in children)
            {
                child.SetParent(null);
            }

            children.Sort((t1, t2) => System.String.Compare(t1.name, t2.name, System.StringComparison.Ordinal));
            
            // Re-parent
            foreach (var child in children)
            {
                child.SetParent(parent);
            }
        } 
    }
}
