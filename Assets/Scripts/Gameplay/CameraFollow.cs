using UnityEngine;


namespace Clubhouse.Games.FoodCatch.Gameplay
{
    public class CameraFollowX : MonoBehaviour
    {
        public Transform player;
        public float moveThreshold = 0.1f; // How much player must move
        public float smoothSpeed = 5f;

        public float minX = -10f;
        public float maxX = 10f;

        private float lastPlayerX;

        void Start()
        {
            lastPlayerX = player.position.x;
        }

        void LateUpdate()
        {
            float currentPlayerX = player.position.x;
            float deltaX = currentPlayerX - lastPlayerX;

            // If player moved more than threshold
            if (Mathf.Abs(deltaX) >= moveThreshold)
            {
                float targetX = transform.position.x + deltaX;

                // Clamp camera position
                targetX = Mathf.Clamp(targetX, minX, maxX);

                Vector3 targetPosition = new Vector3(
                    targetX,
                    transform.position.y,
                    transform.position.z
                );

                transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    smoothSpeed * Time.deltaTime
                );

                // Update last player position
                lastPlayerX = currentPlayerX;
            }
        }
    }

}


