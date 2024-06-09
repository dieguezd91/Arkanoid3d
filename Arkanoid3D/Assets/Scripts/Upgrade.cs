using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] string upgradeName;
    [SerializeField] float speed;
    [SerializeField] float upgradeDuration;
    float _upgradeStartTime;
    private bool _isUpgradeActive = false;
    Rigidbody _rb;
    GameManager gameManager;

    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        gameManager = GameManager.instance;
        gameManager.Upgrades.Add(this);
    }

    public void UpdateUpgrade()
    {
        if (!_isUpgradeActive)
            if (_rb != null) _rb.velocity = Vector3.back * speed; 
        else if (CheckUpgradeFinishTime()) EndUpgrade();
    }

    void OnTriggerEnter(Collider collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Player":
                ApplyUpgrade();
                break;
            case "DeadZone":
                DestroyUpgrade();
                break;
        }
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

    bool CheckUpgradeFinishTime() => _isUpgradeActive && Time.time >= _upgradeStartTime + upgradeDuration;

    public void DestroyUpgrade()
    {
        gameManager.Upgrades.Remove(this);
        Destroy(gameObject);
    }
}