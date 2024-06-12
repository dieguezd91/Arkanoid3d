using UnityEngine;

public class ExpandBar : Upgrade
{
    [SerializeField] float expandedScale;
    PlayerController playerController;

    public override void ApplyUpgrade()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerController.IsBarExpanded)
        {
            gameObject.SetActive(false);
            return;
        }

        base.ApplyUpgrade();
        playerController.ManageBarSize(expandedScale);
    }

    public override void EndUpgrade()
    {
        if (playerController != null) playerController.ManageBarSize(expandedScale);
        base.EndUpgrade();
    }
}
