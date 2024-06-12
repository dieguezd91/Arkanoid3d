using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody RB => _rb;
    Rigidbody _rb;
    public float speed;
    public float minSpeed = 10f; // Velocidad mínima de la pelota
    public float maxSpeed = 25f; // Velocidad máxima de la pelota
    public Vector3 velocity;
    public Vector3 direction; // Dirección de movimiento de la pelota


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        SetNewDirection(); // Establecer una dirección inicial para la pelota
    }

    public void UpdateBall()
    {
        _rb.velocity = direction * Mathf.Clamp(speed, minSpeed, maxSpeed); // Mantener la velocidad dentro de los límites
        GameManager.instance.PlayerController.MagnetLogic();
    }

    public void SetNewDirection()
    {
        direction = new Vector3(Random.Range(-1f, 1f), 0, 1).normalized; // Dirección inicial con un ligero ángulo
        speed = maxSpeed; // Establecer la velocidad inicial al máximo
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Cambiar la dirección de la pelota al colisionar con otro objeto
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        direction = new Vector3(direction.x, 0, direction.z).normalized; // Mantener la dirección en el eje Y en 0 y normalizar

        // Asegurarse de que el ángulo de rebote no sea demasiado plano
        if (Mathf.Abs(direction.z) < 0.1f)
        {
            direction.z = direction.z > 0 ? 0.1f : -0.1f; // Ajustar el ángulo de rebote
            direction = direction.normalized; // Normalizar la dirección ajustada
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
