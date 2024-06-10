using System;
using System.Collections;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int HP;
    bool _destroying;
    [HideInInspector] public string color;
    [SerializeField] GameObject[] _upgrades;
        
    Renderer _renderer;
    [SerializeField] Material _redMat;
    [SerializeField] Material _orangeMat;
    [SerializeField] Material _yellowMat;


    [Header("SFX")]
    [SerializeField] AudioClip breakSFX;
    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponent<Renderer>();

        switch (color)
        {
            case "Red":
                HP = 3;
                _renderer.material = _redMat;
                break;
            case "Orange":
                HP = 2;
                _renderer.material = _orangeMat;
                break;
            case "Yellow":
                HP = 1;
                _renderer.material = _yellowMat;
                break;
            default:
                HP = 1;
                _renderer.material = _yellowMat;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball") && !_destroying) TakeDamage();     
    }

    void TakeDamage()
    {
        HP--;
        if (HP <= 0) StartCoroutine(DestroyBrick());
    }

    private IEnumerator DestroyBrick()
    {
        _audioSource.PlayOneShot(breakSFX);
        _destroying = true;
        TrySpawnUpgrade();
        GameManager.instance.bricksLeft--;
        yield return new WaitForSeconds(breakSFX.length);
        gameObject.SetActive(false);
    }

    private void TrySpawnUpgrade()
    {
        int n = UnityEngine.Random.Range(0, 10);
        if (n < 3)
        {
            GameObject newUpgrade = Instantiate(CreateUpgrade(), transform.position, transform.rotation);
            GameManager.instance.Upgrades.Add(newUpgrade.GetComponent<Upgrade>());
        }
    }

    GameObject CreateUpgrade() => _upgrades[UnityEngine.Random.Range(0, _upgrades.Length)];
}
