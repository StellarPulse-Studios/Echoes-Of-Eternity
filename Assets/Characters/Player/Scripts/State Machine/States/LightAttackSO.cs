using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Light Attack", menuName = "Player/State/Armed/Light Attack")]
    public class LightAttackSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.PreviousVelocity = Vector3.zero;
            board.PreviousSpeed = 0.0f;
            board.attack = false;
            board.lightAttack = false;
            board.animator.SetTrigger("LightAttack");
        }

        public override void OnExit(Blackboard board)
        {
            board.isAnimationStateMachineExited = false;
        }

        public override void OnUpdate(Blackboard board)
        {
            if (board.lightAttack)
            {
                board.lightAttack = false;
                board.animator.SetTrigger("LightAttack");
            }
        }
    }
}
