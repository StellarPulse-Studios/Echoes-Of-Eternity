using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Can Move And Crouch", menuName = "Player/Condition/Can Move And Crouch")]
    public class CanMoveAndCrouchSO : ConditionSO
    {
        public bool crouching;

        public override bool Evaluate(Blackboard board)
        {
            return board.isCrouched == crouching && !Mathf.Approximately(board.Velocity.magnitude, 0.0f);
        }
    }
}
