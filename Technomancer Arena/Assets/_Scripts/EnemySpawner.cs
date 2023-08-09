using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float spawnRate;
    [SerializeField] private Transform[] spawnPoints;
    private Transform playerPos;

    private void Start()
    {
        playerPos = DATA.Instance.player.transform;
        StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (enabled)
        {
            GameObject enemySpawned = Instantiate(enemyToSpawn, spawnPoints[Random.Range(0, spawnPoints.Length-1)].position, Quaternion.identity);
            enemySpawned.GetComponent<PracticeDummy>().SetPlayer(playerPos);


            yield return wait;
        }
    }
}
