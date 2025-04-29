using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : SingletonBehaviour<EnemyPool>
{
    public List<GameObject>         EnemyPrefabs;
    public int                      PoolCount = 10;

    private List<List<GameObject>>  _enemyPools;

    private void Awake()
    {
        _enemyPools = new List<List<GameObject>>();

        foreach (var prefab in EnemyPrefabs)
        {
            List<GameObject> pool = new List<GameObject>();
            for (int i = 0; i < PoolCount; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
            _enemyPools.Add(pool);
        }
    }

    public GameObject GetEnemy(int typeIndex)
    {
        if (typeIndex < 0 || typeIndex >= _enemyPools.Count)
        {
            Debug.LogWarning("Invalid enemy type index.");
            return null;
        }

        foreach (var enemy in _enemyPools[typeIndex])
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        // 풀에 여유 없으면 확장 가능
        GameObject newEnemy = Instantiate(EnemyPrefabs[typeIndex]);
        newEnemy.SetActive(false);
        _enemyPools[typeIndex].Add(newEnemy);
        return newEnemy;
    }
}

