using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Upgrade
{
    public override void ApplyUpgrade()
    {
        if (GameManager.instance.PlayerController.IsMagnetEnabled())
        {
            gameObject.SetActive(false);
            return;
        }

        base.ApplyUpgrade();
        GameManager.instance.PlayerController.EnableMagnet();
    }

    public override void EndUpgrade()
    {
        base.EndUpgrade();
        GameManager.instance.PlayerController.DisableMagnet();
    }


}
