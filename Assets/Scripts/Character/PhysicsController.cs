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
        
        protected override void Start()
        {
            base.Start();
            rb.linearDamping = drag;

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
            Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
            
            movement.Normalize();

            rb.AddForce(movement * acceleration, ForceMode.Acceleration);


        }

        void ApplyJump()
        {
            
            if(checkIsGrounded() && jump)
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
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
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
