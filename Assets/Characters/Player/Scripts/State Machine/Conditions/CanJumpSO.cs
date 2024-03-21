using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Can Jump", menuName = "Player/Condition/Can Jump")]
    public class CanJumpSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return board.isGrounded && board.jump;
        }
    }
}
