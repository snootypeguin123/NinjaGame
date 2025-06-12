using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openDistance = 2f; // Distance at which the door can open
    [SerializeField] private GameObject doorObject;    // The visual door object to disable/open

    private Transform player;
    private float slideHeight = 2f; // How far the door slides up
    private float slideSpeed = 7f;  // How fast the door slides up
    private float yOffset = 0.5f;   // Y-axis offset
    private Vector3 doorStartPos;
    private Vector3 doorTargetPos;
    private bool isOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        if (doorObject != null)
        {
            doorStartPos = doorObject.transform.position + Vector3.up * yOffset;
            doorTargetPos = doorStartPos + Vector3.up * slideHeight;
            doorObject.transform.position = doorStartPos;
        }
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = -10; // or any value lower than your tiles
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Always set this door to the bottom of the hierarchy
        transform.SetAsLastSibling();
        if (player == null) return;

        // Check if all enemies are dead
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // bool allEnemiesDead = enemies.Length == 0;

        // Check if player is nearby
        Vector3 diff = player.position - transform.position;
        bool playerNearby = Mathf.Abs(diff.x) <= openDistance && Mathf.Abs(diff.y) <= openDistance;

        // Determine if the door should be open
        if (playerNearby && !isOpen)
        {
            Debug.Log("Door is opening!");
            isOpen = true;
        }
        else if (!playerNearby && isOpen)
        {
            Debug.Log("Door is closing!");
            isOpen = false;
        }

        // Animate the door sliding up or down
        if (doorObject != null)
        {
            Vector3 targetPos = isOpen ? doorTargetPos : doorStartPos;
            doorObject.transform.position = Vector3.MoveTowards(
                doorObject.transform.position,
                targetPos,
                slideSpeed * Time.deltaTime
            );
        }
    }

    void OpenDoor()
    {
        // No longer needed for this behavior
    }
}
