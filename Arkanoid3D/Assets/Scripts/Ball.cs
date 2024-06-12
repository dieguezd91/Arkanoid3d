using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class Ball : MonoBehaviour
{
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
        _audioSource = GetComponent<AudioSource>();
        SetNewDirection(); // Establecer una dirección inicial para la pelota
    }

    public void UpdateBall()
    {
        transform.position += direction * Mathf.Clamp(speed, minSpeed, maxSpeed) * Time.deltaTime; // Mantener la velocidad dentro de los límites
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
}
