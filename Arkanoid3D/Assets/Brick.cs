using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int HP;
    MeshRenderer _renderer;
    string materialName;

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

    private void Update()
    {
        if (HP <= 0)
            DestroyBrick();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage();      
        }
    }

    void TakeDamage()
    {
        HP--; // Reducir los puntos de vida
    }

    private void DestroyBrick()
    {
        Destroy(gameObject, .3f);
    }
}
