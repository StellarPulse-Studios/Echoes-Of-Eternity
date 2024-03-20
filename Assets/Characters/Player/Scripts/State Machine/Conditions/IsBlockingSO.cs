using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Is Blocking", menuName = "Player/Condition/Is Blocking")]
    public class IsBlockingSO : ConditionSO
    {
        public bool compareValue;

        public override bool Evaluate(Blackboard board)
        {
            return board.block == compareValue;
        }
    }
}
