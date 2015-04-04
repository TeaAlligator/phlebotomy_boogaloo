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
        private Transform _playCanvas;

        void Start()
        {
            _dragParentLimbo = GameObject.Find("DragParentLimbo").transform;
            _playCanvas = GameObject.Find("play_canvas").transform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (transform.parent.parent.name == "TestTubeTable")
            {
                var duplicate = Instantiate(gameObject);
                duplicate.transform.SetParent(transform.parent);
                duplicate.transform.localScale = transform.localScale;
                duplicate.name = gameObject.name;
                SortChildrenByName(transform.parent);
            }
            else if (transform.parent.name == "NeedleWindow")
            {
                transform.parent.GetComponent<NeedleDropbox>().ToggleHasVial();

            }

            transform.SetParent(_dragParentLimbo);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (transform.parent == _dragParentLimbo)
            {
                transform.SetParent(_playCanvas);
            }
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
