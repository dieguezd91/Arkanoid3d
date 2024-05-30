using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] LayerMask ballsLayer;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)
            other.gameObject.GetComponent<Ball>().DestroyBall();
    }
}
