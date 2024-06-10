using UnityEngine;

public class Upgrade : MonoBehaviour
{
    protected GameManager gameManager;
    Rigidbody _rb;

    float _upgradeStartTime;
    bool _isUpgradeActive;

    [Header("Data")]
    [SerializeField] string upgradeName;
    [SerializeField] int speed;
    [SerializeField] float upgradeDuration;

    //AUDIO
    AudioSource _audioSource;
    [SerializeField] AudioClip SFX;

    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
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
        _audioSource.PlayOneShot(SFX);
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