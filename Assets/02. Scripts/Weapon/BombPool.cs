using System.Collections.Generic;
using UnityEngine;

public class BombPool : MonoBehaviour
{
    public static BombPool      Instance;

    public GameObject           BombPrefab;

    private List<GameObject>    _bombs;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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
                bombObject.SetActive(true);
                return bomb;
            }
        }
        return null;
    }
}
