using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiball : Upgrade
{
    [SerializeField] int ballsToSpawn;
    [SerializeField] AudioClip SFX;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        AudioSource.PlayOneShot(SFX);
        for(int n = 0; n < ballsToSpawn; n++)
        {
            Ball newBall = gameManager.BallPool.RequestItem().GetComponent<Ball>();
            newBall.transform.position = gameManager.Balls[0].transform.position;
            gameManager.Balls.Add(newBall);
            newBall.SetNewDirection();  
        }
    }
}
