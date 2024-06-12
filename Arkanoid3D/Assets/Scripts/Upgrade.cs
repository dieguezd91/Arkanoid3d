using UnityEngine;

public class Upgrade : MonoBehaviour
{
    protected GameManager gameManager;

    float _upgradeStartTime;
    bool _isUpgradeActive;

    [Header("Data")]
    [SerializeField] string upgradeName;
    [SerializeField] int speed;
    Vector3 _movement;
    [SerializeField] float upgradeDuration;

    //AUDIO
    AudioSource _audioSource;
    [SerializeField] AudioClip SFX;

    public virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.instance;
    }

    public void UpdateUpgrade()
    {
        if (!_isUpgradeActive) transform.position += Vector3.back * speed * Time.deltaTime;
        else if (CheckUpgradeFinishTime()) EndUpgrade();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!_isUpgradeActive)
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
    }

    public virtual void ApplyUpgrade()
    {
        _audioSource.PlayOneShot(SFX);
        _upgradeStartTime = Time.time;
        _isUpgradeActive = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    public virtual void EndUpgrade() => DestroyUpgrade(); 

    bool CheckUpgradeFinishTime() => Time.time >= _upgradeStartTime + upgradeDuration;

    public void DestroyUpgrade()
    {
        gameManager.Upgrades.Remove(this);
        gameObject.SetActive(false);
    }
}