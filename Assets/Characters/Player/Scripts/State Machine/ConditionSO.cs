using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class ConditionSO : ScriptableObject
    {
        public abstract bool Evaluate(Blackboard board);
    }
}
