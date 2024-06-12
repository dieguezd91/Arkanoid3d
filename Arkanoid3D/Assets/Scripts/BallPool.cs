using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    public static BallPool instance;
    [SerializeField] GameObject _ballPrefab;
    [SerializeField] List<GameObject> _ballList;
    [SerializeField] int _poolSize;

    public void Initialize()
    {
        instance = this;
        _ballList = new List<GameObject>();
        for (int i = 0; i < _poolSize; i++)
            AddBallToPool();
    }

    public void AddBallToPool()
    {
        GameObject ball = Instantiate(_ballPrefab, transform);
        ball.SetActive(false);
        _ballList.Add(ball);
    }

    public GameObject RequestBall()
    {
        for (int i = 0; i < _ballList.Count; i++)
        {
            if (!_ballList[i].activeSelf)
            {
                GameObject ball = _ballList[i];

                GameManager.instance.Balls.Add(ball.GetComponent<Ball>());
                ball.SetActive(true);
                return ball;
            }
        }
        return null;
    }

    public void RemoveItem(Ball ballToRemove)
    {
        GameManager.instance.Balls.Remove(ballToRemove);
        ballToRemove.gameObject.SetActive(false);
    }
}
