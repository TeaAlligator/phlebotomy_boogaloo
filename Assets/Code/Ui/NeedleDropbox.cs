using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Ui
{
    public class NeedleDropbox : MonoBehaviour, IDropHandler
    {
        private Messager _messager;

        private Transform _dragParentLimbo;

        void Start()
        {
            _dragParentLimbo = GameObject.Find("DragParentLimbo").transform;
        }

        public GameObject FirstItem
        {
            get
            {
                if (transform.childCount > 0)
                    return transform.GetChild(0).gameObject;
                return null;
            }
        }

        public void Initialize(Messager messager)
        {
            _messager = messager;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var obj = _dragParentLimbo.GetChild(0);
            if (!obj || obj.tag != "Tube")
                return;

            if (transform.childCount < 2)
            {
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(10, -10, 0);
                obj.transform.Rotate(new Vector3(0, 0, 90));
                obj.transform.localScale = new Vector3(2.5f, 0.8f, 1);
                _messager.Publish(new VialAddedToNeedleMessage
                {
                    Vial = obj.gameObject
                });
            }
        }
    }
}
