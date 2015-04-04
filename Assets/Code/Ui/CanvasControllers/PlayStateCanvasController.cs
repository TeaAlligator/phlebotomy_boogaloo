using System.Globalization;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.States;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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

		private static PatientGenerator _patientGenerator = new PatientGenerator();

        private readonly GameObject _patient;
        private readonly GameObject _patientSpeechBubble;
        private readonly Text       _patientSpeechBubbleText;
		private readonly Button _talkButton;
		private readonly Toggle _drawButton;
		private readonly GameObject _tubesSheet;
        private readonly GameObject _tourniquetTable;
        private readonly GameObject _tourniquet;
		private readonly GameObject _doctorsOrdersObject;
		private readonly Text _scoreText;

        /* TOKENS */
        private readonly MessagingToken _onPatientTalk;
		private readonly MessagingToken _newPatientToken;
		private readonly MessagingToken _scoreChangedToken;


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
            var needleWindow = GetElement("NeedleWindow");
            needleWindow.GetComponent<NeedleDropbox>().Initialize(_messager);
			_drawButton = needleWindow.transform.FindChild("Background").FindChild("Button").GetComponent<Toggle>();
			_drawButton.onValueChanged.AddListener(OnDrawButtonClicked);
            _talkButton.onClick.AddListener(OnTalkButtonClicked);
			_tubesSheet = GetElement("TubesSheet");
			_tubesSheet.GetComponent<Button>().onClick.AddListener(MakeSheetSmall);
			_scoreText = GetElement<Text>("ScoreText");
			GetElement<Button>("NewPatientButton").onClick.AddListener(NewPatient);
            GetElement("Sharps").GetComponent<SharpsDropbox>().Initialize(_messager);

            _onPatientTalk = _messager.Subscribe<PatientTalkMessage>(OnPatientTalk);
			_newPatientToken = _messager.Subscribe<NewPatientMessage>(OnNewPatient);
			_scoreChangedToken = _messager.Subscribe<ScoreChangedMessage>(OnScoreChanged);
        }

		void OnScoreChanged(ScoreChangedMessage input)
		{
			_scoreText.text = "Mistakes:\t\t\t" + input.NewMistakes + "\nNot-Mistakes:\t" + input.NewNotMistakes;
		}

		void MakeSheetBig()
		{
			_tubesSheet.transform.localScale = new Vector3(10, 10, 1);
			_tubesSheet.GetComponent<Button>().onClick.RemoveAllListeners();
			_tubesSheet.GetComponent<Button>().onClick.AddListener(MakeSheetSmall);
		}

		void MakeSheetSmall()
		{
			_tubesSheet.transform.localScale = new Vector3(1, 1, 1);
			_tubesSheet.GetComponent<Button>().onClick.RemoveAllListeners();
			_tubesSheet.GetComponent<Button>().onClick.AddListener(MakeSheetBig);
		}

		void NewPatient()
		{
			_messager.Publish(new NewPatientMessage { NewPatient = _patientGenerator.GeneratePatient() });
		}

		private void OnDrawButtonClicked(bool value)
		{
            _messager.Publish(new DrawButtonClickedMessage
            {
                Value = value
            });
		}

		private void OnNewPatient(NewPatientMessage input)
		{
			_doctorsOrdersObject.transform.GetChild(0).GetComponent<Text>().text = "Name:\t" + input.NewPatient.WristbandFirstName + " " + input.NewPatient.WristbandLastName;
			_doctorsOrdersObject.transform.GetChild(1).GetComponent<Text>().text = "ID:\t\t" + input.NewPatient.WristbandId;
			_doctorsOrdersObject.transform.GetChild(2).GetComponent<Text>().text = "Test:\t\t" + input.NewPatient.DoctorsOrders;
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
			_messager.CancelSubscription(_newPatientToken, _scoreChangedToken);

            base.TearDown();
        }
    }
}
