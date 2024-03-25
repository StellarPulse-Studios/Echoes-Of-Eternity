using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 look;
        public bool sprint;
        public bool jump;
        public bool crouch;
        public bool dodge;
        public bool block;
        public Blackboard blackboard;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
            blackboard.move = move;
        }

        private void OnLook(InputValue value)
        {
            look = value.Get<Vector2>();
            blackboard.look = look;
        }

        private void OnSprint(InputValue value)
        {
            sprint = value.isPressed;
            blackboard.sprint = sprint;
        }

        private void OnJump(InputValue value)
        {
            jump = value.isPressed;
            blackboard.jump = jump;
        }

        private void OnCrouch(InputValue value)
        {
            blackboard.isCrouched = !blackboard.isCrouched;
            crouch = blackboard.isCrouched;
        }

        private void OnDodge(InputValue value)
        {
            dodge = value.isPressed;
            blackboard.dodge = dodge;
        }

        private void OnExit(InputValue value)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Application.Quit();
        }

        private void OnLightAttack(InputValue value)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            blackboard.attack = value.isPressed;
            blackboard.lightAttack = value.isPressed;
        }

        private void OnHeavyAttack(InputValue value)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            blackboard.attack = value.isPressed;
            blackboard.heavyAttack = value.isPressed;
            blackboard.isCharging = value.isPressed;
        }

        private void OnBlock(InputValue value)
        {
            block = value.isPressed;
            blackboard.block = value.isPressed;
        }

        private void OnOne(InputValue value)
        {
            blackboard.isArmed = false;
            blackboard.animator.SetLayerWeight(1, 0.0f);
            blackboard.swordInHand.SetActive(false);
            blackboard.shieldInHand.SetActive(false);
            blackboard.torchInHand.SetActive(false);
            blackboard.swordInBack.SetActive(true);
            blackboard.shieldInBack.SetActive(true);
        }

        private void OnTwo(InputValue value)
        {
            blackboard.isArmed = true;
            blackboard.animator.SetLayerWeight(1, 0.0f);
            blackboard.torchInHand.SetActive(false);
            blackboard.swordInBack.SetActive(false);
            blackboard.shieldInBack.SetActive(false);
        }

        private void OnThree(InputValue value)
        {
            blackboard.isArmed = false;
            blackboard.animator.SetLayerWeight(1, 1.0f);
            blackboard.torchInHand.SetActive(true);
            blackboard.swordInBack.SetActive(true);
            blackboard.shieldInBack.SetActive(true);
        }

        public void RumbleGamepad(float low, float high)
        {
            Gamepad pad = Gamepad.current;
            if (pad == null)
                return;

            pad.SetMotorSpeeds(low, high);
        }
    }
}
