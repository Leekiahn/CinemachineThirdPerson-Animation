using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize;

    [SerializeField] private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    private void SpawnEnemies()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = GetEnemyFromPool();
            if (enemy != null)
            {
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;
                enemy.SetActive(true);
            }
        }
    }

    private GameObject GetEnemyFromPool()
    {
        if (enemyPool.Count > 0)
        {
            return enemyPool.Dequeue();
        }
        return null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the enemy spawn trigger.");

            SpawnEnemies();
        }
    }
}
