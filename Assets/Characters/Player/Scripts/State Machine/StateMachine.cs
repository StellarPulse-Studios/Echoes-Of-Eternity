using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class StateMachine : MonoBehaviour
    {
        public StateSO startState;
        public Blackboard blackboard;

        [Header("Hide In Inspector Variables")]
        public StateSO currentState;


        private void Start()
        {
            currentState = startState;
            if (currentState)
                currentState.OnEnter(blackboard);
        }

        private void Update()
        {
            // Check for transition
            if (currentState)
            {
                List<Transition> transitions = currentState.transitions;
                foreach (Transition transition in transitions)
                {
                    if (transition.condition == null)
                        continue;

                    if (transition.condition.Evaluate(blackboard))
                    {
                        ChangeState(transition.to);
                        break;
                    }
                }
            }

            // Update current state and actions
            if (currentState)
            {
                List<ActionSO> actions = currentState.actions;
                foreach (ActionSO action in actions)
                {
                    if (action == null)
                        continue;

                    action.Evaluate(blackboard);
                }

                currentState.OnUpdate(blackboard);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            StateSO state = Application.isPlaying ? currentState : startState;
            if (state && blackboard)
                state.DrawGizmos(blackboard);
        }
#endif

        private void ChangeState(StateSO nextState)
        {
            if (currentState)
                currentState.OnExit(blackboard);

            currentState = nextState;

            if (currentState)
                currentState.OnEnter(blackboard);
        }
    }
}
