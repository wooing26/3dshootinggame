using System.Collections.Generic;
using UnityEngine;

public class BombPool : SingletonBehaviour<BombPool>
{
    public GameObject           BombPrefab;

    private List<GameObject>    _bombs;

    public void SetPoolSize(int poolSize)
    {
        _bombs = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bomb = Instantiate(BombPrefab);

            _bombs.Add(bomb);

            bomb.transform.SetParent(this.transform);
            bomb.SetActive(false);
        }
    }

    public Bomb Create(Vector3 position)
    {
        foreach(GameObject bombObject in _bombs)
        {
            if (bombObject.activeInHierarchy == false)
            {
                Bomb bomb = bombObject.GetComponent<Bomb>();
                if (bomb == null)
                {
                    return null;
                }

                bombObject.transform.position = position;
                bombObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                bombObject.SetActive(true);
                return bomb;
            }
        }
        return null;
    }
}
