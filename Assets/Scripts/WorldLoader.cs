using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    Dictionary<int, T> grid;
    T defaultValue;

    public Grid(T _defaultValue)
    {
        grid = new Dictionary<int, T>();
        defaultValue = _defaultValue;
    }

    int nonNegative(int n)
    {
        if (n < 0)
        {
            return -n * 2 - 1;
        }
        else
        {
            return n * 2;
        }
    }

    public T this[int key1, int key2]
    {
        get
        {
            int key = nonNegative(key1) | (nonNegative(key2) << 16);
            if (grid.ContainsKey(key))
            {
                return grid[key];
            }
            else
            {
                return defaultValue;
            }
        }
        set
        {
            grid[nonNegative(key1) | (nonNegative(key2) << 16)] = value;
        }
    }
}

[System.Serializable]
struct SpawnType
{
    public GameObject[] variations;
    public int gridResolution;
    public float perlinMultiplier;
    public float spawnProbability;
    public float typeDominance;
}

public class WorldLoader : MonoBehaviour
{
    [SerializeField] float chunkSize;
    [SerializeField] float spawnRadius;

    [SerializeField] GameObject ground;
    [SerializeField] SpawnType[] spawnTypes;
    [SerializeField] int typeGridResolution;
    [SerializeField] float typePerlinMultiplier;

    [SerializeField] GameObject hunter;
    [SerializeField] GameObject player;
    [SerializeField] Vector2 hunterPlayer;
    [SerializeField] GameObject playerCamera;
    [SerializeField] float avoidSpawnRadius;

    GameObject currentPlayer;
    GameObject currentHunter;
    GameObject[] avoidObjects;
    float totalTypeDominance = 0;
    Grid<bool> hasLoaded;
    Vector2 perlinOffset;


    void Start()
    {
        hasLoaded = new Grid<bool>(false);
        perlinOffset = new Vector2(Random.Range(0f, chunkSize * 1000f), Random.Range(0f, chunkSize * 1000f));

        currentPlayer = Instantiate(player, transform.position + Vector3.up, Quaternion.identity);
        playerCamera.transform.SetParent(currentPlayer.transform);
        currentPlayer.GetComponent<PlayerMovement>().relativeTo = playerCamera.transform;
        
        Vector3 hunterPosition = currentPlayer.transform.position;
        hunterPosition.x += hunterPlayer.x;
        hunterPosition.y = 0;
        hunterPosition.z += hunterPlayer.y;
        currentHunter = Instantiate(hunter, hunterPosition, Quaternion.identity);
        currentHunter.GetComponent<BulletShooter>().target = currentPlayer;

        avoidObjects = new GameObject[2]{currentPlayer, currentHunter};

        foreach (SpawnType s in spawnTypes)
        {
            totalTypeDominance += s.typeDominance;
        }
    }

    void Update()
    {
        Vector3 spawnPosition = currentPlayer.transform.position;

        int minX = (int)Mathf.Floor((spawnPosition.x - spawnRadius) / chunkSize);
        int maxX = (int)Mathf.Ceil((spawnPosition.x + spawnRadius) / chunkSize);
        int minY = (int)Mathf.Floor((spawnPosition.z - spawnRadius) / chunkSize);
        int maxY = (int)Mathf.Ceil((spawnPosition.z + spawnRadius) / chunkSize);


        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if ((new Vector3(x * chunkSize, 0, y * chunkSize) - spawnPosition).magnitude <= spawnRadius)
                {
                    LoadChunk(x, y);
                }
            }
        }
    }

    void LoadChunk(int x, int y)
    {
        if (hasLoaded[x, y]) return;

        GameObject chunk = new GameObject("Chunk (" + x + ", " + y + ")");
        chunk.transform.SetParent(transform);

        Vector3 chunkCenter = new Vector3(chunkSize * x, 0, chunkSize * y) + transform.position;
        chunk.transform.position = chunkCenter;

        GameObject groundObj = Instantiate(ground, chunkCenter, Quaternion.identity);
        groundObj.transform.localScale = new Vector3(chunkSize, 1, chunkSize);
        groundObj.transform.SetParent(chunk.transform);

        float negativeOffset = 1 << 16;

        for (int tx = 0; tx < typeGridResolution; tx++)
        {
            for (int ty = 0; ty < typeGridResolution; ty++)
            {
                float typeSize = chunkSize / typeGridResolution;

                Vector3 typeCenter = chunkCenter;
                typeCenter.x += (tx + 1 / 2f) * typeSize  - chunkSize / 2f;
                typeCenter.z += (ty + 1f / 2f) * typeSize  - chunkSize / 2f;;

                float typeValue = Mathf.PerlinNoise(
                    typeCenter.x * typePerlinMultiplier + perlinOffset.x + negativeOffset,
                    typeCenter.z * typePerlinMultiplier + perlinOffset.y + negativeOffset
                    ) * totalTypeDominance;


                SpawnType spawnType = spawnTypes[0];
                foreach (SpawnType s in spawnTypes)
                {
                    if (typeValue < s.typeDominance)
                    {
                        spawnType = s;
                        break;
                    }
                    typeValue -= s.typeDominance;
                }

                for (int sx = 0; sx < spawnType.gridResolution; sx++)
                {
                    for (int sy = 0; sy < spawnType.gridResolution; sy++)
                    {
                        float spawnSize = typeSize / spawnType.gridResolution;
                        Vector3 spawnCenter = typeCenter;
                        spawnCenter.x += (sx + 1f / 2f) * spawnSize - typeSize / 2f;
                        spawnCenter.z += (sy + 1f / 2f) * spawnSize - typeSize / 2f;

                        bool canSpawn = true;
                        foreach(GameObject obj in avoidObjects){
                            if(Vector3.Distance(obj.transform.position, spawnCenter) < avoidSpawnRadius){
                                canSpawn = false;
                                break;
                            }
                        }
                        if(!canSpawn) continue;

                        float spawnValue = Mathf.PerlinNoise(
                            spawnCenter.x * spawnType.perlinMultiplier + perlinOffset.x + negativeOffset,
                            spawnCenter.z * spawnType.perlinMultiplier + perlinOffset.y + negativeOffset
                        );

                        if (spawnValue < spawnType.spawnProbability)
                        {
                            GameObject obj = Instantiate(
                                spawnType.variations[Random.Range(0, spawnType.variations.Length)],
                                spawnCenter + (new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f))) * spawnSize / 4,
                                Quaternion.AngleAxis(Random.Range(0f, Mathf.PI * 2), Vector3.up)
                            );

                            obj.transform.SetParent(chunk.transform);
                        }
                    }

                }
            }
        }

        hasLoaded[x, y] = true;
    }
}
