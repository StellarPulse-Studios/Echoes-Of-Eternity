using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Dodge", menuName = "Player/State/Unarmed/Dodge")]
    public class UnarmedDodgeSO : StateSO
    {
        public bool snapToGround;
        public float snapSpeed = 1.0f;

        private bool m_IsSnappingCompleted;
        private Vector3 m_SnapPosition;

        public override void OnEnter(Blackboard board)
        {
            board.isAnimationCompleted = false;

            if (snapToGround)
            {
                m_IsSnappingCompleted = false;
                m_SnapPosition = GetSnapPosition(board);
                board.Velocity = Vector3.zero;
            }
            else
            {
                board.animator.applyRootMotion = true;
                board.animator.SetTrigger("Roll");
            }
        }

        public override void OnExit(Blackboard board)
        {
            board.animator.applyRootMotion = false;
            board.isAnimationCompleted = false;
            board.isCrouched = false;
            board.jump = false;
            board.dodge = false;
        }

        public override void OnUpdate(Blackboard board)
        {
            if (snapToGround && !m_IsSnappingCompleted)
                SnapToGround(board);
        }

        public override void DrawGizmos(Blackboard board)
        {
            base.DrawGizmos(board);

            Gizmos.color = new Color(1.0f, 0.0f, 1.0f, 0.5f);
            Gizmos.DrawSphere(m_SnapPosition, 0.03f);

            Gizmos.color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
            Gizmos.DrawLine(board.playerTransform.position, m_SnapPosition);
        }

        private Vector3 GetSnapPosition(Blackboard board)
        {
            Vector3 currPos = board.playerTransform.position;
            Ray ray = new Ray(currPos + Vector3.up * 0.5f, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, board.groundLayer))
                currPos.y = hit.point.y;

            return currPos;
        }

        private void SnapToGround(Blackboard board)
        {
            board.Velocity.y = -snapSpeed;
            board.characterController.Move(board.Velocity * Time.deltaTime);

            if (board.isGrounded)
            {
                m_IsSnappingCompleted = true;
                board.animator.applyRootMotion = true;
                board.animator.SetTrigger("Roll");
            }
        }
    }
}
