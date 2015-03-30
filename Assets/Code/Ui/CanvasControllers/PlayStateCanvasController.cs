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
        /* PROPERTIES */
        private float _patientSpeechBubbleTimer;
        private readonly float _patientSpeechBubbleDisplayTime;

        /* REFERENCES */
        private readonly Messager _messager;

        private readonly GameObject _patient;
        private readonly GameObject _patientSpeechBubble;
        private readonly Text       _patientSpeechBubbleText;
        private readonly Button     _talkButton;
		private readonly Button _drawButton;
        private readonly GameObject _tourniquetTable;
        private readonly GameObject _tourniquet;
		private readonly GameObject _doctorsOrdersObject;

        /* TOKENS */
        private readonly MessagingToken _onPatientTalk;
		private readonly MessagingToken _newPatientToken;


        public PlayStateCanvasController(Messager messager, Canvas canvasView)
            : base(canvasView)
        {
            _patientSpeechBubbleTimer = -1;
            _patientSpeechBubbleDisplayTime = 2f;

            _messager = messager;

            _patient = GetElement("Patient");
            _patient.GetComponent<PatientDropbox>().Initialize(_messager);
            _patientSpeechBubble = GetElement("PatientSpeechBubble");
            _patientSpeechBubbleText = _patientSpeechBubble.transform.GetChild(0).GetComponent<Text>();
            _patientSpeechBubble.SetActive(false);
            _talkButton = GetElement<Button>("TalkButton");
            _tourniquetTable = GetElement("TourniquetTable");
            _tourniquet = _tourniquetTable.transform.GetChild(0).gameObject;
			_doctorsOrdersObject = GetElement("DocsOrders");
			_drawButton = GetElement("NeedleWindow").transform.Find("layoutgroup").transform.GetChild(0).GetComponent<Button>();
			_drawButton.onClick.AddListener(OnDrawButtonClicked);
            _talkButton.onClick.AddListener(OnTalkButtonClicked);

            _onPatientTalk = _messager.Subscribe<PatientTalkMessage>(OnPatientTalk);
			_newPatientToken = _messager.Subscribe<NewPatientMessage>(OnNewPatient);
        }

		private void OnDrawButtonClicked()
		{
			_messager.Publish(new DrawButtonClickedMessage());
		}

		private void OnNewPatient(NewPatientMessage input)
		{
			_doctorsOrdersObject.transform.GetChild(0).GetComponent<Text>().text = "Name:\t" + input.NewPatient.WristbandFirstName + " " + input.NewPatient.WristbandLastName;
			_doctorsOrdersObject.transform.GetChild(1).GetComponent<Text>().text = "ID:\t\t" + input.NewPatient.WristbandId;
			_doctorsOrdersObject.transform.GetChild(2).GetComponent<Text>().text = "Test:\t\t" + input.NewPatient.DoctorsOrders.GetType();
		}

        private void OnTalkButtonClicked()
        {
            _messager.Publish(new TalkButtonClickedMessage ());
        }

        private void OnPatientTalk(PatientTalkMessage message)
        {
            _patientSpeechBubbleTimer = 0;
            _patientSpeechBubble.SetActive(true);
            _patientSpeechBubbleText.text = message.Text;
        }

        public override void Update()
        {
            if (_patientSpeechBubbleTimer != -1)
            {
                _patientSpeechBubbleTimer += Time.smoothDeltaTime;

                if (_patientSpeechBubbleTimer > _patientSpeechBubbleDisplayTime)
                {
                    _patientSpeechBubbleTimer = -1;
                    _patientSpeechBubble.SetActive(false);
                }
            }
        }

        public new void TearDown()
        {
            _messager.CancelSubscription(_onPatientTalk);
			_messager.CancelSubscription(_newPatientToken);

            base.TearDown();
        }
    }
}
