using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Vector3 velocity;

    void Start()
    {
        velocity.x = Random.Range(-1f, 1f);

        velocity.z = 1;

        rb.AddForce(velocity * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            GameManager.instance.Lose();
        }

        if(collision.gameObject.CompareTag("Brick"))
        {
            velocity.z *= -1;
        }
    }
}
