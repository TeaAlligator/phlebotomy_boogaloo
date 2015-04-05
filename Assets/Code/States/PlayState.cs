﻿using System;
﻿using System.Runtime.InteropServices;
using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.UI;
using Assets.Code.Ui.CanvasControllers;
using UnityEngine;
﻿using UnityEngine.UI;

namespace Assets.Code.States
{
    public enum MistakeType
    {
        NameMismatch = 0,
        IdMismatch,
        WrongTube,
		NoPermission,
		NoTourniquet
    }

    public enum PlayStages
    {
        TalkStage = 0,
        DrawStage,
        FinishedDrawStage,
        LabelStage,
        WrapUpStage
    }

    public class PlayState : BaseState
    {
		public int Mistakes;
		public int NotMistakes;

        /* PROPERTIES */
        private readonly Messager _messager;
        private readonly CanvasProvider _canvasProvider;
		private readonly PrefabProvider _prefabProvider;
        private UiManager _uiManager;

        private static PatientGenerator _patientGenerator = new PatientGenerator();
        private Patient _currentPatient;

        private Canvas _playCanvas;
        private Tube _tube;
		private GameObject _tubeSlider;

        /* Stage changes / Conditionals */
        private PlayStages _currentStage;

        private bool _tourniquetOnPatient;
		private bool _patientPermission;

		private TubeType _currentTubeType;

        /* REFERENCES */

        /* TOKENS */
        private MessagingToken _onTalkButtonClicked;
        private MessagingToken _onTourniquetOnPatient;
        private MessagingToken _onVialAddedToNeedle;
		private MessagingToken _onDrawClicked;
		private MessagingToken _newPatientToken;

        public PlayState(IoCResolver resolver) : base(resolver)
        {
            _resolver.Resolve(out _messager);
            _resolver.Resolve(out _canvasProvider);
            _resolver.Resolve(out _prefabProvider);
        }

        public override void Initialize()
		{
			_uiManager = new UiManager();
			_uiManager.RegisterUi(new PlayStateCanvasController(_messager, _canvasProvider.GetCanvas("play_canvas")));
        
			_currentStage = PlayStages.TalkStage;
            _tourniquetOnPatient = false;

            _onTalkButtonClicked = _messager.Subscribe<TalkButtonClickedMessage>(OnTalkButtonClicked);
            _onTourniquetOnPatient = _messager.Subscribe<TourniquetOnPatientMessage>(OnTourniquetOnPatient);
			_onDrawClicked = _messager.Subscribe<DrawButtonClickedMessage>(OnDrawClicked);
			_newPatientToken = _messager.Subscribe<NewPatientMessage>(OnNewPatient);
            _onVialAddedToNeedle = _messager.Subscribe<VialAddedToNeedleMessage>(OnVialAddedToNeedle);

            _patientGenerator = new PatientGenerator();
			_playCanvas = _canvasProvider.GetCanvas("play_canvas");
			_playCanvas.gameObject.SetActive(true);
			_currentTubeType = TubeType.Edta;
			NewPatient();
        }

        private void OnVialAddedToNeedle(VialAddedToNeedleMessage message)
        {
            _tubeSlider = message.Vial;
            _tube = _tubeSlider.GetComponent<Tube>();
            _tube.Initialize((TubeType)Enum.Parse(typeof(TubeType), message.Vial.name));
        }

		private void OnNewPatient(NewPatientMessage input)
		{
			_tourniquetOnPatient = false;
			_patientPermission = false;
		    if (_tubeSlider)
		    {
		        _tubeSlider.GetComponent<Slider>().value = 0;
                _tube.StopDraw();
		    }
		    _currentPatient = input.NewPatient;
		}

		public void OnDrawClicked(DrawButtonClickedMessage input)
		{
		    if (!_tubeSlider)
		        return;

		    if (input.Value)
		    {
		        //_tubeSlider.GetComponent<Slider>().value = 0;

		        if (_currentPatient.DoctorsOrders != _currentTubeType)
		            HandleMistake(MistakeType.WrongTube);
		        else if (_currentPatient.FirstName != _currentPatient.WristbandFirstName ||
		                 _currentPatient.LastName != _currentPatient.WristbandLastName)
		            HandleMistake(MistakeType.NameMismatch);
		        else if (_currentPatient.Id != _currentPatient.WristbandId)
		            HandleMistake(MistakeType.IdMismatch);
		        else if (!_tourniquetOnPatient)
		            HandleMistake(MistakeType.NoTourniquet);
		        else if (!_patientPermission)
		            HandleMistake(MistakeType.NoPermission);
		        else
		            HandleNotMistake();

		        _messager.Publish(new ScoreChangedMessage {NewMistakes = Mistakes, NewNotMistakes = NotMistakes});

		        _patientPermission = false;

		        _tube.StartDraw();
		    }
            else
		        _tube.StopDraw();
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
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SwitchState(new MenuState(_resolver));
			}
			if (Input.GetKeyDown(KeyCode.Q))
			{
				_tube.StopDraw();
			}
        }

        private void OnTourniquetOnPatient(TourniquetOnPatientMessage message)
        {
            _tourniquetOnPatient = true;
        }

        private void OnTalkButtonClicked(TalkButtonClickedMessage message)
        {
            var newMessage = new PatientTalkMessage();

            switch (_currentStage)
            {
                case PlayStages.TalkStage:
                    if (!_tourniquetOnPatient)
                    {
                        newMessage.Text = "I'm " + _currentPatient.FirstName + " " + _currentPatient.LastName + ".";
                        break;
                    }
                    else
                    {
						if (!_currentPatient.Rebellious)
						{
							_patientPermission = true;
							newMessage.Text = "Sure, go ahead.";
						}
						else
						{
							_patientPermission = false;
							newMessage.Text = "NOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOPE. NOPE NOPE NOPE. NOPE. NOPE.";
						}
                    }
                    break;
                case PlayStages.DrawStage:
                    newMessage.Text = "...";
                    break;
                case PlayStages.FinishedDrawStage:
                    newMessage.Text = "Thanks";
                    break;
                case PlayStages.WrapUpStage:
                    newMessage.Text = "Alright, I'll be going now";
                    break;
            }

            _messager.Publish(newMessage);
        }

        public override void TearDown()
        {
			_messager.CancelSubscription(_onTalkButtonClicked, _onDrawClicked, _onTourniquetOnPatient, _newPatientToken);

			_uiManager.TearDown();

			UnityEngine.Object.Destroy(_tubeSlider);
        }

        public void NewPatient()
        {
			_currentPatient = _patientGenerator.GeneratePatient();
			_messager.Publish(new NewPatientMessage { NewPatient = _currentPatient });
        }

        public void HandleMistake(MistakeType mistake)
        {
            switch (mistake)
            {
                case MistakeType.NameMismatch:
                    NameMismatch();
                    break;
                case MistakeType.IdMismatch:
                    IdMismatch();
                    break;
                case MistakeType.WrongTube:
                    WrongTube();
                    break;
                case MistakeType.NoPermission:
                    NoPermission();
                    break;
				case MistakeType.NoTourniquet:
					NoTourniquet();
					break;
            }
        }

		private void NoTourniquet()
		{
			Mistakes++;
		}

        private void NameMismatch()
		{
			Mistakes++;
        }

        private void IdMismatch()
		{
			Mistakes++;
        }

        private void WrongTube()
		{
			Mistakes++;
        }

        private void NoPermission()
		{
			Mistakes++;
        }

		private void SetupUI()
		{
		}

		private void HandleNotMistake()
		{
			NotMistakes++;
		}
    }
}
