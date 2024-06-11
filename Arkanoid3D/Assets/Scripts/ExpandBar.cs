using UnityEngine;

public class ExpandBarUpgrade : Upgrade
{
    [SerializeField] float expandedScale;
    PlayerController playerController;

    public override void ApplyUpgrade()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerController.IsBarExpanded())
        {
            Destroy(gameObject);
            return;
        }

        base.ApplyUpgrade();
        playerController.ExpandBar(expandedScale);
    }

    public override void EndUpgrade()
    {
        if (playerController != null) playerController.ShrinkBar(expandedScale);
        base.EndUpgrade();
    }
}
