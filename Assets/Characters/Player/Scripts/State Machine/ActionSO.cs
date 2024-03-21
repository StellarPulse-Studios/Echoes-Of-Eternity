using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class ActionSO : ScriptableObject
    {
        public abstract void Evaluate(Blackboard board);

        public virtual void DrawGizmos(Blackboard board) { }
    }
}
