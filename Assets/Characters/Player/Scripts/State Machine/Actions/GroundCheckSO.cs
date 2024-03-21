using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Ground Check", menuName = "Player/Action/Unarmed/Ground Check")]
    public class GroundCheckSO : ActionSO
    {
        public float secondGroundCheckMaxYDiff = 1.0f;
        public Color firstPassColor;
        public Color secondPassColor;

        private Color groundCheckColor;

        public override void Evaluate(Blackboard board)
        {
            CheckForGround(board);
        }

        private void CheckForGround(Blackboard board)
        {
            CharacterController controller = board.characterController;

            float radius = controller.radius + controller.skinWidth + board.groundCheckRadiusOffset;
            float yOffset = controller.height * 0.5f - controller.radius;
            Vector3 localOffset = controller.center + new Vector3(0.0f, board.groundCheckYOffset - yOffset, 0.0f);
            Vector3 position = board.playerTransform.position + board.playerTransform.TransformVector(localOffset);

            bool isGrounded = Physics.CheckSphere(position, radius, board.groundLayer, QueryTriggerInteraction.Ignore);

            if (isGrounded)
                groundCheckColor = firstPassColor;

            // If currently not grounded but previously grounded, then check for ground with a sphere-cast
            // If nearby ground found within a certain threshold, then grounded is true.
            if (!isGrounded && board.isGrounded)
            {
                Ray ray = new Ray(position + Vector3.up, Vector3.down);
                if (Physics.SphereCast(ray, radius * 0.5f, out RaycastHit hit))
                {
                    float yDiff = board.playerTransform.position.y - hit.point.y;
                    if (yDiff <= secondGroundCheckMaxYDiff)
                        isGrounded = true;

                    if (isGrounded)
                        groundCheckColor = secondPassColor;
                }
            }

            board.isGrounded = isGrounded;
        }

        public override void DrawGizmos(Blackboard board)
        {
            CharacterController controller = board.characterController;

            float radius = controller.radius + controller.skinWidth + board.groundCheckRadiusOffset;
            float yOffset = controller.height * 0.5f - controller.radius;
            Vector3 localOffset = controller.center + new Vector3(0.0f, board.groundCheckYOffset - yOffset, 0.0f);
            Vector3 position = board.playerTransform.position + board.playerTransform.TransformVector(localOffset);

            Gizmos.color = board.isGrounded ? groundCheckColor : new Color(1.0f, 0.0f, 0.0f, 0.5f);
            Gizmos.DrawSphere(position, radius);
        }
    }
}
