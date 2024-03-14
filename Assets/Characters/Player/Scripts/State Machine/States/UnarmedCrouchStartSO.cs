using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Crouch Start", menuName = "Player/State/Unarmed/Crouch Start")]
    public class UnarmedCrouchStartSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.Velocity = Vector3.zero;
            board.PreviousVelocity = Vector3.zero;
            board.isAnimationCompleted = false;

            board.animator.SetBool("IsCrouched", true);
        }

        public override void OnExit(Blackboard board)
        {
            board.isAnimationCompleted = false;
        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
