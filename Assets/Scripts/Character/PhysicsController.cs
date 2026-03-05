using UnityEngine;

// TODO: Consider the benefits of refactoring to namespace Movement
namespace Character
{
    public class PhysicsMovement : MovementController
    {
        [SerializeField] float drag = 0.5f;
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] float JumpForce = 5f;
        bool jump = false;

        // NEW: camera reference for camera-relative movement
        [SerializeField] private Transform cameraTransform;

        protected override void Start()
        {
            base.Start();
            rb.linearDamping = drag;

            TryResolveCamera();
        }

        private void OnEnable()
        {
            // When scene changes, this gets called again (good time to re-resolve camera)
            TryResolveCamera();
        }

        private void TryResolveCamera()
        {
            // If assigned in inspector, keep it
            if (cameraTransform != null) return;

            // Prefer MainCamera tag
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
                return;
            }

            // Fallback: any camera in the scene
            Camera anyCam = FindFirstObjectByType<Camera>();
            if (anyCam != null)
            {
                cameraTransform = anyCam.transform;
                return;
            }

            Debug.LogWarning("[PhysicsMovement] No camera found. Movement will fallback to world-relative until a camera exists.");
        }

        public override float GetHorizontalSpeedPercent()
        {
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            return Mathf.Clamp01(horizontalVelocity.magnitude / maxVelocity);;
        }

        public override void Jump()
        {
            // TODO: integrate jump support from week 2-3
            jump = true;
        }

        protected override void FixedUpdate()
        {
            //base.FixedUpdate(); // TODO: remove base.FixedUpdate() when starting your integration
            ApplyMovement();
            ClampVelocity();
            ApplyRotation();
            ApplyJump();
            //Debug.Log(transform.rotation.eulerAngles);
        }

        void ApplyMovement()
        {
            // TODO integrate your physics from week 2-3 

            // If camera is missing, fall back to world-based movement (won't crash)
            if (cameraTransform == null)
            {
                Vector3 movementFallback = new Vector3(moveInput.x, 0, moveInput.y);
                if (movementFallback.sqrMagnitude > 1f) movementFallback.Normalize();
                rb.AddForce(movementFallback * acceleration, ForceMode.Acceleration);
                return;
            }

            // Camera-relative movement:
            // 1) Flatten camera forward/right so we don't move into the sky/ground
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            // 2) Convert input into world direction relative to camera
            Vector3 movement = (camRight * moveInput.x) + (camForward * moveInput.y);

            // Normalize so diagonal isn't faster
            if (movement.sqrMagnitude > 1f)
                movement.Normalize();

            rb.AddForce(movement * acceleration, ForceMode.Acceleration);
        }

        void ApplyJump()
        {
            if (checkIsGrounded() && jump)
            {
                Debug.Log("JumpTriggered via ApplyJump");
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }

            jump = false;

            // TODO integrate your jump logic from week 2-3
        }

        public bool checkIsGrounded()
        {
            CapsuleCollider capsule = GetComponent<CapsuleCollider>();

            Vector3 bottom = capsule.bounds.center - new Vector3(0, capsule.bounds.extents.y, 0);
            Vector3 origin = bottom + Vector3.up * 0.05f;

            float rayLength = 0.1f; // just enough to detect ground contact

            return Physics.Raycast(
                origin,
                Vector3.down,
                rayLength,
                ~0,
                QueryTriggerInteraction.Ignore
            );
        }

        // TODO integrate collision support from week 2-3 

        void ClampVelocity()
        {
            // Clamp horizontal velocity while preserving vertical (for jumping/falling)
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (horizontalVelocity.magnitude > maxVelocity)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxVelocity;
                rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
            }
        }

        void ApplyRotation()
        {
            // Rotate to face movement direction (camera-relative)
            if (cameraTransform == null) return;

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 direction = (camRight * moveInput.x) + (camForward * moveInput.y);

            if (direction.magnitude > 0.5f)
            {
                // 1. Calculate the target rotation (where we WANT to look)
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

                // 2. Smoothly rotate from our current rotation toward the target
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                );
            }
        }
    }
}