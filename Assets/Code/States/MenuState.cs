using System.Runtime.InteropServices;
using System.Threading;
using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Extensions;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.UI;
using Assets.Code.Ui.CanvasControllers;
using Assets.Code.UnityBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.States
{

    public class MenuState : BaseState
	{
		private UiManager _uiManager;
		private Messager _messager;

        private CanvasProvider _canvasProvider;

		private MessagingToken _stateChangedToken;

        public MenuState(IoCResolver resolver) : base(resolver)
        {
			_uiManager = new UiManager();
            _canvasProvider = resolver.Resolve<CanvasProvider>();
			_messager = resolver.Resolve<Messager>();
			_stateChangedToken = _messager.Subscribe<StateChangedMessage>(OnStateChange);
        }	

        public override void Initialize()
		{
			_uiManager.RegisterUi(new MenuStateCanvasController(_messager, _canvasProvider.GetCanvas("menu_canvas")));
        }

        public override void Update()
        {
			_uiManager.Draw();
        }

        public override void HandleInput()
        {
        }

        public override void TearDown()
		{
			_messager.CancelSubscription(_stateChangedToken);

			_uiManager.TearDown();
        }

		public void OnStateChange(StateChangedMessage input)
		{
			if (input.TargetStateType == typeof(PlayState))
			{
				SwitchState(new PlayState(_resolver));
			}
		}
    }
}
