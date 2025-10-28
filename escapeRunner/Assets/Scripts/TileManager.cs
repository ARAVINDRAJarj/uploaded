using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject groundTilePrefab;
    public Transform player;
    public int numberOfTiles = 8;
    public float tileLength = 10f;

    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnZ = 0f;
    private float safeZone = 25f; // distance before spawning new tile

    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (player.position.z - safeZone > (spawnZ - numberOfTiles * tileLength))

        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(groundTilePrefab, Vector3.forward * spawnZ, Quaternion.identity);
        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
