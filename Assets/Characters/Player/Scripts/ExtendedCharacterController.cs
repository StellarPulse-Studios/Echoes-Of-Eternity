using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class ExtendedCharacterController : MonoBehaviour
    {
        [SerializeField]
        private float stepDownOffset = 0.5f;

        private Transform m_Transform;
        private CharacterController m_CharacterController;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            m_Transform = transform;
        }

        private Vector3 m_RayHitPoint;
        private Vector3 m_SphereHitPoint;

        public Vector3 GetStepDownMotion(Vector3 motion)
        {
            Vector3 currPos = m_Transform.position;
            Vector3 nextPos = currPos + motion;

            const float rayOriginYOffset = 1.0f;
            Vector3 rayOrigin = new Vector3(nextPos.x, currPos.y + rayOriginYOffset, nextPos.z);

            Ray ray = new Ray(rayOrigin, Vector3.down);
            float maxRayLength = rayOriginYOffset + stepDownOffset;

            if (Physics.Raycast(ray, out RaycastHit hit, maxRayLength))
            {
                float yOffset = hit.point.y - currPos.y;
                if (yOffset < 0.0f)
                    motion.y += yOffset;

                m_RayHitPoint = hit.point;
            }

            return motion;
        }

        public CollisionFlags Move(Vector3 motion)
        {
            return m_CharacterController.Move(motion);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_CharacterController == null)
                m_CharacterController = GetComponent<CharacterController>();

            Gizmos.color = new Color(1.0f, 0.0f, 1.0f);
            Gizmos.DrawLine(transform.position, m_RayHitPoint);

            Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.5f);
            Gizmos.DrawSphere(m_RayHitPoint, 0.03f);

            Gizmos.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
            Gizmos.DrawSphere(m_SphereHitPoint, 0.03f);
        }
#endif
    }
}
