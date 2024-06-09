using UnityEngine;

public class ExpandBarUpgrade : Upgrade
{
    [SerializeField] private float expandedScale = 2.0f;
    private PlayerController playerController;

    public override void ApplyUpgrade()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

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
        if (playerController != null)
        {
            playerController.ShrinkBar(expandedScale);
        }

        base.EndUpgrade();
    }
}
