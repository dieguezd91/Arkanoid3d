using UnityEngine;

public class ExpandBar : Upgrade
{
    [SerializeField] float expandedScale;
    PlayerController playerController;

    public override void ApplyUpgrade()
    {
        playerController = GameManager.instance.PlayerScript;
        if (playerController.IsBarExpanded)
        {
            DestroyUpgrade();
            return;
        }

        base.ApplyUpgrade();
        playerController.ManageBarSize(expandedScale);
    }

    public override void EndUpgrade()
    {
        playerController.ManageBarSize(expandedScale);
        base.EndUpgrade();
    }
}
