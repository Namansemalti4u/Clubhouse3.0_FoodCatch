using UnityEngine;

public class Food : MonoBehaviour
{
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
    public void Init(float a_position, Sprite a_sprite, PolygonCollider2D a_collider)
    {
        transform.position = new Vector2(a_position, transform.position.y);
        GetComponent<SpriteRenderer>().sprite = a_sprite;
        GetComponent<PolygonCollider2D>().points = a_collider.points;
        float randomScale = Random.Range(0.6f, 0.8f);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

}
