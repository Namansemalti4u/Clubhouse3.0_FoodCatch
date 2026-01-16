using Clubhouse.Games.FoodCatch.Core;
using UnityEngine;
using static Food;

namespace Clubhouse.Games.FoodCatch.Gameplay
{
    public class PlayerController : Helper.Singleton<PlayerController>
    {
        [SerializeField]
        private float maxSpeed, boundValue;
        private SpriteRenderer spriteRenderer;

        private float currentSpeed;
        private float lerpRate;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            spriteRenderer = transform.GetComponent<SpriteRenderer>();
            lerpRate = 0.1f;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            TouchHandling();
        }

        #region Touch Methods
        private void TouchHandling()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                MovementLogic(touch.position.x < Screen.width / 2 ? -1 : 1);
            }
            else
            {
                currentSpeed = 0f;
            }
        }
        #endregion

        private void MovementLogic(int multiplier)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x += GetSpeed() * multiplier * Time.deltaTime;
            //Clamp x-axis in a range.
            currentPosition.x = Mathf.Clamp(currentPosition.x, -boundValue, boundValue);
            //Change facing direction.
            spriteRenderer.flipX = multiplier == -1;
            transform.position = currentPosition;
        }

        private float GetSpeed()
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, lerpRate);
            return currentSpeed;
        }

        public Vector2 LaunchFood(Food food, bool onEdge)
        {
            if (transform.position.x > 1 && onEdge || transform.position.x > 7)
            {
                Vector2 start = food.transform.position;
                Vector2 target = LevelManager.Instance.safePoint.position;
                float angle = 60;
                float gravity = Mathf.Abs(Physics2D.gravity.y * food.rb.gravityScale);
                Debug.Log($"Launch from right edge to safe point. Start: {start}, Target: {target}, Angle: {angle}, Gravity: {gravity}");
                //return TargetVelocity(start, target, angle, gravity);
                return CalculateImpulseForce(food.rb, start, target, angle);
            }

            return new Vector2(Random.Range(1f, 2f), 4.5f);
        }

        private Vector2 TargetVelocity(Vector2 start, Vector2 target, float angleDegrees, float gravity)
        {
            float theta = angleDegrees * Mathf.Deg2Rad;

            float D = target.x - start.x;
            float H = target.y - start.y;

            float cos = Mathf.Cos(theta);
            float tan = Mathf.Tan(theta);

            float numerator = gravity * D * D;
            float denominator = 2f * cos * cos * (D * tan - H);
            float r = Mathf.Sqrt(numerator / denominator);

            float vx = r * cos;
            float vy = r * Mathf.Sin(theta);

            return new Vector2(vx, vy);
        }

        public static Vector2 CalculateImpulseForce(Rigidbody2D rb, Vector2 start, Vector2 target, float angleDegrees)
        {
            float gravity = -Physics2D.gravity.y;

            float theta = angleDegrees * Mathf.Deg2Rad;

            float D = target.x - start.x;
            float H = target.y - start.y;

            float cos = Mathf.Cos(theta);
            float tan = Mathf.Tan(theta);

            float speed = Mathf.Sqrt(
                (gravity * D * D) /
                (2f * cos * cos * (D * tan - H))
            );

            Vector2 velocity = new Vector2(
                speed * cos,
                speed * Mathf.Sin(theta)
            );

            // Impulse = mass * velocity
            return rb.mass * velocity;
        }


    }
}
