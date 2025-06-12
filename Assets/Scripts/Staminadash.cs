using UnityEngine;

public class Staminadash : MonoBehaviour
{
    public float dashSpeed = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    private Rigidbody2D rb;
    private PlayerStateMachine stateMachine;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector2 dashDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<PlayerStateMachine>();
        if (rb == null)
            Debug.LogError("Staminadash: No Rigidbody2D found on player!");
        if (stateMachine == null)
            Debug.LogError("Staminadash: No PlayerStateMachine found on player!");
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            // Force dash velocity every frame
            rb.linearVelocity = dashDirection * dashSpeed;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop horizontal dash, keep vertical
                cooldownTimer = dashCooldown;
            }
        }
        else if (cooldownTimer <= 0f && stateMachine != null && stateMachine.InputReader.IsQuickDashPressed())
        {
            // Determine dash direction (facing direction)
            float facing = transform.localScale.x >= 0 ? 1f : -1f;
            dashDirection = new Vector2(facing, 0f);
            rb.linearVelocity = dashDirection * dashSpeed;
            isDashing = true;
            dashTimer = dashDuration;
        }
    }
}
