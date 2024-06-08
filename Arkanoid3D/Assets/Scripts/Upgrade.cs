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
    [SerializeField] Rigidbody _rb;

    public void UpdateUpgrade()
    {
        if(!_isUpgradeActive)
        {
            Debug.Log(_rb);
            _rb.velocity = Vector3.back * speed;
        }
        else if(CheckUpgradeFinishTime()) EndUpgrade();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) ApplyUpgrade();
        else if (!_isUpgradeActive && collision.gameObject.CompareTag("DeadZone")) DestroyUpgrade();
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
        //DestroyUpgrade();
    }

    bool CheckUpgradeFinishTime() => _isUpgradeActive && Time.time >= _upgradeStartTime + upgradeDuration;

    public void DestroyUpgrade()
    {
        GameManager.instance.Upgrades.Remove(this);
        Destroy(gameObject);
    }
}
