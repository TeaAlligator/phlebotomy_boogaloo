using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Ui
{
    public class PatientDropbox : MonoBehaviour, IDropHandler
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
            if (_dragParentLimbo.childCount == 0)
                return;

            var obj = _dragParentLimbo.GetChild(0);
            if (obj.name != "Tourniquet")
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
