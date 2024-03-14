using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Crouch", menuName = "Player/State/Unarmed/Crouch")]
    public class UnarmedCrouchSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.isAnimationCompleted = false;
            board.animator.SetFloat("MoveSpeed", 0.0f);
            board.animator.SetBool("IsCrouched", true);
        }

        public override void OnExit(Blackboard board)
        {
            board.animator.SetBool("IsCrouched", false);

            if (board.Velocity.magnitude > 1.0f)
                board.PreviousVelocity = board.Velocity.normalized * board.crouchWalkSpeed;

            board.jump = false;
            board.isCrouched = false;
            board.dodge = false;
        }

        public override void OnUpdate(Blackboard board)
        {
            UpdateMovement(board);
        }

        private void UpdateMovement(Blackboard board)
        {
            Vector2 moveInput = board.move;
            Vector3 moveDir = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

            Vector3 currentVelocity = board.Velocity;
            float currentSpeed = currentVelocity.magnitude;
            float targetSpeed = GetTargetSpeed(board);

            board.CurrentSpeed = currentSpeed;

            currentSpeed = Accelerate(currentSpeed, targetSpeed, board);

            UpdateRotation(moveDir, board);

            moveDir = Quaternion.Euler(0.0f, board.playerTransform.eulerAngles.y, 0.0f) * Vector3.forward;

            board.Velocity = currentSpeed * moveDir;

            Vector3 stepDownMotion = board.extendedCharacterController.GetStepDownMotion(board.Velocity * Time.deltaTime);

            board.characterController.Move(stepDownMotion);

            board.animator.SetFloat("MoveSpeed", currentSpeed);
        }

        private float GetTargetSpeed(Blackboard board)
        {
            if (board.move == Vector2.zero)
                return 0.0f;

            return board.crouchWalkSpeed;
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
