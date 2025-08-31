using UnityEngine;

public class PlayerCollision : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag == "MiniObstacle")
        {
            GameEvents.MiniHit();
        }
        else if (tag == "DeadlyObstacle")
        {
            GameEvents.Hit();
        }
    }
}
