using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody RB => _rb;
    Rigidbody _rb;
    public float speed;
    public Vector3 velocity;
    public Vector3 direction; // Direcci�n de movimiento de la pelota


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void UpdateBall()
    {
        _rb.velocity = direction * speed;
        //transform.Translate(direction * speed * Time.deltaTime);                // Mover la pelota en su direcci�n a la velocidad especificada
    }

    public void SetNewDirection() => direction = new Vector3(Random.Range(-1f, 1f), 0f, 1f).normalized;

    private void OnCollisionEnter(Collision collision)
    {
        // Cambiar la direcci�n de la pelota al colisionar con otro objeto
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        direction.y = 0f; // Mantener la direcci�n en el eje Y en 0 para evitar movimientos verticales
        direction = direction.normalized; // Normalizar la direcci�n

        if (collision.gameObject.tag == "DeadZone")
        {
            GameManager.instance.Balls.Remove(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, direction * speed);
    }
}
