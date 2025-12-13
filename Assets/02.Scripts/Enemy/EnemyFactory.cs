using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int poolSize = 10; // 초기 풀 크기

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Start()
    {
        InitializePool();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemies();
        }
    }

    /// <summary>
    /// 오브젝트 풀 초기화
    /// </summary>
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    /// <summary>
    /// 적 스폰 (풀에서 가져오기)
    /// </summary>
    private void SpawnEnemies()
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

    /// <summary>
    /// 풀에서 적 가져오기
    /// </summary>
    private GameObject GetEnemyFromPool()
    {
        // 풀에 사용 가능한 적이 있으면 반환
        if (enemyPool.Count > 0)
        {
            return enemyPool.Dequeue();
        }

        // 풀이 비어있으면 새로 생성
        GameObject newEnemy = Instantiate(enemyPrefab, transform);
        return newEnemy;
    }

    /// <summary>
    /// 적을 풀로 반환 (EnemyStats에서 호출)
    /// </summary>
    public void ReturnEnemyToPool(GameObject enemy)
    {
        if (enemy != null)
        {
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }
}