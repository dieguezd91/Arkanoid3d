using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPool<T> : MonoBehaviour
{

    private GameObject _prefab;
    [SerializeField] private List<GameObject> _itemList;
    public int PoolSize => _poolSize;
    int _poolSize;

    public GenericPool(GameObject prefab, int poolSize) 
    {
        _prefab = prefab;
        _poolSize = poolSize;
        Initialize();
    }

    public void Initialize()
    {
        for (int i = 0; i < _poolSize; i++)
            AddItemToPool();
    }

    public void AddItemToPool()
    {
        GameObject item = Instantiate(_prefab);
        item.SetActive(false);
        _itemList.Add(item);
        item.transform.parent = transform;
    }

    public GameObject RequestItem()
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (!_itemList[i].activeSelf)
            {
                _itemList[i].SetActive(true);
                return _itemList[i];
            }
        }
        return null;
    }
}
