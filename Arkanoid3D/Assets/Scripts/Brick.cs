using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Destroy(gameObject, .3f);
    }

    private void TrySpawnUpgrade()
    {
        int n = Random.Range(0, 10);
        if (n < 3) Instantiate(CreateUpgrade(), transform.position, transform.rotation);
    }

    GameObject CreateUpgrade()
    {
        GameObject newUpgrade;
        int r = Random.Range(0, 100);
        if (r > 75) newUpgrade = _upgrades[0];
        else if (r > 50) newUpgrade = _upgrades[1];
        else if (r > 25) newUpgrade = _upgrades[2];
        else newUpgrade = _upgrades[3];
        return newUpgrade;
    }
}
