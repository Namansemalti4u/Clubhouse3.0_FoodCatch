using Clubhouse.Games.FoodCatch.Core;
using UnityEngine;

namespace Clubhouse.Games.FoodCatch.Gameplay
{
    public class PlayerController : Helper.Singleton<PlayerController>
    {
        private const string IsMoving = "isMoving";

        [SerializeField]
        private float maxSpeed, boundValue;
        private Animator animController;

        private float currentSpeed;
        private float lerpRate;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animController = GetComponent<Animator>();
            lerpRate = 0.1f;
        }

        // Update is called once per frame
        void LateUpdate()
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.A))
            {
                MovementLogic(-1);
                animController.SetBool(IsMoving, true);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                MovementLogic(1);
                animController.SetBool(IsMoving, true);
            }
            else
#endif
                TouchHandling();
        }

        #region Touch Methods
        private void TouchHandling()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                MovementLogic(touch.position.x < Screen.width / 2 ? -1 : 1);
                animController.SetBool(IsMoving, true);
            }
            else
            {
                currentSpeed = 0f;
                animController.SetBool(IsMoving, false);
            }
        }
        #endregion

        private void MovementLogic(int multiplier)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x += GetSpeed() * multiplier * Time.deltaTime;
            currentPosition.x = Mathf.Clamp(currentPosition.x, -boundValue, boundValue);
            transform.localScale = new Vector3(multiplier == -1 ? 1 : -1, 1, 1);
            transform.position = currentPosition;
        }

        private float GetSpeed()
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, lerpRate);
            return currentSpeed;
        }

        public Vector2 LaunchFood(Food food, bool onEdge)
        {
            if (CanBounceFromEdge(onEdge) || IsSafePointNear())
            {
                // Coroutine to scale Food to half when bouncing off in 1.2 seconds.
                StartCoroutine(ScaleOverTime(food, 0.7f, 2));

                Vector2 start = food.transform.position;
                Vector2 target = LevelManager.Instance.safePoint.position;
                float angle = 70;
                return CalculateImpulseForce(food.rb, start, target, angle);
            }
            return new Vector2(1.25f, 4.5f);
        }

        private System.Collections.IEnumerator ScaleOverTime(Food food, float scale, float duration)
        {
            float currentTime = 0.0f;
            Vector3 originalScale = food.transform.localScale;
            Vector3 targetScale = Vector3.one * scale;
            while (currentTime < duration)
            {
                food.transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            }
            food.transform.localScale = targetScale;
        }

        private bool IsSafePointNear()
        {
            return transform.position.x > boundValue * 0.6f;
        }

        private bool CanBounceFromEdge(bool onEdge)
        {
            return (onEdge && transform.position.x > boundValue * 0.1f);
        }

        public static Vector2 CalculateImpulseForce(Rigidbody2D rb, Vector2 start, Vector2 target, float angleDegrees)
        {
            float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float theta = angleDegrees * Mathf.Deg2Rad;

            float D = target.x - start.x;
            float H = target.y - start.y;

            float cos = Mathf.Cos(theta);
            float tan = Mathf.Tan(theta);

            float speed = Mathf.Sqrt((gravity * D * D) / (2f * cos * cos * (D * tan - H)));
            Vector2 velocity = new(speed * cos, speed * Mathf.Sin(theta));
            return rb.mass * velocity;
        }
    }
}
