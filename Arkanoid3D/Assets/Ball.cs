using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Vector3 velocity;
    public Vector3 direction; // Dirección de movimiento de la pelota

    public void UpdateBall()
    {
        transform.Translate(direction * speed * Time.deltaTime);                // Mover la pelota en su dirección a la velocidad especificada
    }

    public void SetNewDirection() => direction = new Vector3(Random.Range(-1f, 1f), 0f, 1f).normalized;

    private void OnCollisionEnter(Collision collision)
    {
        // Cambiar la dirección de la pelota al colisionar con otro objeto
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        direction.y = 0f; // Mantener la dirección en el eje Y en 0 para evitar movimientos verticales
        direction = direction.normalized; // Normalizar la dirección

    }

    public void DestroyBall()
    {
        if (tag == "ExtraBall")
        {
            GameManager.instance.ExtraBalls.Remove(gameObject);
            Destroy(gameObject);
        }
        else if (tag == "Ball") GameManager.instance.LoseRound();
    }
}
