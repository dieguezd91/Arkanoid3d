using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandBar : Upgrade
{
    [SerializeField] float expandedScale = 2.0f;
    private Transform playerTransform;
    private Vector3 originalScale;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        originalScale = playerTransform.localScale;

        playerTransform.localScale = new Vector3(originalScale.x * expandedScale, originalScale.y, originalScale.z);

#if UNITY_EDITOR
        Debug.Log("Bar expanded");
#endif
    }

    public override void EndUpgrade()
    {
        if (playerTransform != null)
        {
            playerTransform.localScale = originalScale;

#if UNITY_EDITOR
            Debug.Log("Bar restored to original size");
#endif
        }

        base.EndUpgrade();
    }
}
