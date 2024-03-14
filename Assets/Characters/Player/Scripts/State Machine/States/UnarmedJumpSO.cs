using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Jump", menuName = "Player/State/Unarmed/Jump")]
    public class UnarmedJumpSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            float y = -Mathf.Sign(board.gravity) * Mathf.Sqrt(2.0f * Mathf.Abs(board.gravity) * board.jumpHeight);
            board.Velocity.y += y;

            board.isAnimationCompleted = false;
            board.isGrounded = false;
            board.animator.SetBool("IsGrounded", false);
            board.animator.SetTrigger("Jump");
        }

        public override void OnExit(Blackboard board)
        {
            if (board.move == Vector2.zero && board.PreviousVelocity.magnitude > board.runSpeed)
            {
                board.PreviousVelocity = board.PreviousVelocity.normalized * board.runSpeed;
            }

            board.jump = false;
            board.isCrouched = false;
            board.dodge = false;
            board.isAnimationCompleted = false;
        }

        public override void OnUpdate(Blackboard board)
        {
            if (board.Velocity.y < 0.0f)
                board.fallingTime += Time.deltaTime;

            board.Velocity.y += board.gravity * Time.deltaTime;
            board.characterController.Move(board.Velocity * Time.deltaTime);
        }
    }
}
