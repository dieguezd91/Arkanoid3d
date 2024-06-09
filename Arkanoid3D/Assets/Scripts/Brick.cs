using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int HP;
    MeshRenderer _renderer;
    string materialName;
    [SerializeField] GameObject[] _upgrades;
    private bool isDestroyed = false;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();

        materialName = _renderer.material.name;

        switch (materialName)
        {
            case string name when name.Contains("Red"):
                HP = 3;
                break;
            case string name when name.Contains("Orange"):
                HP = 2;
                break;
            case string name when name.Contains("Yellow"):
                HP = 1;
                break;
            default:
                HP = 1;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball")) TakeDamage();     
    }

    void TakeDamage()
    {
        HP--;
        if (HP <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            DestroyBrick();
        }
    }

    private void DestroyBrick()
    {
        TrySpawnUpgrade();
        GameManager.instance.bricksLeft--;
        Destroy(gameObject);
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
