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

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closestEnemy = null;
            float minDistanceFromPlayer = float.MaxValue;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, board.playerTransform.position);
                if (distance < minDistanceFromPlayer )
                {
                    minDistanceFromPlayer = distance;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy != null && minDistanceFromPlayer <= board.enemyMaxRangeThreshold)
            {
                board.closestEnemy = closestEnemy.transform;

                Vector3 enemyDir = closestEnemy.transform.position - board.playerTransform.position;
                enemyDir.y = 0.0f;
                enemyDir.Normalize();
                board.playerTransform.rotation = Quaternion.LookRotation(enemyDir);
            }
            else
            {
                board.closestEnemy = null;
            }
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

        public override void DrawGizmos(Blackboard board)
        {
            base.DrawGizmos(board);

            Gizmos.color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
            Gizmos.DrawWireSphere(board.playerTransform.position, board.enemyMaxRangeThreshold);

            UnityEditor.Handles.color = new Color(1.0f, 0.0f, 1.0f, 0.05f);
            UnityEditor.Handles.DrawSolidArc(board.playerTransform.position, Vector3.up, board.playerTransform.forward, 360.0f, board.enemyMaxRangeThreshold);
        }
    }
}
