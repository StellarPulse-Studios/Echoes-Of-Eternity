using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Free Fall", menuName = "Player/State/Unarmed/Free Fall")]
    public class UnarmedFreeFallSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.animator.SetBool("IsGrounded", false);
            board.animator.SetTrigger("FreeFall");
        }

        public override void OnExit(Blackboard board)
        {
            board.animator.SetBool("IsGrounded", true);
            //board.animator.SetFloat("MoveSpeed", 0.0f);
        }

        public override void OnUpdate(Blackboard board)
        {
            if (board.Velocity.y < 0.0f)
                board.fallingTime += Time.deltaTime;

            board.Velocity.y += board.gravity * Time.deltaTime;
            board.characterController.Move(board.Velocity * Time.deltaTime);
        }
    }
}
