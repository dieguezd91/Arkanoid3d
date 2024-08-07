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
            Ball newBall = gameManager.BallPool.RequestBall().GetComponent<Ball>();
            newBall.transform.position = gameManager.Balls[0].transform.position;
            newBall.SetNewDirection();  
        }
    }
}
