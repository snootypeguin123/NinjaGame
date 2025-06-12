using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;

    // Call this immediately after instantiating the bullet
    public void SetDirection(Vector2 targetPosition)
    {
        Vector2 startPosition = transform.position;
        direction = (targetPosition - startPosition).normalized;
    }

    // Set the speed multiplier for this bullet
    public void SetSpeed(float multiplier)
    {
        speed = 10f * multiplier;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit: " + collision.tag);
        if (collision.CompareTag("Enemy"))
        {
            var damageable = collision.GetComponent<MonoBehaviour>();
            if (damageable != null)
            {
                var method = damageable.GetType().GetMethod("TakeDamage");
                if (method != null)
                {
                    method.Invoke(damageable, new object[] { 1 });
                }
            }
        }
        // Destroy on any collision except the player, the gun, or other bullets
        if (!collision.CompareTag("Player") && !collision.CompareTag("Gun") && !collision.CompareTag("Bullets") && !collision.CompareTag("Hellorb"))
        {
            Destroy(gameObject);
        }
    }
}