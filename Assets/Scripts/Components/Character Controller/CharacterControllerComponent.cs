using System;
using System.ComponentModel;
using UDT.Core;
using UDT.Core.Controllables;
using UDT.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BSH.Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerComponent : StandardComponent<CharacterControllerData, CharacterSystem>, IControllable
    {
        //Values of the Character Controller
        [ReadOnly(true)] public CharacterController _characterController;
        
        public enum MovingState
        {
            Idle, Run, Sprint, Jumping, Falling
        }
        [ReadOnly(true)] public MovingState movingState;
        public Vector3 velocity;

        private float jumpTime;
        private bool jumpPressed;
        private bool movePressed;
        private bool grounded;
        private float horizontalMovementInput;
        private Vector2 movementInput;
        bool enableGravity = true;

        public override void OnInstantiate()
        {
            base.OnInstantiate();
            _characterController = GetComponent<CharacterController>();
        }
        
        void Update()
        {
            //Check if the Character is grounded
            RaycastHit hit;
            grounded = Physics.Raycast(transform.position, Physics.gravity.normalized, out hit, _characterController.height / 2 + 0.1f);
            
            //Set the Moving State based on the grounded state
            if (grounded)
            {
                if (movingState == MovingState.Falling)
                {
                    movingState = MovingState.Idle;
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
            if (jumpPressed)
                jumpTime += Time.deltaTime;
            else
                jumpTime += Time.deltaTime + Time.deltaTime * Data.JumpSustainWeight;
            
            if (jumpTime > Data.JumpCurve.keys[^1].time && movingState == MovingState.Jumping) movingState = MovingState.Falling;
            
            //Apply Gravity
            if (enableGravity && !grounded) velocity += Physics.gravity/60;
            else if(movingState != MovingState.Jumping) velocity.y = 0;

            if (!movePressed)
            {
                horizontalMovementInput = Mathf.Lerp(horizontalMovementInput,0,Data.groundDeceleration * Time.deltaTime);
                velocity.x = Data.decelerationCurve.Evaluate(Mathf.Abs(horizontalMovementInput)) * horizontalMovementInput * Data.groundSpeed;
            }
            else
            {
                horizontalMovementInput = Mathf.Lerp(horizontalMovementInput, movementInput.x, Data.groundAcceleration * Time.deltaTime);
                
                var velocityX = Data.accelerationCurve.Evaluate(Mathf.Abs(horizontalMovementInput)) * horizontalMovementInput * Data.groundSpeed;
                if(Mathf.Abs(velocity.x) < Mathf.Abs(velocityX) || Mathf.Sign(velocity.x) != Mathf.Sign(velocityX)) velocity.x = velocityX;
            }

            
            //Move the Character
            _characterController.Move(velocity * Time.deltaTime);
        }

        public override void OnInputAction(InputAction.CallbackContext context)
        {
            if (context.action.name == "Move")
            {
                OnMove(context);
            }
            else if (context.action.name == "Jump")
            {
                OnJump(context);
            }
        }

        public virtual void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            this.movementInput = input;
            
            if(context.performed) movePressed = true;
            else if(context.canceled) movePressed = false;
        }
        
        public virtual void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                jumpPressed = true;
                if (grounded)
                {
                    jumpTime = 0;
                    movingState = MovingState.Jumping;
                    
                    velocity.x *= Data.onJumpSpeedBoost;
                }
            }
            else if (context.canceled)
            {
                jumpPressed = false;
            }
        }
    }
}