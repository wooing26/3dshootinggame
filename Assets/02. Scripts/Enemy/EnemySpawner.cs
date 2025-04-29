using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyPool _enemyPool;

    private void Awake()
    {
        _enemyPool = GetComponent<EnemyPool>();
    }
}
