using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Upgrade
{
    private Ball _ball;
    public override void ApplyUpgrade()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        _ball = ball.GetComponent<Ball>();

        if (_ball.isMagnetEnabled())
        {
            Destroy(gameObject);
            return;
        }

        base.ApplyUpgrade();
        _ball.EnableMagnet();

    }
    public override void EndUpgrade()
    {
        base.EndUpgrade();
        _ball.DisableMagnet();
    }
}
