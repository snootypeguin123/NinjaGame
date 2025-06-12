using UnityEngine;

public class CameraCode : MonoBehaviour
{
    public GameObject player;  // Reference to the player GameObject
    private Vector3 offset;  // Store the offset between camera and player
    public float heightOffset = 2.0f; // Amount to raise the camera
    public float horizontalOffset = 0.0f; // Amount to shift the camera horizontally
    public float panDownOffset = -3.0f; // How much to pan down when S is held
    public float panSpeed = 5.0f; // How fast the camera pans
    private float currentYOffset; // The current y offset used for smooth panning

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindPlayer();
        currentYOffset = heightOffset;
    }

    void FindPlayer()
    {
        // Try to find player by tag
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null)
        {
            Debug.LogWarning("No object with 'Player' tag found! Please tag your player object with 'Player' tag.");
            return;
        }

        // Calculate the initial offset
        offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            FindPlayer(); // Try to find player again if reference is lost
            return;
        }
        
        // Handle camera panning with S key
        float targetYOffset = heightOffset;
        if (Input.GetKey(KeyCode.S))
        {
            targetYOffset = panDownOffset;
        }
        // Smoothly interpolate the y offset
        currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, Time.deltaTime * panSpeed);

        transform.position = player.transform.position - offset;
        // Raise the camera by currentYOffset and shift by horizontalOffset
        transform.position = new Vector3(
            transform.position.x + horizontalOffset,
            transform.position.y + currentYOffset,
            transform.position.z
        );
    }
}
