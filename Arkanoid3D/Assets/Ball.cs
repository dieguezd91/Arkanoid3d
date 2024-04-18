using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Vector3 velocity;
    public Vector3 direction; // Direcci�n de movimiento de la pelota

    public void UpdateBall()
    {
        transform.Translate(direction * speed * Time.deltaTime);                // Mover la pelota en su direcci�n a la velocidad especificada
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeadZone")) GameManager.instance.LooseRound();             //Si colisiona con la pared inferior, perder la ronda
        else                                                                                            //Cambiar la direcci�n de la pelota al colisionar con otro objeto
        {
            direction = Vector3.Reflect(direction, collision.contacts[0].normal);
            direction.y = 0f;                                                       // Mantener la direcci�n en el eje Y en 0 para evitar movimientos verticales
            direction = direction.normalized;                                       // Normalizar la direcci�n
        }
    }
}
