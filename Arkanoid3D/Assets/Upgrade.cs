using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    //TODO: APLICAR DIFERENTES VALORES DE CADA UPGRADE CON SCRIPTABLES
    [SerializeField] string upgradeName;
    [SerializeField] float speed;
    [SerializeField] float upgradeDuration;
    float _upgradeStartTime;
    private bool _isUpgradeActive = false;

    public virtual void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.back * speed;
    }

    private void Update()
    {
        if (_isUpgradeActive && CheckUpgradeFinishTime()) EndUpgrade();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) ApplyUpgrade();
        else if (collision.gameObject.CompareTag("DeadZone")) DestroyUpgrade();
    }

    public virtual void ApplyUpgrade()
    {
        _upgradeStartTime = Time.time;
        _isUpgradeActive = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

#if UNITY_EDITOR
        Debug.Log($"Upgrade started: {upgradeName}");
#endif
    }

    public virtual void EndUpgrade()
    {
#if UNITY_EDITOR
        Debug.Log($"Upgrade ended: {upgradeName}");
#endif
        _isUpgradeActive = false;
        DestroyUpgrade();
    }

    bool CheckUpgradeFinishTime() => _upgradeStartTime != 0 && Time.time >= _upgradeStartTime + upgradeDuration;

    public void DestroyUpgrade() => Destroy(gameObject);
}
