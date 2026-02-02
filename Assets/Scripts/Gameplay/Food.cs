using Clubhouse.Games.Common;
using Clubhouse.Games.FoodCatch.Core;
using Clubhouse.Games.FoodCatch.Gameplay;
using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{
    public Rigidbody2D rb;
    private Timer despawnTimer;
    private Vector2 force;
    private const float MAX_HEIGHT = 4.3f;
    private float maxUpwardVelocity;

    public FoodType foodType;
    public enum FoodType
    {
        Edible,
        Inedible
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        CalculateMaxUpwardVelocity();
        SetDespawnTimer();
    }

    private void SetDespawnTimer()
    {
        if (despawnTimer == null)
            despawnTimer = new Timer(2f, Despawn);
        else
            despawnTimer.ResetTimer();
    }

    private void Despawn()
    {
        LevelManager.Instance.Despawn(this);
        despawnTimer.Disable();
    }

    void Update()
    {
        despawnTimer.Update(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.y > maxUpwardVelocity)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxUpwardVelocity);
    }

    private void CalculateMaxUpwardVelocity()
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        maxUpwardVelocity = Mathf.Sqrt(2f * gravity * MAX_HEIGHT);
    }

    public void Init(Vector3 position, Sprite foodSprite)
    {
        transform.localScale = Vector3.one;
        transform.position = position;
        GetComponent<SpriteRenderer>().sprite = foodSprite;
        despawnTimer.ResetTimer();
        rb.constraints = RigidbodyConstraints2D.None;
        rb.linearVelocity = Vector2.zero;
        CalculateMaxUpwardVelocity();
        BounceOffFromPlayer(Random.Range(0.6f, 1.25f), 4.5f);
        rb.angularVelocity = -Random.Range(75f, 100f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "Ground":
                Stop();
                if (foodType == FoodType.Edible)
                    GameManagerFC.Instance.AddScore(GameManagerFC.DROP, transform);
                else
                    GameManagerFC.Instance.ResetStreakTimer();
                break;
            case "Finish":
                rb.linearVelocity = Vector2.zero;
                StartCoroutine(MoveToTarget());
                GameManagerFC.Instance.AddScore(GameManagerFC.CAUGHT, transform);
                GameManagerFC.Instance.ResetStreakTimer();
                break;
            case "Player":
                if (foodType == FoodType.Edible)
                {
                    Bounds bounds = collider.bounds;
                    bool onEdge = transform.position.x >= bounds.max.x - 0.1f;
                    Vector2 launchForce = collider.GetComponent<PlayerController>().LaunchFood(this, onEdge);
                    BounceOffFromPlayer(launchForce.x, launchForce.y);
                }
                else
                {
                    if (rb.linearVelocity != Vector2.zero)
                    {
                        GameManagerFC.Instance.AddScore(GameManagerFC.WRONG, transform);
                        despawnTimer.Enable();
                    }
                }
                break;
        }
    }

    IEnumerator MoveToTarget()
    {
        while (Vector2.Distance(transform.position, LevelManager.Instance.safePoint.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, LevelManager.Instance.safePoint.position, 5f * Time.deltaTime);
            yield return null;
        }
        Stop();
    }

    public void BounceOffFromPlayer(float x, float y)
    {
        rb.linearVelocity = Vector2.zero;
        force.Set(x, y);
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        despawnTimer.Enable();
    }
}
