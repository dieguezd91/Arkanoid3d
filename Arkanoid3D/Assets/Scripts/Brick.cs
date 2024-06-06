using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int HP;
    MeshRenderer _renderer;
    string materialName;
    [SerializeField] GameObject[] _upgrades;

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
                HP = 1; // Valor predeterminado si el material no coincide con ninguno de los casos anteriores
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball")) TakeDamage();     
    }

    void TakeDamage()
    {
        HP--; // Reducir los puntos de vida
        if (HP <= 0) DestroyBrick();
    }

    private void DestroyBrick()
    {
        TrySpawnUpgrade();
        Destroy(gameObject, .3f);
    }

    private void TrySpawnUpgrade()
    {
        int n = Random.Range(0, 10);
        if (n < 3) Instantiate(CreateUpgrade(), transform.position, transform.rotation);
    }

    GameObject CreateUpgrade() => _upgrades[Random.Range(0, _upgrades.Length)];
}
