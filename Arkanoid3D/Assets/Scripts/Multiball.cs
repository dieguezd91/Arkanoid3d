using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiball : Upgrade
{
    [SerializeField] int ballsToSpawn;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();

        for(int n = 0; n < ballsToSpawn; n++)
        {
            GameObject newBall = BallPool.instance.RequestBall();
            newBall.transform.position = GameManager.instance.Balls[0].transform.position;
            GameManager.instance.Balls.Add(newBall);
            Ball newBallScript = newBall.GetComponent<Ball>();
            newBallScript.SetNewDirection();
        }
    }
}
