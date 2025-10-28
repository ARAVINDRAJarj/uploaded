using UnityEngine;
using System.Collections;

public class SnakeSpawner : MonoBehaviour
{
    public GameObject snakePrefab;
    public Transform player;
    public float spawnDistance = 40f;
    public float spawnInterval = 6f;
    public float spawnRangeX = 4f;

    void Start()
    {
        StartCoroutine(SpawnSnakes());
    }

    IEnumerator SpawnSnakes()
    {
        while (true)
        {
            SpawnSnake();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSnake()
    {
        if (player == null || snakePrefab == null) return;

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, 0.3f, player.position.z + spawnDistance);

        Instantiate(snakePrefab, spawnPos, Quaternion.identity);
    }
}
