using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    Dictionary<int, T> grid;
    T defaultValue;

    public Grid (T _defaultValue) {
        grid = new Dictionary<int, T>();
        defaultValue = _defaultValue;
    }

    int nonNegative(int n) {
        if(n < 0){
            return -n * 2 - 1;
        } else {
            return n * 2;
        }
    }

    public T this[int key1, int key2]
    {
        get
        {
            int key = nonNegative(key1) | (nonNegative(key2) << 16);
            if(grid.ContainsKey(key)){
                return grid[key];
            } else {
                return defaultValue;
            }
        }
        set
        {
            grid[nonNegative(key1) | (nonNegative(key2) << 16)] = value;
        }
    }
}

public class WorldLoader : MonoBehaviour
{
    Grid<bool> hasLoaded;

    [SerializeField] Vector2 chunkSize;
    [SerializeField] GameObject ground;
    [SerializeField] GameObject tree;
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] float spawnRadius;
    [SerializeField] float perlinMultiplier;
    [SerializeField] int perlinResolution;
    [SerializeField] GameObject player;
    [SerializeField] GameObject camera;
    GameObject currentPlayer;


    void Start()
    {
        hasLoaded = new Grid<bool>(false);

        currentPlayer = Instantiate(player, transform.position + Vector3.up, Quaternion.identity);
        camera.transform.SetParent(currentPlayer.transform);
        currentPlayer.GetComponent<PlayerMovement>().relativeTo = camera.transform;
    }

    void Update()
    {
        spawnPosition = currentPlayer.transform.position;
        int minX = (int)Mathf.Floor(spawnPosition.x - spawnRadius);
        int maxX = (int)Mathf.Ceil(spawnPosition.x + spawnRadius);
        int minY = (int)Mathf.Floor(spawnPosition.y - spawnRadius);
        int maxY = (int)Mathf.Ceil(spawnPosition.y + spawnRadius);


        for(int x = minX; x <= maxX; x++){
            for(int y = minY; y <= maxY; y++){
                if((new Vector3(x * chunkSize.x, y * chunkSize.y, 0) - spawnPosition).magnitude <= spawnRadius){
                    LoadChunk(x, y);
                }
            }
        }
    }

    void LoadChunk(int x, int y) {
        if(hasLoaded[x, y]) return;

        GameObject chunk = new GameObject("Chunk (" + x + ", " + y + ")");
        chunk.transform.SetParent(transform);

        Vector3 center = new Vector3(chunkSize.x * x, 0, chunkSize.y * y) + transform.position;
        GameObject groundObj = Instantiate(ground, center, Quaternion.identity);
        groundObj.transform.localScale = new Vector3(chunkSize.x, 1, chunkSize.y);
        groundObj.transform.SetParent(chunk.transform);
        

        int n = perlinResolution;
        for(int sx = 0; sx < n; sx++){
            for(int sy = 0; sy < n; sy++){
                Vector3 position = center + new Vector3((sx - (n - 1) / 2) * chunkSize.x, 0, (sy - (n - 1) / 2) * chunkSize.y) / n;

                float px = (position.x * perlinMultiplier) + (float)(1 << 16);
                float py = (position.z * perlinMultiplier) + (float)(1 << 16);

                if(Mathf.PerlinNoise(px, py) > 0.7 && Vector3.Distance(currentPlayer.transform.position, position) > 20){
                    GameObject treeObject = Instantiate(tree, position, Quaternion.identity);
                    treeObject.transform.SetParent(chunk.transform);
                }
            }
        }

        hasLoaded[x, y] = true;
    }
}
