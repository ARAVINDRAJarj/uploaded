using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform player;
    public float spawnInterval = 2f;
    public float spawnDistance = 40f;
    public float xRange = 4f;

    void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.Find("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            float randomX = Random.Range(-xRange, xRange);
            Vector3 spawnPos = new Vector3(randomX, 0.5f, player.position.z + spawnDistance);
            GameObject obj = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

            Destroy(obj, 5f); // removes after 5 seconds
        }
    }
}
