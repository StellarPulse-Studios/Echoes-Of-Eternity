using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Is Animation Completed", menuName = "Player/Condition/Is Animation Completed")]
    public class IsAnimationCompletedSO : ConditionSO
    {
        public bool compareValue;

        public override bool Evaluate(Blackboard board)
        {
            return board.isAnimationCompleted == compareValue;
        }
    }
}
