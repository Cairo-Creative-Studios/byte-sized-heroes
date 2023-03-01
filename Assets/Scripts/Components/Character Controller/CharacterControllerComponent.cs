using System.ComponentModel;
using UDT.Core;
using UDT.Core.Controllables;
using UDT.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BSH.Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerComponent : StandardComponent<CharacterControllerData, CharacterSystem>, IComponentControllable, IFSM
    {
        //Inherited from IComponentControllable
        public byte inputByte { get; set; }
        public bool isPossessed { get; set; }
        public Controller Controller { get; set; }
        public SerializableDictionary<string, string> InputsToMethodsMap { get; set; }
        //Inherited from IFSM
        public Tree<IStateNode> states { get; set; }
        
        //Values of the Character Controller
        [ReadOnly(true)] public CharacterController _characterController;
        
        public enum MovingState
        {
            Idle, Run, Sprint, Jumping, Falling
        }
        [ReadOnly(true)] public MovingState movingState;
        public Vector3 velocity;
        
        private float jumpTime;
        private bool grounded;
        bool enableGravity = true;

        public override void OnInstantiate()
        {
            InitMachine();
            _characterController = GetComponent<CharacterController>();
        }
        public void InitMachine()
        {
            StateMachineModule.AddStateMachine(this);
        }
        
        void Update()
        {
            //Check if the Character is grounded
            RaycastHit hit;
            grounded = Physics.Raycast(transform.position, Physics.gravity.normalized, out hit, _characterController.height / 2 + 0.1f);
            
            //Set the Moving State based on the grounded state
            if (grounded)
            {
                if (movingState == MovingState.Falling || movingState == MovingState.Jumping)
                {
                    OnLand();
                }
            }
            else
            {
                if (movingState == MovingState.Idle || movingState == MovingState.Run || movingState == MovingState.Sprint)
                {
                    movingState = MovingState.Falling;
                }
            }
            
            //Apply Jump Curve
            if(movingState == MovingState.Jumping) velocity.y = Data.JumpCurve.Evaluate(jumpTime) * Data.jumpStrength;
            jumpTime += Time.deltaTime;
            
            //Apply Gravity
            if (enableGravity && !grounded) velocity += Physics.gravity;
            else velocity.y = 0;
            
            //Move the Character
            _characterController.Move(velocity * Time.deltaTime);
        }

        public virtual void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            velocity.x = input.x * Data.groundSpeed;
        }
        
        public virtual void OnJump(InputAction.CallbackContext context)
        {
            if(grounded && context.ReadValue<bool>() && movingState != MovingState.Jumping)
                movingState = MovingState.Jumping;
        }

        public virtual void OnLand()
        {
            if(movingState == MovingState.Jumping || movingState == MovingState.Falling)
                movingState = MovingState.Idle;
        }

        public virtual void LandCheck()
        {
            var hit = Physics.Raycast(transform.position, Vector3.down, out var hitInfo, 1f);
            if (hit && hitInfo.collider.gameObject.layer == 8)
            {
                OnLand();
            }
        }
    }
}