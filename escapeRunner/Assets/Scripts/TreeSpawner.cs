using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public Transform player;
    public float spawnDistance = 30f;
    public float treeSpacing = 10f;
    public float sideOffset = 7f;

    private float nextSpawnZ = 0f;

    void Update()
    {
        if (player == null || treePrefabs.Length == 0) return;

        // Keep spawning trees ahead of the player
        while (player.position.z + spawnDistance > nextSpawnZ)
        {
            SpawnTreeRow(nextSpawnZ);
            nextSpawnZ += treeSpacing;
        }
    }

    void SpawnTreeRow(float zPos)
    {
        // Left side
        Vector3 leftPos = new Vector3(-sideOffset, 0, zPos);
        // Right side
        Vector3 rightPos = new Vector3(sideOffset, 0, zPos);

        // Choose random trees
        GameObject leftTree = treePrefabs[Random.Range(0, treePrefabs.Length)];
        GameObject rightTree = treePrefabs[Random.Range(0, treePrefabs.Length)];

        Instantiate(leftTree, leftPos, Quaternion.identity, transform);
        Instantiate(rightTree, rightPos, Quaternion.identity, transform);
    }
}
