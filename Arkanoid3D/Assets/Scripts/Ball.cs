using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    Rigidbody _rb;
    Vector3 direction;

    [Header("Speed values")]
    [SerializeField] int speed;
    [SerializeField] int minSpeed;
    [SerializeField] int maxSpeed;


    [Header("AUDIO")]
    [SerializeField] AudioClip _ballLostSFX;
    [SerializeField] AudioClip _wallBounce;
    [SerializeField] AudioClip _playerBounce;
    AudioSource _audioSource;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!GameManager.instance.PlayerScript.IsMagnetActive) Bounce(collision);
            else GameManager.instance.PlayerScript.CatchBall(this);
        }
        else if (collision.gameObject.CompareTag("DeadZone"))
        {
            _audioSource.PlayOneShot(_ballLostSFX);
            BallPool.instance.RemoveItem(this);
        }
        else Bounce(collision);
    }

    private void Bounce(Collision collision)
    {
        // Cambiar la direcci�n de la pelota al colisionar con otro objeto
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        direction.y = 0;
        Vector3.Normalize(direction); // Mantener la direcci�n en el eje Y en 0 y normalizar

        // Asegurarse de que el �ngulo de rebote no sea demasiado plano
        if (Mathf.Abs(direction.z) < 0.1f)
        {
            direction.z = direction.z > 0 ? 0.1f : -0.1f; // Ajustar el �ngulo de rebote
            direction = direction.normalized; // Normalizar la direcci�n ajustada
        }
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed); // Asegurarse de que la velocidad este dentro de los limites

        if(collision.gameObject.tag == "Player")
            _audioSource.PlayOneShot(_playerBounce);
        else if(collision.gameObject.tag == "Wall")
            _audioSource.PlayOneShot(_wallBounce);
    }
}
