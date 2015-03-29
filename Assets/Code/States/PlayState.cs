using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.UI;
using Assets.Code.Ui.CanvasControllers;
using UnityEngine;

namespace Assets.Code.States
{
    public class PlayState : BaseState
    {
        /* PROPERTIES */
        private readonly Messager _messager;
        private readonly CanvasProvider _canvasProvider;
        private UiManager _uiManager;

        /* REFERENCES */

        /* TOKENS */
        private MessagingToken _onTalkButtonClicked;

        public PlayState(IoCResolver resolver) : base(resolver)
        {
            _resolver.Resolve(out _messager);
            _resolver.Resolve(out _canvasProvider);
        }

        public override void Initialize()
        {
            _uiManager = new UiManager();
            _uiManager.RegisterUi(new PlayStateCanvasController(_messager, _canvasProvider.GetCanvas("play_canvas")));

            _onTalkButtonClicked = _messager.Subscribe<TalkButtonClickedMessage>(OnTalkButtonClicked);
        }

        public override void Update()
        {
            _uiManager.Draw();
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _messager.Publish(new PatientTalkMessage());
            }
        }

        public void OnTalkButtonClicked(TalkButtonClickedMessage message)
        {
            _messager.Publish(new PatientTalkMessage());
        }

        public override void TearDown()
        {
            _messager.CancelSubscription(_onTalkButtonClicked);
        }
    }
}
