using UnityEngine;

public class Coins : MonoBehaviour
{
    // Static variable to keep track of total coins collected
    public static int totalCoins = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when another collider enters this object's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Increment the coin counter
            totalCoins++;
            
            // Destroy this coin object
            Destroy(gameObject);
        }
    }
}
