using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Ui
{
    public class PatientDropbox : MonoBehaviour, IDropHandler
    {
        private Messager _messager;

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
            var obj = DragHandler.ItemBeingDragged;
            if (!obj || obj.name != "Tourniquet")
                return;

            if (transform.childCount < 1)
            {
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                _messager.Publish(new TourniquetOnPatientMessage());
            }
        }
    }
}
