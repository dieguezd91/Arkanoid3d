using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Upgrade
{
    PlayerController playerController;
    public override void ApplyUpgrade()
    {
        playerController = GameManager.instance.PlayerScript;
        if (playerController.IsMagnetActive)
        {
            DestroyUpgrade();
            return;
        }

        base.ApplyUpgrade();
        playerController.ManageMagnetState();

    }
    public override void EndUpgrade()
    {
        playerController.ManageMagnetState();
        base.EndUpgrade();
    }
}
