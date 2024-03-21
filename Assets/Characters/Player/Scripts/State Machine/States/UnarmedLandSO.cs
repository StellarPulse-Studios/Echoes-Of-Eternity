using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Land", menuName = "Player/State/Unarmed/Land")]
    public class UnarmedLandSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.isAnimationCompleted = false;
            board.animator.applyRootMotion = true;

            board.animator.SetBool("IsGrounded", true);
            board.animator.SetFloat("MoveSpeed", 0.0f);

            // If jumped from very high ground, then trigger roll
            if (board.fallingTime >= board.totalFallTimeToBecomeUnstable)
            {
                board.animator.SetTrigger("Roll");
            }
        }

        public override void OnExit(Blackboard board)
        {
            board.isAnimationCompleted = false;
            board.animator.applyRootMotion = false;

            if (board.move == Vector2.zero)
            {
                board.PreviousVelocity = Vector3.zero;
                board.Velocity = Vector3.zero;
            }

            board.jump = false;
        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
