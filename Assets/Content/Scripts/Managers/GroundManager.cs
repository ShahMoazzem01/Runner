using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager Instance { get; private set; }

    [Header("GroundPools")]
    [SerializeField] ObjectPool[] groundPools;
    [SerializeField] private ObjectPool firstGroundPool;
    [SerializeField] int initalGroundPoolSize = 4;
    [SerializeField] float groundLength = 10f;

    Transform lastGround;
    private bool firstSpawnDone = false; // To track the first ground

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        SpawnInitialGround();
    }

    void SpawnInitialGround()
    {
        for (int i = 0; i < initalGroundPoolSize; i++)
        {
            SpawnGround();
        }
    }

    public void SpawnGround()
    {
        ObjectPool chosenPool;
        if (!firstSpawnDone && firstGroundPool != null)
        {
            chosenPool = firstGroundPool;
            firstSpawnDone = true;
        }
        else
        {
            int randomIndex = Random.Range(0, groundPools.Length);
            chosenPool = groundPools[randomIndex];
        }

        GameObject ground = chosenPool.GetObject();

        if (lastGround == null)
        {
            ground.transform.position = new Vector3(0f, 0f, 0f);
        }
        else
        {
            ground.transform.position = lastGround.position + new Vector3(0, 0f, groundLength);
        }

        //Assign pool back-reference
        if (ground.TryGetComponent(out GroundTile tile))
        {
            tile.SetPool(chosenPool);
        }

        lastGround = ground.transform;

    }
}
