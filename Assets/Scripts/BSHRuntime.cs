using UDT.Core;
using UDT.Core.Controllables;
using UnityEngine;

namespace BSH
{
    public class BSHRuntime : Runtime<BSHRuntime>
    {
        public PlayerController playerController;

        public override void Init()
        {
            name = "BSH Runtime";
        }

        public class Title : State<BSHRuntime>
        {
            public override void Enter()
            {
                root.playerController = ControllerModule.CreatePlayerController("Player Controller");
                SetState("Play");
            }
        }
        
        public class Play: State<BSHRuntime>
        {
            CameraSystem _cameraSystem;
            CharacterSystem _characterSystem;
            StandardObject _player;
            
            public override void Enter()
            {
                //Start Game Systems
                _characterSystem = System<CharacterSystem>.StartSystem();
                _cameraSystem = System<CameraSystem>.StartSystem();
                
                
                Debug.Log(root.playerController);
                
                //Create Player
                //_player = _characterSystem.CreateCharacter("Player");
                //_characterSystem._player = _player;
                root.playerController.Possess(_characterSystem.GetObjectFromData("Player"));
            }
            
            public override void Exit()
            {
                System<CameraSystem>.StopSystem();
                System<CharacterSystem>.StopSystem();
            }
            
            public class Playing: State<Play>
            {
            }
            
            public class Pause: State<Play>
            {
            }
        }
        

        public class GameOver : State<BSHRuntime>
        {
        }
    }
}