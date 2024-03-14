using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class StateSO : ScriptableObject
    {
        public List<Transition> transitions;
        public List<ActionSO> actions;

        public abstract void OnEnter(Blackboard board);
        public abstract void OnExit(Blackboard board);
        public abstract void OnUpdate(Blackboard board);

        public virtual void DrawGizmos(Blackboard board)
        {
            foreach (var action in actions)
                if (action != null)
                    action.DrawGizmos(board);
        }
    }
}
