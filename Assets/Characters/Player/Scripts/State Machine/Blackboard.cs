using UnityEngine;

namespace Player
{
    public class Blackboard : MonoBehaviour
    {
        [Header("Physics")]
        public float gravity = -9.81f;
        public float acceleration = 10.0f;
        public float angularSpeed = 240.0f;
        public float walkSpeed = 2.0f;
        public float runSpeed = 4.0f;
        public float sprintSpeed = 6.0f;
        public float jumpHeight = 2.0f;
        public float totalFallTimeToBecomeUnstable = 1.0f;
        public float crouchWalkSpeed = 2.047f;

        [Header("Ground Check")]
        public float groundCheckRadiusOffset = 0.01f;
        public float groundCheckYOffset = 0.0f;
        public LayerMask groundLayer;

        [Header("Components")]
        public Transform cameraTransform;
        public Transform playerTransform;
        public CharacterController characterController;
        public ExtendedCharacterController extendedCharacterController;
        public Animator animator;

        [Header("Inputs")]
        public Vector2 move;
        public Vector2 look;
        public bool sprint;
        public bool jump;
        public bool dodge;

        [Header("State Variables")]
        public bool isGrounded;
        public bool isAnimationCompleted;
        public float turnAngle;
        public Quaternion targetRotation;
        public float fallingTime;
        public bool isCrouched;

        [Header("Debug Variables")]
        public float CurrentSpeed;
        public Vector3 Velocity;
        public Vector3 PreviousVelocity;
        public float PreviousSpeed;
    }
}