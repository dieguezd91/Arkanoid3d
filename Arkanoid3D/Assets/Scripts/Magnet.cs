using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Upgrade
{
    private Ball _ball;
    [SerializeField] AudioClip SFX;
    public override void ApplyUpgrade()
    {
        _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        AudioSource.PlayOneShot(SFX);
        if (_ball.isMagnetEnabled())
        {
            Debug.Log("Magnet is already active. Upgrade ignored");
            Destroy(gameObject);
            return;
        }

        base.ApplyUpgrade();
        _ball.EnableMagnet();
        Debug.Log("Magnet enabled");

    }
    public override void EndUpgrade()
    {
        _ball.DisableMagnet();
        base.EndUpgrade();
        Debug.Log("Magnet disabled");
    }
}
