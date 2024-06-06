using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    public static BallPool instance;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private List<GameObject> ballList;
    [SerializeField] public int poolsize;

    public void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddBallsToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.SetActive(false);
            ballList.Add(ball);
            ball.transform.parent = transform;
        }
    }

    public GameObject RequestBall()
    {
        for (int i = 0; i < ballList.Count; i++)
        {
            if (!ballList[i].activeSelf)
            {
                ballList[i].SetActive(true);
                return ballList[i];
            }
        }
        return null;
    }
}
