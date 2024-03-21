using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Heavy Attack", menuName = "Player/State/Armed/Heavy Attack")]
    public class HeavyAttackSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.PreviousVelocity = Vector3.zero;
            board.PreviousSpeed = 0.0f;
            board.attack = false;
            board.heavyAttack = false;
            board.animator.SetTrigger("HeavyAttack");
        }

        public override void OnExit(Blackboard board)
        {
            board.isAnimationStateMachineExited = false;
        }

        public override void OnUpdate(Blackboard board)
        {
            if (board.heavyAttack)
            {
                board.heavyAttack = false;
                board.animator.SetTrigger("HeavyAttack");
            }
        }
    }
}
