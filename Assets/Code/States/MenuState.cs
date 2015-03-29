using System.Runtime.InteropServices;
using System.Threading;
using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Extensions;
using Assets.Code.UnityBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.States
{

    public class MenuState : BaseState
    {
        private CanvasProvider _canvasProvider;
		private PrefabProvider _prefabProvider;

        private Canvas _menuCanvas;

        public MenuState(IoCResolver resolver) : base(resolver)
        {
			_prefabProvider = resolver.Resolve<PrefabProvider>();
            _canvasProvider = resolver.Resolve<CanvasProvider>();
            _menuCanvas = _canvasProvider.GetCanvas("menu_canvas");
            _menuCanvas.gameObject.SetActive(true);
        }	

        public override void Initialize()
        {
        }

        public override void Update()
        {
        }

        public override void HandleInput()
        {
        }

        public override void TearDown()
        {
        }
    }
}
