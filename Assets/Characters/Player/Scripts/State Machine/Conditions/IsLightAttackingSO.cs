using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Is Light Attacking", menuName = "Player/Condition/Is Light Attacking")]
    public class IsLightAttackingSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return board.lightAttack;
        }
    }
}
