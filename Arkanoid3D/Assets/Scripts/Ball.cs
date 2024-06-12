using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody RB => _rb;
    Rigidbody _rb;
    public float speed;
    public float minSpeed = 10f; // Velocidad m�nima de la pelota
    public float maxSpeed = 25f; // Velocidad m�xima de la pelota
    public Vector3 velocity;
    public Vector3 direction; // Direcci�n de movimiento de la pelota


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        SetNewDirection(); // Establecer una direcci�n inicial para la pelota
    }

    public void UpdateBall()
    {
        _rb.velocity = direction * Mathf.Clamp(speed, minSpeed, maxSpeed); // Mantener la velocidad dentro de los l�mites
        GameManager.instance.PlayerController.MagnetLogic();
    }

    public void SetNewDirection()
    {
        direction = new Vector3(Random.Range(-1f, 1f), 0, 1).normalized; // Direcci�n inicial con un ligero �ngulo
        speed = maxSpeed; // Establecer la velocidad inicial al m�ximo
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Cambiar la direcci�n de la pelota al colisionar con otro objeto
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        direction = new Vector3(direction.x, 0, direction.z).normalized; // Mantener la direcci�n en el eje Y en 0 y normalizar

        // Asegurarse de que el �ngulo de rebote no sea demasiado plano
        if (Mathf.Abs(direction.z) < 0.1f)
        {
            direction.z = direction.z > 0 ? 0.1f : -0.1f; // Ajustar el �ngulo de rebote
            direction = direction.normalized; // Normalizar la direcci�n ajustada
        }

        speed = Mathf.Clamp(speed, minSpeed, maxSpeed); // Asegurarse de que la velocidad este dentro de los limites

        if (collision.gameObject.CompareTag("DeadZone"))
        {
            GameManager.instance.Balls.Remove(this);
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, direction * speed);
    }

}
