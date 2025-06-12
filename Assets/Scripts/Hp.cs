using UnityEngine;
using UnityEngine.SceneManagement;

public class Hp : MonoBehaviour
{
    public int hp = 3; // Set initial HP
    public int maxHp = 100; // Set this to your player's max HP
    public GameObject hpPrefab; // Assign your HP prefab in the Inspector
    public Vector3 prefabSpawnPosition = Vector3.zero; // Set spawn position in Inspector if needed
    public float hitCooldownDuration = 1.0f; // seconds
    private float hitCooldownTimer = 0f;
    private float originalBarWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject healthBar = GameObject.FindGameObjectWithTag("Healthbar");
        if (healthBar != null)
        {
            RectTransform rt = healthBar.GetComponent<RectTransform>();
            if (rt != null)
                originalBarWidth = rt.sizeDelta.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For testing: reduce HP with H key
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
        // Decrement hit cooldown timer
        if (hitCooldownTimer > 0f)
        {
            hitCooldownTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int amount)
    {
        if (hitCooldownTimer > 0f)
            return;
        hitCooldownTimer = hitCooldownDuration;
        hp -= amount;
        // Shrink health bar (UI version)
        GameObject healthBar = GameObject.FindGameObjectWithTag("Healthbar");
        if (healthBar != null)
        {
            RectTransform rt = healthBar.GetComponent<RectTransform>();
            if (rt != null)
            {
                float healthPercent = Mathf.Clamp01((float)hp / maxHp);
                rt.sizeDelta = new Vector2(originalBarWidth * healthPercent, rt.sizeDelta.y);
            }
        }
        if (hp <= 0)
        {
            Instantiate(hpPrefab, prefabSpawnPosition, Quaternion.identity);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
