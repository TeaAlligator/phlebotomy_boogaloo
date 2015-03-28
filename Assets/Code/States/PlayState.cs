using Assets.Code.DataPipeline;

namespace Assets.Code.States
{
    public enum error
    {
        NameMismatch = 0,
        IdMismatch,
        WrongTube,
        NoPermission
    }

    public class PlayState : BaseState
    {
        private static PatientGenerator _patientGenerator = new PatientGenerator();

        private Patient _patient;

        public PlayState(IoCResolver resolver) : base(resolver)
        {
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

        public void HandleMistake(error mistake)
        {
            switch (mistake)
            {
                case error.NameMismatch:
                    NameMismatch();
                    break;
                case error.IdMismatch:
                    IdMismatch();
                    break;
                case error.WrongTube:
                    WrongTube();
                    break;
                case error.NoPermission:
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
