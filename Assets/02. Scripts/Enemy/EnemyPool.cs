using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : SingletonBehaviour<EnemyPool>
{
    [System.Serializable]
    public class Pool
    {
        public EEnemyType type;
        public GameObject prefab;
        public int        size;
    }

    public List<Pool>                                 EnemyPrefabs;

    private Dictionary<EEnemyType, Queue<GameObject>> _enemyPoolDictionary;

    private void Start()
    {
        _enemyPoolDictionary = new Dictionary<EEnemyType, Queue<GameObject>>();

        foreach (Pool pool in EnemyPrefabs)
        {
            Queue<GameObject> enemyPool = new Queue<GameObject>();
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject enemy = Instantiate(pool.prefab);
                enemy.transform.SetParent(this.transform);
                enemy.SetActive(false);
                enemyPool.Enqueue(enemy);
            }

            _enemyPoolDictionary.Add(pool.type, enemyPool);
        }
    }

    public GameObject GetEnemy(EEnemyType type)
    {
        if (_enemyPoolDictionary.TryGetValue(type, out Queue<GameObject> enemyPool))
        {
            GameObject enemy = null;
            if (enemyPool.Count > 0)
            {
                enemy = enemyPool.Dequeue();
                enemy.SetActive(true);
            }
            else
            {
                // 풀에 남는 오브젝트가 없다면 새로 생성
                int typeIndex = 0;
                for (int i = 0; i < EnemyPrefabs.Count; i++)
                {
                    if (EnemyPrefabs[i].type == type)
                    {
                        typeIndex = i;
                        break;
                    }
                }

                enemy = Instantiate(EnemyPrefabs[typeIndex].prefab);
            }

            return enemy;
        }

        return null;
    }

    public void ReleaseEnemy(EEnemyType type, GameObject enemy)
    {
        enemy.SetActive(false);
        _enemyPoolDictionary[type].Enqueue(enemy);
    }
}

