using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Ground Check While Jump", menuName = "Player/Action/Unarmed/Ground Check While Jump")]
    public class GroundCheckWhileJumpSO : ActionSO
    {
        public GroundCheckSO groundCheck;

        public override void Evaluate(Blackboard board)
        {
            if (board.Velocity.y <= 0.0f)
                groundCheck.Evaluate(board);
        }
    }
}
