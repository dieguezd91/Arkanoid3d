using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Vector3 velocity;
    private Vector3 direction; // Dirección de movimiento de la pelota
    private bool isMoving = false; // bool para controlar si la pelota está en movimiento

    void Start()
    {
        // Inicializar la dirección de la pelota al azar solo una vez
        direction = new Vector3(Random.Range(-1f, 1f), 0f, 1f).normalized;
    }

    private void Update()
    {
        if (!isMoving && Input.GetKeyDown(KeyCode.Space))
        {
            StartMoving();
        }

        if (isMoving)
        {
            // Mover la pelota en su dirección a la velocidad especificada
            transform.Translate(direction * speed * Time.deltaTime);

        }
    }

    void StartMoving()
    {
        isMoving = true;
    }


    void OnCollisionStay(Collision collision)
    {
        // Cambiar la dirección de la pelota al colisionar con otro objeto
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        direction.y = 0f; // Mantener la dirección en el eje Y en 0 para evitar movimientos verticales
        direction = direction.normalized; // Normalizar la dirección
    }
}
