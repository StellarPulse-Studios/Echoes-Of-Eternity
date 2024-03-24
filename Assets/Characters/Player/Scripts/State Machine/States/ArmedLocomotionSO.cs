using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Armed Locomotion", menuName = "Player/State/Armed/Locomotion")]
    public class ArmedLocomotionSO : StateSO
    {
        public bool defaultRunning;

        public override void OnEnter(Blackboard board)
        {
            board.fallingTime = 0.0f;
            board.Velocity = board.PreviousVelocity;
            board.animator.SetFloat("H", 0.0f);
            board.animator.SetFloat("MoveSpeed", board.Velocity.magnitude);
            board.animator.SetBool("IsArmed", true);
            board.animator.SetBool("IsGrounded", true);

            // Weapons
            board.sword.SetActive(true);
            board.shield.SetActive(true);
        }

        public override void OnExit(Blackboard board)
        {
            board.PreviousVelocity = board.Velocity;
            board.PreviousSpeed = board.PreviousVelocity.magnitude;

            // Weapons
            board.sword.SetActive(false);
            board.shield.SetActive(false);
        }

        public override void OnUpdate(Blackboard board)
        {
            UpdateMovement(board);
        }

        private void UpdateMovement(Blackboard board)
        {
            Vector2 moveInput = board.move;
            float moveInputMagnitude = moveInput.magnitude;
            Vector3 moveDir = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;
            moveInputMagnitude = Mathf.Clamp01(moveInputMagnitude);

            Vector3 currentVelocity = board.Velocity;
            float currentSpeed = currentVelocity.magnitude;
            float targetSpeed = GetTargetSpeed(board, moveInputMagnitude);

            board.CurrentSpeed = currentSpeed;

            currentSpeed = Accelerate(currentSpeed, targetSpeed, board);

            UpdateRotation(moveDir, board);

            moveDir = Quaternion.Euler(0.0f, board.playerTransform.eulerAngles.y, 0.0f) * Vector3.forward;

            board.Velocity = currentSpeed * moveDir;

            Vector3 stepDownMotion = board.extendedCharacterController.GetStepDownMotion(board.Velocity * Time.deltaTime);

            board.characterController.Move(stepDownMotion);

            board.animator.SetFloat("MoveSpeed", currentSpeed);
        }

        private float GetTargetSpeed(Blackboard board, float inputMagnitude)
        {
            if (board.move == Vector2.zero)
                return 0.0f;

            float speed = defaultRunning ? board.swordRunSpeed : board.swordWalkSpeed;

            speed *= (inputMagnitude <= 0.708) ? 0.5f : 1.0f;

            return speed;
        }

        private float Accelerate(float currentSpeed, float targetSpeed, Blackboard board)
        {
            const float threshold = 0.1f;

            // acceleration and deceleration
            if (currentSpeed < targetSpeed - threshold || currentSpeed > targetSpeed + threshold)
            {
                currentSpeed += Mathf.Sign(targetSpeed - currentSpeed) * board.acceleration * Time.deltaTime;
                currentSpeed = Mathf.Round(currentSpeed * 1000.0f) * 0.001f;
            }
            else
            {
                currentSpeed = targetSpeed;
            }

            return currentSpeed;
        }

        private void UpdateRotation(Vector3 moveDir, Blackboard board)
        {
            if (board.move == Vector2.zero)
                return;

            float targetYRot = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + board.cameraTransform.eulerAngles.y;
            Quaternion targetRot = Quaternion.Euler(0.0f, targetYRot, 0.0f);
            board.playerTransform.rotation = Quaternion.RotateTowards(board.playerTransform.rotation, targetRot, board.angularSpeed * Time.deltaTime);
        }
    }
}
