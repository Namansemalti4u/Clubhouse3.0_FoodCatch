using Clubhouse.Games.Common;
using Clubhouse.Games.FoodCatch.Core;
using Clubhouse.Games.FoodCatch.Gameplay;
using UnityEngine;

public class Food : MonoBehaviour
{

    public Rigidbody2D rb;
    private Timer despawnTimer;

    [SerializeField]
    Vector2 force;

    public FoodType foodType;
    public enum FoodType
    {
        Edible,
        Inedible
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetDespawnTimer();
    }

    private void SetDespawnTimer()
    {
        if (despawnTimer == null)
            despawnTimer = new Timer(2, Despawn);
        else
            despawnTimer.ResetTimer();
    }

    private void Despawn()
    {
        LevelManager.Instance.Despawn(this);
        despawnTimer.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        despawnTimer.Update(Time.deltaTime);
    }

    /// <summary>
    /// Method for object pooling initialization
    /// </summary>
    public void Init(Vector3 position, Sprite foodSprite)
    {
        transform.position = position;
        transform.GetComponent<SpriteRenderer>().sprite = foodSprite;
        despawnTimer.ResetTimer();

        // Force calculation for forward jump motion.
        rb.constraints = RigidbodyConstraints2D.None;
        BounceOffFromPlayer(Random.Range(0.6f, 1.25f), 4.5f);
        rb.angularVelocity = -Random.Range(75, 100);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Ground":
                Stop();
                if (foodType == FoodType.Edible)
                    GameManagerFC.Instance.AddScore(GameManagerFC.DROP);
                break;
            case "Finish":
                rb.linearVelocity = Vector2.zero;
                StartCoroutine(MoveToTarget());
                GameManagerFC.Instance.AddScore(GameManagerFC.CAUGHT);
                break;
            case "Player":
                switch (foodType)
                {
                    case FoodType.Edible:
                        {
                            // Check if contact with player was at the edge of its collider
                            Bounds collBounds = collider.bounds;
                            bool onEdge = transform.position.x >= collBounds.max.x - 0.1f;
                            force = collider.gameObject.GetComponent<PlayerController>().LaunchFood(this, onEdge);
                            BounceOffFromPlayer(force.x, force.y);
                            break;
                        }
                    case FoodType.Inedible:
                        if (rb.linearVelocity != Vector2.zero)
                        {
                            GameManagerFC.Instance.AddScore(GameManagerFC.WRONG);
                            despawnTimer.Enable();
                        }
                        break;
                }
                break;
        }
    }

    System.Collections.IEnumerator MoveToTarget()
    {
        while (Vector2.Distance(transform.position, LevelManager.Instance.safePoint.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, LevelManager.Instance.safePoint.position, 5 * Time.deltaTime);
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