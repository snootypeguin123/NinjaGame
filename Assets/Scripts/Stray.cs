using UnityEngine;

public class Stray : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float moveSpeed = 3f;
    public GameObject hellOrbPrefab;
    public float throwInterval = 2f;
    private Transform player;
    private Vector3 lastPlayerPosition;
    private bool isThrowing = false;
    private float throwTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < detectionRadius)
        {
            // Move away from the player
            Vector3 directionAway = (transform.position - player.position).normalized;
            transform.position += directionAway * moveSpeed * Time.deltaTime;
            isThrowing = false;
        }
        else
        {
            if (!isThrowing)
            {
                lastPlayerPosition = player.position;
                isThrowing = true;
                throwTimer = 0f;
            }
            if (isThrowing && hellOrbPrefab != null)
            {
                throwTimer += Time.deltaTime;
                if (throwTimer >= throwInterval)
                {
                    ThrowHellOrb();
                    throwTimer = 0f;
                }
            }
        }
    }

    void ThrowHellOrb()
    {
        GameObject orb = Instantiate(hellOrbPrefab, transform.position, Quaternion.identity);
        // Always use the player's current position when shooting
        var hellorb = orb.GetComponent<Hellorb>();
        if (hellorb != null && player != null)
        {
            hellorb.SetDirection(player.position);
        }
        // Optionally support Bullet script as well
        var bullet = orb.GetComponent<Bullet>();
        if (bullet != null && player != null)
        {
            bullet.SetDirection(player.position);
        }
    }
}
