﻿using System;
﻿using System.Runtime.InteropServices;
using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.UI;
using Assets.Code.Ui.CanvasControllers;
using UnityEngine;

namespace Assets.Code.States
{
    public enum MistakeType
    {
        NameMismatch = 0,
        IdMismatch,
        WrongTube,
        NoPermission
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

        /* REFERENCES */

        /* TOKENS */
        private MessagingToken _onTalkButtonClicked;
        private MessagingToken _onTourniquetOnPatient;

        public PlayState(IoCResolver resolver) : base(resolver)
        {
            _resolver.Resolve(out _messager);
            _resolver.Resolve(out _canvasProvider);
            _resolver.Resolve(out _prefabProvider);
        }

        public override void Initialize()
        {
            _currentStage = PlayStages.TalkStage;
            _tourniquetOnPatient = false;

            _uiManager = new UiManager();
            _uiManager.RegisterUi(new PlayStateCanvasController(_messager, _canvasProvider.GetCanvas("play_canvas")));

            _onTalkButtonClicked = _messager.Subscribe<TalkButtonClickedMessage>(OnTalkButtonClicked);
            _onTourniquetOnPatient = _messager.Subscribe<TourniquetOnPatientMessage>(OnTourniquetOnPatient);

            _patientGenerator = new PatientGenerator();
			_playCanvas = _canvasProvider.GetCanvas("play_canvas");
			_playCanvas.gameObject.SetActive(true);
			_tubeSlider = GameObject.Instantiate(_prefabProvider.GetPrefab("Slider"));
			_tubeSlider.transform.SetParent(_playCanvas.transform);
			_tubeSlider.transform.localScale = new Vector3(-2.5f, 6, 1);
			_tubeSlider.transform.localPosition = new Vector3(723.24f, 118.9f, 0);
			_tube = _tubeSlider.GetComponent<Tube>();
			_tube.StartDraw();
            _currentPatient = _patientGenerator.GeneratePatient();
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
				NewPatient();
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
                    newMessage.Text = "Sure, go ahead.";
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
			_messager.CancelSubscription(_onTalkButtonClicked);

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
            }
        }

        private void NameMismatch()
        {
        }

        private void IdMismatch()
        {
        }

        private void WrongTube()
        {
        }

        private void NoPermission()
        {
        }
    }
}
