using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Will Attack", menuName = "Player/Condition/Will Attack")]
    public class IsArmedSO : ConditionSO
    {
        public bool compareValue;

        public override bool Evaluate(Blackboard board)
        {
            return board.isArmed == compareValue;
        }
    }
}
