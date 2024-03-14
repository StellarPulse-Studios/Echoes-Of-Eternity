using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Has Became Unstable", menuName = "Player/Condition/Has Became Unstable")]
    public class HasBecameUnstableSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return board.fallingTime >= board.totalFallTimeToBecomeUnstable;
        }
    }
}
