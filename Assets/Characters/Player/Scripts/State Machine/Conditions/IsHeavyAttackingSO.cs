using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Is Heavy Attacking", menuName = "Player/Condition/Is Heavy Attacking")]
    public class IsHeavyAttackingSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return board.heavyAttack;
        }
    }
}
