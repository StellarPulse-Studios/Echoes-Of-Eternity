using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Skid Stop", menuName = "Player/State/Unarmed/Skid Stop")]
    public class UnarmedSkidStopSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.Velocity = Vector3.zero;
            board.PreviousVelocity = Vector3.zero;
            board.animator.applyRootMotion = true;
            board.isAnimationCompleted = false;
            board.animator.SetTrigger("Skid");
            board.animator.SetFloat("MoveSpeed", 0.0f);
        }

        public override void OnExit(Blackboard board)
        {
            board.isAnimationCompleted = false;
            board.animator.applyRootMotion = false;
        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
