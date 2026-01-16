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
    public void Init()
    {
        // Force calculation for forward jump motion.
        rb.constraints = RigidbodyConstraints2D.None;
        force.Set(Random.Range(1, 2.5f), Random.Range(4, 4.5f));
        BounceOffFromPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Ground":
                Stop();
                Debug.Log("Food dropped!");
                GameManagerFC.Instance.AddScore(GameManagerFC.DROP);
                break;
            case "Finish":
                Stop();
                Debug.Log("Food saved!");
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
                            BounceOffFromPlayer();
                            break;
                        }
                    case FoodType.Inedible:
                        GameManagerFC.Instance.AddScore(GameManagerFC.WRONG);
                        despawnTimer.Enable();
                        Debug.Log("Inedible food caught!");
                        break;
                }
                break;
        }
    }

    public void BounceOffFromPlayer()
    {
        rb.linearVelocity = Vector2.zero;
        Debug.Log($"Applying force: {force}");
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        despawnTimer.Enable();
    }
}