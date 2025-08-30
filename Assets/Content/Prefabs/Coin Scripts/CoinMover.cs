using UnityEngine;

public class CoinMover : MonoBehaviour
{
    public float worldMoveSpeed = 10f; // Should match player's forward speed

    void Update()
    {
        // Move coin toward player (negative Z direction)
        transform.Translate(0, 0, -worldMoveSpeed * Time.deltaTime, Space.World);
    }
}
