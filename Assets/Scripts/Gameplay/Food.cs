using Clubhouse.Games.Common;
using Clubhouse.Games.FoodCatch.Core;
using UnityEngine;

public class Food : MonoBehaviour
{

    private Rigidbody2D rb;
    private Timer despawnTimer;

    [SerializeField]
    float forceX, forceY;

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
        forceX = Random.Range(1, 2.5f);
        forceY = Random.Range(4, 4.5f);
        BounceOffFromPlayer(new Vector2(forceX, forceY));
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
                            float forceX = Random.Range(1f, 2f);
                            float forceY = 4.5f;
                            var bounceForce = new Vector2(forceX, forceY);
                            BounceOffFromPlayer(bounceForce);
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

    public void BounceOffFromPlayer(Vector2 bounceForce)
    {
        forceX = bounceForce.x;
        forceY = bounceForce.y;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(bounceForce, ForceMode2D.Impulse);
    }

    private void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        despawnTimer.Enable();
    }
}