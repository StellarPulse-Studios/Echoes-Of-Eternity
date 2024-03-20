using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Is Animation State Machine Exited", menuName = "Player/Condition/Is Animation State Machine Exited")]
    public class IsExitedFromAnimationStateMachineSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return board.isAnimationStateMachineExited;
        }
    }
}
