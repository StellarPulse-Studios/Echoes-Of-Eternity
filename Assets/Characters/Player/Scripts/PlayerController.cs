using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(ExtendedCharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public LayerMask groundLayer;
        public float groundCheckRadiusOffset = 0.01f;
        public float groundCheckYOffset = 0.0f;

        public float walkSpeed = 1.8f;
        public float runSpeed = 4.6f;
        public float sprintSpeed = 6.4f;
        public float angularSpeed = 120.0f;

        public float gravity = -9.81f;
        public float acceleration = 10.0f;
        public float jumpHeight = 1.0f;

        public Transform cameraTransform;
        public PlayerInputController input;
        public Transform graphics;
        public Animator animator;

        [Header("Debug Variables")]
        public float CurrentSpeed;

        private bool m_IsGrounded;
        private bool m_IsJumping;
        private Vector3 m_Velocity;
        private Transform m_Transform;
        private CharacterController m_CharacterController;
        private ExtendedCharacterController m_ExtendedCharacterController;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_ExtendedCharacterController = GetComponent<ExtendedCharacterController>();
        }

        private void Start()
        {
            m_Transform = transform;
        }

        private Vector3 m_SphereHitPoint;

        private void Update()
        {
            CheckForGround();

            Vector3 pos = m_Transform.position;

            if (m_IsGrounded)
            {
                GroundMovement();

                if (!m_IsJumping && input.jump)
                {
                    m_IsJumping = true;

                    float y = -Mathf.Sign(gravity) * Mathf.Sqrt(2.0f * Mathf.Abs(gravity) * jumpHeight);
                    m_Velocity.y += y;

                    animator.SetTrigger("Jump");
                }
            }
            else
            {
                m_Velocity.y += gravity * Time.deltaTime;
                m_ExtendedCharacterController.Move(m_Velocity * Time.deltaTime);

                //m_IsGrounded = true;
                //GroundMovement();
            }

            animator.SetBool("IsGrounded", m_IsGrounded);

            if (graphics)
                graphics.SetPositionAndRotation(pos, m_Transform.rotation);

            input.jump = false;
        }

        private void GroundMovement()
        {
            Vector2 moveInput = input.move;
            float moveInputMagnitude = moveInput.magnitude;

            Vector3 moveDir = (moveInputMagnitude > 1E-05f) ? (new Vector3(moveInput.x, 0.0f, moveInput.y) / moveInputMagnitude) : Vector3.zero;

            Vector3 currentVelocity = m_Velocity;
            float currentSpeed = currentVelocity.magnitude;
            float targetSpeed = GetTargetSpeed();

            CurrentSpeed = currentSpeed;

            // acceleration and deceleration
            if (currentSpeed < targetSpeed - 0.1f || currentSpeed > targetSpeed + 0.1f)
            {
                currentSpeed += Mathf.Sign(targetSpeed - currentSpeed) * acceleration * Time.deltaTime;
                currentSpeed = Mathf.Round(currentSpeed * 1000.0f) * 0.001f;
            }
            else
            {
                currentSpeed = targetSpeed;
            }

            if (moveInput != Vector2.zero)
            {
                float targetYRot = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                Quaternion targetRot = Quaternion.Euler(0.0f, targetYRot, 0.0f);
                m_Transform.rotation = Quaternion.RotateTowards(m_Transform.rotation, targetRot, angularSpeed * Time.deltaTime);
            }

            moveDir = Quaternion.Euler(0.0f, m_Transform.eulerAngles.y, 0.0f) * Vector3.forward;

            m_Velocity = moveDir * currentSpeed;

            Vector3 stepDownMotion = m_ExtendedCharacterController.GetStepDownMotion(Time.deltaTime * m_Velocity);
            m_ExtendedCharacterController.Move(stepDownMotion);

            animator.SetFloat("MoveSpeed", currentSpeed);
        }

        private float GetTargetSpeed()
        {
            float speed = 0.0f;

            if (input.move == Vector2.zero)
                return speed;

            speed = runSpeed;

            if (input.sprint)
                speed = sprintSpeed;

            return speed;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_CharacterController == null)
                m_CharacterController = GetComponent<CharacterController>();

            float radius = m_CharacterController.radius + m_CharacterController.skinWidth + groundCheckRadiusOffset;
            float yOffset = m_CharacterController.height * 0.5f - m_CharacterController.radius;
            Vector3 localOffset = m_CharacterController.center + new Vector3(0.0f, groundCheckYOffset - yOffset, 0.0f);
            Vector3 position = transform.position + transform.TransformVector(localOffset);

            Gizmos.color = m_IsGrounded ? new Color(0.0f, 1.0f, 0.0f, 0.5f) : new Color(1.0f, 0.0f, 0.0f, 0.5f);
            Gizmos.DrawSphere(position, radius);

            Gizmos.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
            Gizmos.DrawSphere(m_SphereHitPoint, radius * 0.5f);
        }
#endif

        private void CheckForGround()
        {
            float radius = m_CharacterController.radius + m_CharacterController.skinWidth + groundCheckRadiusOffset;
            float yOffset = m_CharacterController.height * 0.5f - m_CharacterController.radius;
            Vector3 localOffset = m_CharacterController.center + new Vector3(0.0f, groundCheckYOffset - yOffset, 0.0f);
            Vector3 position = m_Transform.position + m_Transform.TransformVector(localOffset);

            bool isGrounded = Physics.CheckSphere(position, radius, groundLayer, QueryTriggerInteraction.Ignore);

            // If currently not grounded but previously grounded, then check for ground with a sphere-cast
            // If nearby ground found within a certain threshold, then grounded is true.
            if (!isGrounded && m_IsGrounded)
            {
                Ray ray = new Ray(position + Vector3.up, Vector3.down);
                if (Physics.SphereCast(ray, radius * 0.5f, out RaycastHit hit))
                {
                    float yDiff = m_Transform.position.y - hit.point.y;
                    if (yDiff <= 1.0f)
                        isGrounded = true;
                }
            }

            //if (!m_IsGrounded && isGrounded)
            //    m_Velocity = Vector3.zero;

            m_IsGrounded = isGrounded;

            if (m_IsJumping && m_Velocity.y > 0.0f)
            {
                m_IsGrounded = false;
            }

            if (m_IsJumping && m_IsGrounded)
            {
                m_IsJumping = false;
            }

            if (!m_IsJumping && !m_IsGrounded)
                animator.SetBool("FreeFall", true);

            if (m_IsGrounded)
                animator.SetBool("FreeFall", false);
        }
    }
}
