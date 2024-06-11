using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Upgrade
{
    private Ball _ball;
    public override void ApplyUpgrade()
    {
        _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        if (_ball.MagnetActive)
        {
            Destroy(gameObject);
            return;
        }

        base.ApplyUpgrade();
        _ball.ManageMagnetState();

    }
    public override void EndUpgrade()
    {
        _ball.ManageMagnetState();
        base.EndUpgrade();
    }
}
