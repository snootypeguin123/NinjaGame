using UnityEngine;

public class Hellorb : MonoBehaviour
{
    public float speed = 5f; // Adjust speed as needed
    private Vector2 direction;

    // Call this immediately after instantiating the hellorb
    public void SetDirection(Vector2 targetPosition)
    {
        Vector2 startPosition = transform.position;
        direction = (targetPosition - startPosition).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D: " + collision.tag);
        // Check if the collided object is the player
        if (collision.CompareTag("Player"))
        {
            // Deal 30 damage to the player if they have an Hp component
            Hp playerHp = collision.GetComponent<Hp>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(30);
            }
            else
            {
                Debug.LogWarning("Player object does not have an Hp component.");
            }

            // Destroy the hellorb after hitting the player
            Destroy(gameObject);
        }
        // Destroy on collision with ground
        else if (collision.CompareTag("tile"))
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D: " + collision.collider.tag);
        // Destroy on collision with ground (for non-trigger colliders)
        if (collision.collider.CompareTag("tile"))
        {
            Destroy(gameObject);
        }
    }
}
