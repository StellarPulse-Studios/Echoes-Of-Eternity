using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Transition
    {
        public ConditionSO condition;
        public StateSO to;
    }
}
