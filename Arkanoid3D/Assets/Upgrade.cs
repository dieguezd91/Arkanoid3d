using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] string upgradeName;
    [SerializeField] float speed;
    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        SetVelocity();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            ApplyUpgrade();
        else if (collision.gameObject.CompareTag("DeadZone"))
            DestroyUpgrade();
    }

    void SetVelocity()                //CONSULTAR SI DEFINIR Y DECLARAR RB SERÍA MEJOR
    {
        _rb.velocity = Vector3.back * speed;
    }

    void ApplyUpgrade()
    {
#if UNITY_EDITOR
        Debug.Log($"Upgrade: {upgradeName}");
#endif
        Destroy(gameObject);
    }

    void DestroyUpgrade()
    {
#if UNITY_EDITOR
        Debug.Log($"{upgradeName} lost");
#endif
        Destroy(gameObject);
    }
}
