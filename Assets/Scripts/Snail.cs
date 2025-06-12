using UnityEngine;
using UnityEngine.SceneManagement;

public class Snail : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float chaseDistance = 5f;
    
    private float startingX;
    private int direction = 1;
    private int health = 1;
    private Rigidbody2D rb;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingX = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);
            if (distToPlayer <= chaseDistance)
            {
                // Chase the player only horizontally
                Vector2 directionToPlayer = (player.position - transform.position).normalized;
                directionToPlayer.y = 0; // Prevent vertical movement
                directionToPlayer = directionToPlayer.normalized; // Re-normalize in case x was 0
                rb.MovePosition(rb.position + directionToPlayer * moveSpeed * Time.deltaTime);
                // Flip sprite to face the player
                if ((directionToPlayer.x > 0 && transform.localScale.x < 0) || (directionToPlayer.x < 0 && transform.localScale.x > 0))
                {
                    Vector3 newScale = transform.localScale;
                    newScale.x *= -1;
                    transform.localScale = newScale;
                }
            }
            else
            {
                // Stay still
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deal 30 damage to the player instead of restarting the scene
            Hp playerHp = collision.gameObject.GetComponent<Hp>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(30);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
