using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Lane Settings")]
    [SerializeField] float playAreaWidth = 12f;
    int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    Vector3 targetPosition;
    float laneWidth;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 10f;

    [Header("Jump Settings")]
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float jumpDuration = 0.5f; // time to reach apex

    bool isJumping;
    float jumpTimer = 0f;
    float groundY; // Fixed ground level

    void Awake()
    {
        laneWidth = playAreaWidth / 3f;
        groundY = transform.position.y; // Store the initial ground level
        updateTargetPosition();
    }

    void Update()
    {
        // Horizontal movement - only on X axis
        Vector3 horizontalTarget = new Vector3(targetPosition.x, transform.position.y, transform.position.z);

        float speed = moveSpeed * Time.deltaTime;
        if (GameManager.Instance != null)
            speed *= GameManager.Instance.gameSpeed;

        transform.position = Vector3.MoveTowards(
            transform.position,
            horizontalTarget,
            speed
        );

        // Handle Jumping
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float t = jumpTimer / jumpDuration;

            // Simple parabola jump: y = 4*h*t*(1-t)
            float newY = groundY + (4 * jumpHeight * t * (1 - t));
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (jumpTimer >= jumpDuration)
            {
                isJumping = false;
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            }
        }
    }


    void updateTargetPosition()
    {
        // Lane X positions: left = +4, center = 0, right = -4
        float x = (currentLane - 1) * laneWidth; // center lane = 0
        targetPosition = new Vector3(x, groundY, transform.position.z); // Always use groundY for target position
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float moveValue = context.ReadValue<float>();
            if (moveValue < 0) MoveLeft();
            if (moveValue > 0) MoveRight();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) Jump();
    }

    public void MoveLeft()
    {
        if (currentLane == 0) return;
        currentLane--;
        updateTargetPosition();
    }

    public void MoveRight()
    {
        if (currentLane == 2) return;
        currentLane++;
        updateTargetPosition();
    }

    public void Jump()
    {
        if (isJumping) return;
        AudioManager.Instance.PlayJumpSound();
        isJumping = true;
        jumpTimer = 0f;
    }
}