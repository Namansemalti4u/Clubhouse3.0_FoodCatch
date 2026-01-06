using UnityEngine;

namespace Clubhouse.Games.FoodCatch.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
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
        void Update()
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
    }
}
