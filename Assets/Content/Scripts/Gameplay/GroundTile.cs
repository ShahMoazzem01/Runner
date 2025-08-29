using UnityEngine;

public class GroundTile : MonoBehaviour, IPoolable
{
    public float moveSpeed = 5f;
    ObjectPool myPool;

    public void SetPool(ObjectPool pool) => myPool = pool;

    public void OnSpawned()
    {
        // Optional: reset tile state here (e.g., obstacles, coins)
    }

    public void OnDespawned()
    {
        // Optional: cleanup before returning to pool
    }

    void Update()
    {
        // Move tile backward using GameManager speed multiplier
        float speedMultiplier = GameManager.Instance.gameSpeed;
        transform.Translate(Vector3.back * moveSpeed * speedMultiplier * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VanishPoint"))
        {
            myPool.ReturnObject(gameObject);
            GroundManager.Instance.SpawnGround();
        }
    }
}