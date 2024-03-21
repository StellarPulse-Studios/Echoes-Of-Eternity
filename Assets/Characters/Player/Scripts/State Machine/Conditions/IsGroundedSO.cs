using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Is Grounded", menuName = "Player/Condition/Is Grounded")]
    public class IsGroundedSO : ConditionSO
    {
        public bool compareValue = false;

        public override bool Evaluate(Blackboard board)
        {
            return board.isGrounded == compareValue;
        }
    }
}
