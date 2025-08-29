using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("lane Settings")]
    [SerializeField] float playAreaWidth = 12f;
    int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    Vector3 targetPosition;
    float laneWidth;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 10f;

    [Header("Jump Settings")]
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float jumpDuration = 0.5f; //time to reach apex

    bool isJumping;
    float jumpTimer = 0f;
    float startY;
    float TargetY;



    void Awake()
    {
        laneWidth = playAreaWidth / 3f;
        updateTargetPosition();
        startY = transform.position.y;
    }

    void Update()
    {

        //horizontal movement
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        //handle Jumping
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float t = jumpTimer / jumpDuration;

            //simple simple parabola jump : y = x^2 / y = 4*h*t*(1-t)
            float newY = startY + (4 * jumpHeight * t * (1 - t));
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (jumpTimer >= jumpDuration)
            {
                isJumping = false;
                transform.position = new Vector3(transform.position.x, startY, transform.position.z);
            }

        }
    }


    void updateTargetPosition()
    {
        // Lane X positions: left = +4, center = 0, right = -4
        float x = (currentLane - 1) * laneWidth; // center lane = 0
        targetPosition = new Vector3(x, transform.position.y, transform.position.z);
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
        isJumping = true;
        jumpTimer = 0f;
        startY = transform.position.y;
    }




}
