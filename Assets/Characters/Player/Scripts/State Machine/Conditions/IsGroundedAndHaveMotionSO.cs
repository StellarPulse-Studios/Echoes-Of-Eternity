using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Is Grounded And Have Motion", menuName = "Player/Condition/Is Grounded And Have Motion")]
    public class IsGroundedAndHaveMotionSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return board.isGrounded && !Mathf.Approximately(board.PreviousVelocity.magnitude, 0.0f);
        }
    }
}
