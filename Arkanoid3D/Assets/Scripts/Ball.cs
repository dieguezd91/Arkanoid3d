using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    Rigidbody _rb;
    Vector3 direction;

    [Header("Speed values")]
    [SerializeField] int speed;
    [SerializeField] int minSpeed;
    [SerializeField] int maxSpeed;

    //MAGNET
    public bool MagnetActive => magnetActive;
    bool magnetActive;
    bool catchedBall;
    Transform magnetPos;

    [Header("AUDIO")]
    [SerializeField] AudioClip _ballLostSFX;
    [SerializeField] AudioClip _wallBounce;
    [SerializeField] AudioClip _playerBounce;
    AudioSource _audioSource;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        magnetPos = GameObject.Find("MagnetPos").GetComponent<Transform>();
        SetNewDirection(); // Establecer una dirección inicial para la pelota
    }

    public void UpdateBall()
    {
        _rb.velocity = direction * Mathf.Clamp(speed, minSpeed, maxSpeed); // Mantener la velocidad dentro de los límites
        if (catchedBall) MagnetLogic();
    }

    public void SetNewDirection()
    {
        direction = new Vector3(Random.Range(-1f, 1f), 0, 1).normalized; // Dirección inicial con un ligero ángulo
        speed = maxSpeed; // Establecer la velocidad inicial al máximo
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!magnetActive) Bounce(collision);
            else
            {
                _rb.velocity = Vector3.zero;
                speed = 0;
                catchedBall = true;
            }
        }
        else if (collision.gameObject.CompareTag("DeadZone"))
        {
            _audioSource.PlayOneShot(_ballLostSFX);
            BallPool.instance.RemoveItem(collision.gameObject.GetComponent<Ball>());
        }
        else Bounce(collision);
    }

    private void Bounce(Collision collision)
    {
        // Cambiar la dirección de la pelota al colisionar con otro objeto
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        direction.y = 0;
        Vector3.Normalize(direction); // Mantener la dirección en el eje Y en 0 y normalizar

        // Asegurarse de que el ángulo de rebote no sea demasiado plano
        if (Mathf.Abs(direction.z) < 0.1f)
        {
            direction.z = direction.z > 0 ? 0.1f : -0.1f; // Ajustar el ángulo de rebote
            direction = direction.normalized; // Normalizar la dirección ajustada
        }
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed); // Asegurarse de que la velocidad este dentro de los limites

        if(collision.gameObject.tag == "Player")
            _audioSource.PlayOneShot(_playerBounce);
        else if(collision.gameObject.tag == "Wall")
            _audioSource.PlayOneShot(_wallBounce);
    }

    public void ManageMagnetState() => magnetActive = !magnetActive;
    public void MagnetLogic()
    {
        transform.position = magnetPos.position;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetNewDirection();
            speed = maxSpeed; // Restablecer la velocidad al maximo al liberar la pelota
            catchedBall = false;
        }
    }
}
