using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Crouch End", menuName = "Player/State/Unarmed/Crouch End")]
    public class UnarmedCrouchEndSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.Velocity = Vector3.zero;
            board.PreviousVelocity = Vector3.zero;
            board.isAnimationCompleted = false;

            board.animator.SetBool("IsCrouched", false);
            board.animator.SetFloat("MoveSpeed", 0.0f);
        }

        public override void OnExit(Blackboard board)
        {
            board.isAnimationCompleted = false;
            board.dodge = false;
        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
