using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiball : Upgrade
{
    [SerializeField] int ballsToSpawn;
    [SerializeField] GameObject ballPrefab;
    GameObject _ball;

    public override void Start()
    {
        base.Start();
        _ball = GameManager.instance.Ball;
    }

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();

        for(int n = 0; n < ballsToSpawn; n++)
        {
            GameObject newBall = Instantiate(ballPrefab, _ball.transform.position, _ball.transform.rotation);
            GameManager.instance.ExtraBalls.Add(newBall);
            Ball newBallScript = newBall.GetComponent<Ball>();
            newBallScript.SetNewDirection();
        }
    }
}
