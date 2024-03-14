using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Always True", menuName = "Player/Condition/Always True")]
    public class AlwaysTrueSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return true;
        }
    }
}
