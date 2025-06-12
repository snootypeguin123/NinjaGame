using UnityEngine;

public class GunFollow : MonoBehaviour
{
    public GameObject player;  // Reference to the player GameObject
    public float distance = 1.0f; // Distance from player
    public GameObject bulletPrefab; // Assign in inspector
    public Transform firePoint; // Assign in inspector (tip of gun)
    public enum GunState { Revolver, Shotgun }
    private GunState currentState = GunState.Revolver;
    public int shotgunPellets = 5; // Number of bullets in shotgun spread
    public float shotgunSpreadAngle = 20f; // Total spread angle in degrees

    void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No object with 'Player' tag found! Please tag your player object with 'Player' tag.");
        }
    }

    void LateUpdate()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // Get mouse position in world space (2D)
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.transform.position.z * -1));
        mouseWorldPos.z = 0f; // For 2D, keep everything on the same plane

        // Calculate direction from player to mouse
        Vector3 direction = (mouseWorldPos - player.transform.position).normalized;

        // Set gun position at fixed distance from player in that direction
        transform.position = player.transform.position + direction * distance;

        // Optional: Rotate gun to face the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Log revolverstate when 1 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("revolverstate");
            currentState = GunState.Revolver;
        }
        // Log shotgun state when 2 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("shotgun state");
            currentState = GunState.Shotgun;
        }

        // Fire logic based on state
        if (Input.GetMouseButtonDown(0) && bulletPrefab != null && firePoint != null)
        {
            if (currentState == GunState.Revolver)
            {
                // Fire one bullet
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
                var bulletScript = bullet.GetComponent<Bullet>();
                // Ignore collision with player and gun
                var bulletCollider = bullet.GetComponent<Collider2D>();
                if (bulletCollider != null)
                {
                    // Ignore collision with player
                    var playerCollider = player.GetComponent<Collider2D>();
                    if (playerCollider != null)
                        Physics2D.IgnoreCollision(bulletCollider, playerCollider);
                    // Ignore collision with gun
                    var gunCollider = GetComponent<Collider2D>();
                    if (gunCollider != null)
                        Physics2D.IgnoreCollision(bulletCollider, gunCollider);
                }
                if (bulletScript != null)
                {
                    bulletScript.SetDirection(mouseWorldPos);
                    bulletScript.SetSpeed(4f); // Make revolver bullet 4x faster
                    // Make revolver bullet 4x longer on the y axis
                    Vector3 scale = bullet.transform.localScale;
                    scale.x *= 4f;
                    bullet.transform.localScale = scale;
                }
                else
                {
                    Debug.LogError("Bullet prefab does not have a Bullet script attached!");
                }
            }
            else if (currentState == GunState.Shotgun)
            {
                // Fire a spread of bullets with slight randomization
                float startAngle = angle - shotgunSpreadAngle / 2f;
                float angleStep = shotgunSpreadAngle / (shotgunPellets - 1);
                for (int i = 0; i < shotgunPellets; i++)
                {
                    // Add randomization within +/- (angleStep/2) degrees
                    float randomOffset = Random.Range(-angleStep / 2f, angleStep / 2f);
                    float pelletAngle = startAngle + angleStep * i + randomOffset;
                    Quaternion pelletRotation = Quaternion.Euler(0, 0, pelletAngle);
                    Vector3 pelletDir = pelletRotation * Vector3.right;
                    Vector3 spawnPos = firePoint.position + pelletDir * 0.5f; // Offset set back to 0.5 units
                    GameObject bullet = Instantiate(bulletPrefab, spawnPos, pelletRotation);
                    var bulletScript = bullet.GetComponent<Bullet>();
                    // Ignore collision with player and gun
                    var bulletCollider = bullet.GetComponent<Collider2D>();
                    if (bulletCollider != null)
                    {
                        // Ignore collision with player
                        var playerCollider = player.GetComponent<Collider2D>();
                        if (playerCollider != null)
                            Physics2D.IgnoreCollision(bulletCollider, playerCollider);
                        // Ignore collision with gun
                        var gunCollider = GetComponent<Collider2D>();
                        if (gunCollider != null)
                            Physics2D.IgnoreCollision(bulletCollider, gunCollider);
                    }
                    if (bulletScript != null)
                    {
                        // Calculate direction for each pellet
                        bulletScript.SetDirection(firePoint.position + pelletDir);
                        // Randomize speed between 0.85 and 1.15
                        if (bulletScript.GetType().GetMethod("SetSpeed") != null)
                        {
                            float speedMultiplier = Random.Range(0.85f, 1.15f);
                            bulletScript.SetSpeed(speedMultiplier);
                        }
                    }
                    else
                    {
                        Debug.LogError("Bullet prefab does not have a Bullet script attached!");
                    }
                }
            }
        }
    }
}
