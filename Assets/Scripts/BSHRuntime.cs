using UDT.Core;
using UDT.Core.Controllables;
using Unity.VisualScripting;
using UnityEngine;

namespace BSH
{
    public class BSHRuntime : Runtime<BSHRuntime>
    {
        public PlayerController _playerController;

        public override void Init()
        {
            name = "BSH Runtime";
            
            //_playerController = new PlayerController();
        }

        public class Title : State<BSHRuntime>
        {
            public void OnEnter()
            {
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
                //_cameraSystem = System<CameraSystem>.StartSystem().BaseInstance;
                //_characterSystem = System<CharacterSystem>.StartSystem(managedObjectType: "Character").BaseInstance;
                
                //Create Player
                //_player = _characterSystem.CreateCharacter("Player");
                //_characterSystem._player = _player;
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