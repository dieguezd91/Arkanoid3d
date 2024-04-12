using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Vector3 velocity;
    private Vector3 direction; // Dirección de movimiento de la pelota
    private bool isMoving = false; // Flag para controlar si la pelota está en movimiento

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

            //// Detectar colisiones con los bordes y cambiar la dirección
            //CheckCollisionWithBorders();
        }
    }

    void StartMoving()
    {
        isMoving = true;
    }

    //void CheckCollisionWithBorders()
    //{
    //    // Obtener los límites de la pantalla
    //    float minX = -10f;
    //    float maxX = 10f;
    //    float minZ = -5f;
    //    float maxZ = 5f;

    //    // Verificar colisión con bordes y cambiar la dirección
    //    if (transform.position.x < minX || transform.position.x > maxX)
    //    {
    //        direction.x = -direction.x;
    //    }

    //    if (transform.position.z < minZ || transform.position.z > maxZ)
    //    {
    //        direction.z = -direction.z;
    //    }
    //}

    void OnCollisionStay(Collision collision)
    {
        // Cambiar la dirección de la pelota al colisionar con otro objeto
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        direction.y = 0f; // Mantener la dirección en el eje Y en 0 para evitar movimientos verticales
        direction = direction.normalized; // Normalizar la dirección
    }
}
