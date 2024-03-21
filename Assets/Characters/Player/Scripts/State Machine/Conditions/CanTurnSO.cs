using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Can Turn", menuName = "Player/Condition/Can Turn")]
    public class CanTurnSO : ConditionSO
    {
        public float turnAngleThreshold = 45.0f;
        public override bool Evaluate(Blackboard board)
        {
            if (board.move == Vector2.zero || !Mathf.Approximately(board.Velocity.magnitude, 0.0f))
                return false;

            Quaternion cameraYRot = Quaternion.Euler(0.0f, board.cameraTransform.eulerAngles.y, 0.0f);
            Vector3 inputDir = new Vector3(board.move.x, 0.0f, board.move.y).normalized;
            Vector3 inputDirRelativeToCameraSpace = cameraYRot * inputDir;

            Vector3 cameraForward = cameraYRot * Vector3.forward;
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);

            float turnDir = Mathf.Sign(Vector3.Dot(board.playerTransform.right, inputDirRelativeToCameraSpace));
            float turnAngle = Vector3.Angle(board.playerTransform.forward, inputDirRelativeToCameraSpace);
            board.turnAngle = turnAngle * turnDir;

            board.targetRotation = Quaternion.LookRotation(inputDirRelativeToCameraSpace, Vector3.up);

            // Camera view axis
            Debug.DrawRay(board.playerTransform.position + Vector3.up, cameraForward, Color.blue);
            Debug.DrawRay(board.playerTransform.position + Vector3.up, cameraRight, Color.red);
            Debug.DrawRay(board.playerTransform.position + Vector3.up, Vector3.up, Color.green);

            Debug.DrawRay(board.playerTransform.position + Vector3.up, inputDirRelativeToCameraSpace, Color.magenta);

            return turnAngle >= turnAngleThreshold;
        }
    }
}
