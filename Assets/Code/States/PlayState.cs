using System;
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

        private int _currentStage;

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
            _currentStage = 0;

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
            var Message = new PatientTalkMessage();

            switch (_currentStage)
            {
                case 0:
                case 1:
                    Message.Text = "I'm Adam Sandler.";
                    _currentStage = Math.Min(1, _currentStage);
                    break;
                case 2:
                    Message.Text = "Sure, go ahead.";
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                    Message.Text = "...";
                    break;
                case 7:
                case 8:
                    Message.Text = "Thanks";
                    break;
                default:
                    Message.Text = "Alright, I'll be going now";
                    break;
            }

            _messager.Publish(new PatientTalkMessage());
        }

        public override void TearDown()
        {
            _messager.CancelSubscription(_onTalkButtonClicked);
        }
    }
}
