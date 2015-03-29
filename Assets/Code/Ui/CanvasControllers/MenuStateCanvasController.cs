using System.Globalization;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.States;
using Assets.Code.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.CanvasControllers
{
    public class MenuStateCanvasController : BaseCanvasController
    {
        /* REFERENCES */
        private readonly Messager _messager;

		private Button _playButton;
		private Button _exitButton;

		public MenuStateCanvasController(Messager messager, Canvas canvasView)
            : base(canvasView)
        {
			_messager = messager;
			canvasView.gameObject.SetActive(true);

			_playButton = GetElement<Button>("play_button");
			_playButton.onClick.AddListener(PlayButtonClicked);
			_exitButton = GetElement<Button>("exit_button");
			_exitButton.onClick.AddListener(ExitButtonClicked);
		}

		public void PlayButtonClicked()
		{
			_canvasView.gameObject.SetActive(false);
			_messager.Publish(new StateChangedMessage { TargetStateType = typeof(PlayState) });
		}

		public void ExitButtonClicked()
		{
			Application.Quit();
		}

        public override void Update()
        {
        }

        public new void TearDown()
        {
            base.TearDown();
        }
    }
}
