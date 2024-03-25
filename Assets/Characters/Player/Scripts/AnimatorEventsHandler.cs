using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class AnimatorEventsHandler : MonoBehaviour
    {
        public float m_ChargeMultiplier = 0.1f;
        public BoxCollider m_BoxCollider;
        public Blackboard m_Blackboard;
        public LayerMask m_HitBoxLayer;
        public float m_MoveSpeed = 2.0f;

        private HashSet<Collider> m_ColliderSet;
        private bool m_IsBoxCasting;
        private bool m_CanMoveTowardTarget;

        private void Start()
        {
            m_ColliderSet = new HashSet<Collider>();
            m_IsBoxCasting = false;
        }

        public void EnableHitBox()
        {
            m_ColliderSet.Clear();
            m_IsBoxCasting = true;
        }

        public void DisableHitBox()
        {
            m_IsBoxCasting = false;
        }

        public void EnableCharging()
        {
            m_Blackboard.hasStartedCharging = true;

            if (m_Blackboard.isCharging)
            {
                m_Blackboard.animator.SetFloat("ChargeMultiplier", m_ChargeMultiplier);
            }
        }

        public void DisableCharging()
        {
            m_Blackboard.hasStartedCharging = false;

            if (m_Blackboard.isCharging)
            {
                m_Blackboard.isCharged = true;
                m_Blackboard.animator.SetFloat("ChargeMultiplier", 0.0f);
            }
            else
            {
                m_Blackboard.animator.SetFloat("ChargeMultiplier", 1.0f);
            }
        }

        public void EnableMoving()
        {
            m_CanMoveTowardTarget = true;
            //UnityEditor.EditorApplication.isPaused = true;
        }

        public void DisableMoving()
        {
            m_CanMoveTowardTarget = false;
        }

        private void MoveTowardTarget()
        {
            if (m_Blackboard.closestEnemy && Vector3.Distance(m_Blackboard.playerTransform.position, m_Blackboard.closestEnemy.position) <= m_Blackboard.enemyMinRangeThreshold)
                return;

            Vector3 motion = m_Blackboard.extendedCharacterController.GetStepDownMotion(Time.deltaTime * m_MoveSpeed * m_Blackboard.transform.forward);
            m_Blackboard.characterController.Move(motion);
        }

        private void Update()
        {
            if (m_CanMoveTowardTarget)
            {
                MoveTowardTarget();
            }

            if (m_IsBoxCasting)
            {
                DoDamage();
            }
        }

        private void DoDamage()
        {
            Vector3 center = m_BoxCollider.transform.TransformPoint(m_BoxCollider.center);
            Collider[] colliders = Physics.OverlapBox(center, m_BoxCollider.size * 0.5f, m_BoxCollider.transform.rotation, m_HitBoxLayer);

            foreach (Collider collider in colliders)
            {
                if (m_ColliderSet.Contains(collider))
                    continue;

                m_ColliderSet.Add(collider);

                if (collider.TryGetComponent(out IDamagable damagable))
                    damagable.OnDamage();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
            Matrix4x4 prevMatrix = Gizmos.matrix;
            Gizmos.matrix = m_BoxCollider.transform.localToWorldMatrix;
            Gizmos.DrawCube(m_BoxCollider.center, m_BoxCollider.size);
            Gizmos.matrix = prevMatrix;
        }
    }
}
