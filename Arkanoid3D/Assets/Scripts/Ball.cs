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
    public Vector3 direction; // Dirección de movimiento de la pelota

    private bool magnetActive = false;
    private bool catchedBall = false;
    [SerializeField] Transform magnetPos;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        magnetPos = GameObject.Find("MagnetPos").GetComponent<Transform>();
    }

    public void UpdateBall()
    {
        _rb.velocity = direction * speed;
        MagnetLogic();
        //transform.Translate(direction * speed * Time.deltaTime);                // Mover la pelota en su dirección a la velocidad especificada
    }

    public void SetNewDirection() => direction = new Vector3(Random.Range(-1f, 1f), 0f, 1f).normalized;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && magnetActive)
        {
            Debug.Log("Catching the ball");
            _rb.velocity = Vector3.zero;
            speed = 0f;
            catchedBall = true;
        }
        // Cambiar la dirección de la pelota al colisionar con otro objeto
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        direction.y = 0f; // Mantener la dirección en el eje Y en 0 para evitar movimientos verticales
        direction = direction.normalized; // Normalizar la dirección

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

    public void EnableMagnet()
    {
        magnetActive = true;
    }
    public void DisableMagnet()
    {
        magnetActive = false;
    }
    public bool isMagnetEnabled()
    {
        return magnetActive;
    }
    public void MagnetLogic()
    {
        if (catchedBall)
        {
            transform.position = magnetPos.position;
        }
        if (catchedBall && Input.GetKeyDown(KeyCode.Space))
        {
            SetNewDirection();
            speed = 25;
            catchedBall = false;
        }
    }
}
