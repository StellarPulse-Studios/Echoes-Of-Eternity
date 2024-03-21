using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WaitForSecondSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return false;
        }
    }
}
