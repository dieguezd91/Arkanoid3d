using UnityEngine;

public class ExpandBarUpgrade : Upgrade
{
    [SerializeField] private float expandedScale = 2.0f;
    private PlayerController playerController;
    [SerializeField] AudioClip SFX;

    public override void ApplyUpgrade()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        AudioSource.PlayOneShot(SFX);
        if (playerController.IsBarExpanded())
        {
#if UNITY_EDITOR
            Debug.Log("Bar is already expanded. Upgrade ignored.");
#endif
            Destroy(gameObject);
            return;
        }


        base.ApplyUpgrade();
        playerController.ExpandBar(expandedScale);
#if UNITY_EDITOR
        Debug.Log("Bar expanded!");
#endif
    }

    public override void EndUpgrade()
    {
        if (playerController != null)
        {
            playerController.ShrinkBar(expandedScale);

#if UNITY_EDITOR
            Debug.Log("Bar restored to original size.");
#endif
        }

        base.EndUpgrade();
    }
}
