using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using UnityEngine;

namespace Assets.Code.UnityBehaviours
{
    public class UnityReferenceMaster : MonoBehaviour, IResolvableItem
    {
        public void LoadCanvases(CanvasProvider canvasProvider)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var childCanvas = transform.GetChild(i).GetComponent<Canvas>();

                if (childCanvas != null)
                {
                    childCanvas.gameObject.SetActive(false);
                    canvasProvider.AddCanvas(childCanvas);
                }
            }
        }
    }
}
