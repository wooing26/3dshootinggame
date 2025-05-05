using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    private EnemyPool _enemyPool;
    public float      RandomSpawnRadius = 5f;
    public float      MinSpawnInterval  = 1f;
    public float      MaxSpawnInterval  = 3f;
    private float     _nextSpawnTime;
    private float     _spawnTimer;

    private void Awake()
    {
        _enemyPool = GetComponent<EnemyPool>();
    }

    private void Start()
    {
        ScheduleNextSpawn();
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _nextSpawnTime)
        {
            int enemyType = Random.Range(0, (int)EEnemyType.Count);

            Vector3 spawnPoint = GetRandomSpawnPosition();
            SpawnEnemy((EEnemyType)enemyType, spawnPoint);
            ScheduleNextSpawn();
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // 예시: 현재 위치 주변 랜덤 반경 5 안
        Vector3 spawnPoint = transform.position + Random.insideUnitSphere * RandomSpawnRadius;
        spawnPoint.y = 1.5f;
        return spawnPoint;
    }

    private void ScheduleNextSpawn()
    {
        _nextSpawnTime = Random.Range(MinSpawnInterval, MaxSpawnInterval);
        _spawnTimer = 0;
    }

    public void SpawnEnemy(EEnemyType type, Vector3 position)
    {
        GameObject enemyObject = _enemyPool.GetEnemy(type);
        if (enemyObject != null)
        {
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.Initialize(position);
        }
    }
}
