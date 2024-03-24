using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Block", menuName = "Player/State/Armed/Block")]
    public class BlockSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.PreviousVelocity = Vector3.zero;
            board.PreviousSpeed = 0.0f;
            board.animator.SetBool("Block", true);
            board.shield.SetActive(true);
        }

        public override void OnExit(Blackboard board)
        {
            board.attack = false;
            board.lightAttack = false;
            board.heavyAttack = false;
            board.jump = false;
            //board.shield.SetActive(false);
            board.animator.SetBool("Block", false);
        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
