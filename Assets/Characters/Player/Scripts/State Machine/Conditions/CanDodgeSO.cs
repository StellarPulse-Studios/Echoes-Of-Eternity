using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Can Dodge", menuName = "Player/Condition/Can Dodge")]
    public class CanDodgeSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return board.dodge;
        }
    }
}
