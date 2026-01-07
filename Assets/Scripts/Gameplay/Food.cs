using Clubhouse.Games.Common;
using Clubhouse.Games.FoodCatch.Core;
using UnityEngine;

public class Food : MonoBehaviour
{

    private Rigidbody2D rb;
    private Timer despawnTimer;

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
        rb.linearVelocity = Vector2.zero;
        float forceX = Random.Range(1f, 2);
        float forceY = Random.Range(1f, 3);
        rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                Stop();
                GameManagerFC.Instance.AddScore(GameManagerFC.DROP);
                break;
            case "Finish":
                Stop();
                GameManagerFC.Instance.AddScore(GameManagerFC.CAUGHT);
                break;
            case "Player":
                {
                    switch (foodType)
                    {
                        case FoodType.Edible:
                            {
                                float forceX = Random.Range(0.8f, 1);
                                rb.AddForce(new Vector2(forceX, 1), ForceMode2D.Impulse);
                                break;
                            }
                        case FoodType.Inedible:
                            GameManagerFC.Instance.AddScore(GameManagerFC.WRONG);
                            despawnTimer.Enable();
                            break;
                    }
                    break;
                }            
        }
    }

    private void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        despawnTimer.Enable();
    }
}