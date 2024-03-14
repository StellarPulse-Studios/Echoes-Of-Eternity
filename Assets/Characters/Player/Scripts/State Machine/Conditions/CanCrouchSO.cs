using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Can Crouch", menuName = "Player/Condition/Can Crouch")]
    public class CanCrouchSO : ConditionSO
    {
        public bool crouching;

        public override bool Evaluate(Blackboard board)
        {
            return board.isCrouched == crouching;
        }
    }
}
