using System.Globalization;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.States;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.CanvasControllers
{
    public class PlayStateCanvasController : BaseCanvasController
    {
        /* REFERENCES */
        private readonly Messager _messager;

        private readonly GameObject _patientSpeechBubble;
        private readonly Text       _patientSpeechBubbleText;
        private readonly Button _talkButton;

        /* TOKENS */
        private readonly MessagingToken _onPatientTalk;

        public PlayStateCanvasController(Messager messager, Canvas canvasView)
            : base(canvasView)
        {
            _messager = messager;

            _patientSpeechBubble = GetElement("PatientSpeechBubble");
            _patientSpeechBubbleText = _patientSpeechBubble.transform.GetChild(0).GetComponent<Text>();
            _talkButton = GetElement<Button>("TalkButton");

            _talkButton.onClick.AddListener(OnTalkButtonClicked);

            _onPatientTalk = _messager.Subscribe<PatientTalkMessage>(OnPatientTalk);
        }

        private void OnTalkButtonClicked()
        {
            _messager.Publish(new TalkButtonClickedMessage ());
        }

        private void OnPatientTalk(PatientTalkMessage message)
        {
            
        }

        public new void TearDown()
        {
            _messager.CancelSubscription(_onPatientTalk);

            base.TearDown();
        }
    }
}
