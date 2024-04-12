using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int HP;


    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        string materialName = renderer.material.name;

        if (materialName.Contains("Red"))
        {
            HP = 3;
        }
        else if (materialName.Contains("Orange"))
        {
            HP = 2;
        }
        else if (materialName.Contains("Yellow"))
        {
            HP = 1;
        }
        else
        {
            HP = 1; // Valor predeterminado si el material no coincide con ninguno de los casos anteriores
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
