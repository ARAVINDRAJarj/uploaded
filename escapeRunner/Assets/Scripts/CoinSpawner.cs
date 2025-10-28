using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform player;
    public float spawnDistance = 40f;
    public float spawnInterval = 1.5f;
    public float spawnRangeX = 4f;
    public float spawnHeight = 1.5f;

    void Start()
    {
        StartCoroutine(SpawnCoins());
    }

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            SpawnCoin();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCoin()
    {
        if (player == null || coinPrefab == null) return;

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnHeight, player.position.z + spawnDistance);

        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }
}
