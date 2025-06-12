using UnityEngine;

public class InputReader
{
    // Consider using Unity's new Input System for more robust handling
    // For now, using the legacy Input Manager

    public Vector2 GetMovementInput()
    {
        // Use GetAxisRaw for immediate response without smoothing
        float horizontal = Input.GetAxisRaw("Horizontal");
        // float vertical = Input.GetAxisRaw("Vertical"); // Ignore vertical axis for standard movement

        // Only use horizontal input for walking/running
        Vector2 input = new Vector2(horizontal, 0f);

        // Normalization might not be strictly necessary anymore with only one axis,
        // but doesn't hurt to keep if other inputs could be added later.
        // if (input.sqrMagnitude > 1) // No need to normalize a 1D vector derived this way
        // {
        //     input.Normalize();
        // }
        return input;
    }

    public bool IsRunPressed()
    {
        // Only allow running with Right Shift
        return Input.GetKey(KeyCode.RightShift);
    }

    public bool IsJumpPressed()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool IsCrouchHeld()
    {
        // Use GetKey for continuous check while held
        // Changed crouch key to apostrophe (')
        return Input.GetKey(KeyCode.Quote);
    }

    public bool IsSlamPressed()
    {
        // Use GetKey for continuous check while held
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    }

    public bool IsQuickDashPressed()
    {
        // Quick dash on tap of Left Shift
        return Input.GetKeyDown(KeyCode.LeftShift);
    }
}