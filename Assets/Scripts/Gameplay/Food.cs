using UnityEngine;

public class Food : MonoBehaviour
{
    enum FoodType
    {
        Edible,
        Inedible
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
        //transform.position = new Vector2(a_position, transform.position.y);
        //GetComponent<SpriteRenderer>().sprite = a_sprite;
        //GetComponent<PolygonCollider2D>().points = a_collider.points;
        // Forward Jump force.

    }

}
