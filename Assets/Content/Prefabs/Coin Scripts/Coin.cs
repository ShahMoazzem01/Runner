using UnityEngine;

public class Coin : MonoBehaviour
{
    public int laneIndex; // 0 = left, 1 = middle, 2 = right
    public CoinPool pool;

    void OnTriggerEnter(Collider other)
    {
        // If coin hits obstacle, return it to pool
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            ReturnToPool();
        }

        // If player collects coin
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayCoinSound();
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        // Add score or other logic here
        Debug.Log("Coin collected!");
        //update coinCount and ui in game manager
        GameManager.Instance.onCoinCollected();

        // Play coin collection sound

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnToPool(this);
        else
            gameObject.SetActive(false);
    }
}
