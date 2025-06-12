using UnityEngine;

public class Glass : MonoBehaviour
{
    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullets"))
        {
            Destroy(gameObject);
        }
    }
}
