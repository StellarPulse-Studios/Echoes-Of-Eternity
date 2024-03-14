using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Can Quick Stop From Sprint", menuName = "Player/Condition/Can Quick Stop From Sprint")]
    public class CanQuickStopFromSprintSO : ConditionSO
    {
        public float quickStopThreshold = 4.5f;

        public override bool Evaluate(Blackboard board)
        {
            bool quickStop = board.move == Vector2.zero && board.Velocity.magnitude > quickStopThreshold;
            return quickStop;
        }
    }
}
