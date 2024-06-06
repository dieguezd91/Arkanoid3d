using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] string upgradeName;
    [SerializeField] float speed;
    [SerializeField] float upgradeDuration;
    float _upgradeStartTime;
    private bool _isUpgradeActive = false;
    Rigidbody _rb;

    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        GameManager.instance.Upgrades.Add(this);
    }

    public void UpdateUpgrade()
    {
        if(!_isUpgradeActive) _rb.velocity = Vector3.back * speed;
        else if(_isUpgradeActive && CheckUpgradeFinishTime()) EndUpgrade();
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
    }

    public virtual void EndUpgrade()
    {
        _isUpgradeActive = false;
        DestroyUpgrade();
    }

    bool CheckUpgradeFinishTime() => _upgradeStartTime != 0 && Time.time >= _upgradeStartTime + upgradeDuration;

    public void DestroyUpgrade()
    {
        GameManager.instance.Upgrades.Remove(this);
        Destroy(gameObject);
    }
}
