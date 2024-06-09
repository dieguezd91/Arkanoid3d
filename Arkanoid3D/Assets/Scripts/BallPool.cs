using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    public static BallPool instance;
    [SerializeField] GameObject _ballPrefab;
    [SerializeField] private List<GameObject> _ballList;
    public int PoolSize => _poolSize;
    int _poolSize;

    public void Initialize(int poolSize)
    {
        instance = this;
        _poolSize = poolSize;

        _ballList = new List<GameObject>();
        for (int i = 0; i < _poolSize; i++)
            AddItemToPool();
    }

    public void AddItemToPool()
    {
        GameObject ball = Instantiate(_ballPrefab);
        ball.SetActive(false);
        _ballList.Add(ball);
        ball.transform.parent = transform;
    }

    public GameObject RequestItem()
    {
        for (int i = 0; i < _ballList.Count; i++)
        {
            if (!_ballList[i].activeSelf)
            {
                _ballList[i].SetActive(true);
                return _ballList[i];
            }
        }
        return null;
    }
}
