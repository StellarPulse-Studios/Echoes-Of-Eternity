using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Turn", menuName = "Player/State/Unarmed/Turn")]
    public class UnarmedTurnSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.animator.applyRootMotion = true;
            board.isAnimationCompleted = false;
            board.animator.SetFloat("TurnAngle", board.turnAngle);
            board.animator.SetTrigger("Turn");
        }

        public override void OnExit(Blackboard board)
        {
            board.isAnimationCompleted = false;
            board.animator.applyRootMotion = false;
            board.animator.SetFloat("TurnAngle", 0.0f);

            float angleDiff = Quaternion.Angle(board.playerTransform.rotation, board.targetRotation);
            if (angleDiff < 5.0f)
                board.playerTransform.rotation = board.targetRotation;
        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
