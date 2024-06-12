using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Upgrade
{
    public override void ApplyUpgrade()
    {
        if (GameManager.instance.PlayerScript.IsMagnetActive)
        {
            DestroyUpgrade();
            return;
        }

        base.ApplyUpgrade();
        GameManager.instance.PlayerScript.ManageMagnetState();

    }
    public override void EndUpgrade()
    {
        GameManager.instance.PlayerScript.ManageMagnetState();
        base.EndUpgrade();
    }
}
