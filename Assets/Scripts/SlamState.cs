using UnityEngine;

public class SlamState : PlayerBaseState
{
    private float enterTime;
    private float maxFallSpeed = -30f; // Adjust as needed

    public SlamState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        enterTime = Time.time;
        // Play slam animation if available
        if (stateMachine.Animator != null)
            stateMachine.Animator.Play("Slam");
        Debug.Log($"[SlamState] Entering Slam State at {enterTime:F2}s");
    }

    public override void Tick(float deltaTime)
    {
        // Severely limit horizontal movement
        float limitedHorizontal = 0f;
        if (stateMachine.RB != null)
        {
            Vector2 moveInput = stateMachine.InputReader.GetMovementInput();
            limitedHorizontal = moveInput.x * stateMachine.MoveSpeed * 0.1f; // 10% of normal speed
            stateMachine.RB.linearVelocity = new Vector2(limitedHorizontal, maxFallSpeed);
        }

        // If grounded, transition to Idle/Walk/Run
        if (stateMachine.IsGrounded())
        {
            stateMachine.JumpsRemaining = stateMachine.MaxJumps;
            Vector2 moveInput = stateMachine.InputReader.GetMovementInput();
            if (moveInput == Vector2.zero)
                stateMachine.SwitchState(stateMachine.IdleState);
            else if (stateMachine.InputReader.IsRunPressed())
                stateMachine.SwitchState(stateMachine.RunState);
            else
                stateMachine.SwitchState(stateMachine.WalkState);
            return;
        }

        // Allow jump if jumps remain (double jump)
        if (stateMachine.InputReader.IsJumpPressed() && stateMachine.JumpsRemaining > 0)
        {
            stateMachine.SwitchState(stateMachine.JumpState);
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log($"[SlamState] Exiting Slam State after {Time.time - enterTime:F2}s");
    }
}
