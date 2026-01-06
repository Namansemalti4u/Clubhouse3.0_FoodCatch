using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] 
    private FoodType foodType;
    enum FoodType
    {
        Edible,
        Inedible
    }

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method for object pooling initialization
    /// </summary>
    public void Init()
    {
        // Force calculation for forward jump motion.
        rb.linearVelocity = Vector2.zero;
        float forceX = Random.Range(2f, 5f);
        float forceY = Random.Range(5f, 7f);
        rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
    }

}
