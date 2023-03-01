using NaughtyAttributes;
using UDT.Core;
using UnityEngine;

namespace BSH.Characters
{
    [CreateAssetMenu(fileName = "CharacterControllerData", menuName = "BSH/Character Controller Data", order = 0)]
    public class CharacterControllerData : ComponentData<CharacterControllerComponent>
    {
        [Header(" - Objects - ")] 
        [Tooltip("Path to the Character GameObject within the StandardObject Prefab")]
        public string characterPath = "character";
        
        //Speed
        [Header(" - Speed - ")]
        [Tooltip("The Speed of the Character when on the Ground")]
        public float groundSpeed = 5f;
        [Tooltip("Multiplier the GroundSpeed when the Character is Sprinting")]
        public float sprintMultiplier = 2f;
        [Tooltip("Multiplier to the GroundSpeed when the Character is in the Air")]
        public float airSpeed = 0.5f;
        
        //Jump control
        [Header(" - Jump Control - ")]
        [Tooltip("The Strength of the Jump, this is multiplied to the JumpCurve")]
        public float jumpStrength = 10f;
        [Tooltip("Curve to control the Jump")]
        public AnimationCurve JumpCurve;
        [Tooltip("Multiplier to movement speed, that pushes the player forward on Jump")]
        public float onJumpSpeedBoost = 1.2f;
        
        //Orientation
        public enum OrientationType
        {
            None, Floor
        }
        
        [Header(" - Orientation - ")]
        [Tooltip("The Type of Orientation the Character will have. " +
                 "Floor, will make the Character always align to the floor. " +
                    "MovementOnFloor, will make the Character align to the floor, only when moving")]
        public OrientationType orientationType = OrientationType.None;
        [ShowIf("orientationType", OrientationType.Floor)]
        [Tooltip("The Speed at which the Character will align to the floor")]
        public float alignmentSpeed = 1f;
        [ShowIf("orientationType", OrientationType.Floor)]
        [Tooltip("The Minimum Speed the character must be moving to align to the floor")]
        public float minimumMovementSpeedForAlignment = 0f;

        //Wall Climb
        public enum WallClimbType
        {
            None, Clamber, Slide
        }
        
        [Header(" - Wall Climb - ")]
        [Tooltip("The Type of Wall Climb the Character will do")]
        public WallClimbType wallClimbType = WallClimbType.None;
        [HideIf("wallClimbType", WallClimbType.None)]
        [Tooltip("The Distance away from the wall that the Character will be able to climb")]
        public float wallCheckDistance = 0.5f;
        [ShowIf("wallClimbType", WallClimbType.Slide)]
        [Tooltip("The Speed at which the Character will slide down the wall")]
        public float wallSlideSpeed = 1f;

        public override string GetAttachedGOPath()
        {
            return "Character";
        }
    }
}