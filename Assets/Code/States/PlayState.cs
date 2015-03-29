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
    public enum MistakeType
    {
        NameMismatch = 0,
        IdMismatch,
        WrongTube,
        NoPermission
    }

    public class PlayState : BaseState
    {
		private float _currentTime = 0;
		private float _endTime = 0;
        private CanvasProvider _canvasProvider;
		private PrefabProvider _prefabProvider;

        private static PatientGenerator _patientGenerator;
        private Patient _patient;

        private Canvas _menuCanvas;
		private Tube _tube;

        public PlayState(IoCResolver resolver) : base(resolver)
        {
            _patientGenerator = new PatientGenerator();
			_prefabProvider = resolver.Resolve<PrefabProvider>();
            _canvasProvider = resolver.Resolve<CanvasProvider>();
            _menuCanvas = _canvasProvider.GetCanvas("menu_canvas");
            _menuCanvas.gameObject.SetActive(true);
			var tubeSlider = Object.Instantiate(_prefabProvider.GetPrefab("Slider"));
			tubeSlider.transform.SetParent(_menuCanvas.transform);
			tubeSlider.transform.localScale = new Vector3(-10, 10, 1);
			tubeSlider.transform.localPosition = new Vector3(0, 0, 0);
			_tube = tubeSlider.GetComponent<Tube>();
			_tube.StartDraw();
        }	

        public override void Initialize()
        {
        }

        public override void Update()
        {
            NewPatient();
        }

        public override void HandleInput()
        {
        }

        public override void TearDown()
        {
        }

        public void NewPatient()
        {
            _patient = _patientGenerator.GeneratePatient();
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
