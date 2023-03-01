using BSH.Cameras;
using BSH.Characters;
using UDT.Core;

namespace BSH
{
    public class CharacterSystem : System<CharacterSystem>
    {
        public StandardObject _player;
        public SideViewCameraComponent _playerCamera;
		bool paused = false;
        
        public StandardObject CreateCharacter(string name)
        {
            var character = ObjectModule.CreateInstanceFromData(name);
            character.AddComponent<CharacterControllerComponent>();
            return character;
        }

        public void EnablePlayerCamera()
        {
            _playerCamera.virtualCamera.gameObject.SetActive(true);
        }

	    public void SetPaused(bool paused)
        {
			this.paused = paused;
        }
    }
}