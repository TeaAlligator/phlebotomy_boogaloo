using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Loading;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.States;
using Assets.Code.Utilities;
using UnityEngine;

namespace Assets.Code.UnityBehaviours
{
    public class StateMaster : MonoBehaviour
    {
        private UnityReferenceMaster _unityReference;
        private Messager _messager;
        private IoCResolver _resolver;

        private BaseState _currentState;

        public void Start()
        {
            _resolver = new IoCResolver();

#if UNITY_EDITOR
            /* RESOURCE LIST CREATION */
            FileServices.CreateResourcesList("Assets/Resources/resourceslist.txt");
#endif
            FileServices.LoadResourcesList("resourceslist");

            #region LOAD RESOURCES
            // messager
            _messager = new Messager();
            _resolver.RegisterItem(_messager);

            // unity reference master
            _unityReference = GetComponent<UnityReferenceMaster>();
            _resolver.RegisterItem(_unityReference);

            // texture provider
            var textureProvider = new TextureProvider();
            TextureLoader.LoadTextures(textureProvider, "Textures");
            _resolver.RegisterItem(textureProvider);

            // sound provider
            var soundProvider = new SoundProvider();
            SoundLoader.LoadSounds(soundProvider, "Sounds");
            _resolver.RegisterItem(soundProvider);

            // prefab provider
            var prefabProvider = new PrefabProvider();
            PrefabLoader.LoadPrefabs(prefabProvider);
            _resolver.RegisterItem(prefabProvider);

            // data provider
            var gameDataProvider = new GameDataProvider();
            _resolver.RegisterItem(gameDataProvider);

            // canvas provider
            var canvasProvider = new CanvasProvider();
            _unityReference.LoadCanvases(canvasProvider);
            _resolver.RegisterItem(canvasProvider);
            #endregion

            /* BEGIN STATE */
            _currentState = new MenuState(_resolver);
            _currentState.Initialize();

            /* SUBSCRIBE FOR GAME END */
            _messager.Subscribe<ExitGameMessage>(OnExitGame);
        }

        private void OnExitGame(ExitGameMessage message)
        {
            Application.Quit();
        }

        public void OnApplicationQuit()
        {
            SavePlayerData();
        }

        private void SavePlayerData()
        {
        }

        public void Update()
        {
            /* SWITCH STATE IF NEEDED */
            if (_currentState.IsReadyForStateSwitch)
            {
                var previousState = _currentState;
                _currentState = _currentState.TargetSwitchState;

                SavePlayerData();
                previousState.TearDown();
                _currentState.Initialize();
            }

            /* UPDATE STATE */
            _currentState.Update ();
            _currentState.HandleInput();
        }
    }
}