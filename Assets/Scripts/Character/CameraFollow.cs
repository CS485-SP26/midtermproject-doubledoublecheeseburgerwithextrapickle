using UnityEngine;

namespace Character 
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -5f);
        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private float rotationSpeed = 5f;

        void Start()
        {
            Debug.Assert(player != null, "CameraFollow requires a player Transform.");
        }

        void LateUpdate()
        {
            if (player == null) return;

            // Desired camera position behind player
            Vector3 desiredPosition = player.position + player.TransformDirection(offset);

            // Smooth follow
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // Look at player
            Quaternion desiredRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }
}