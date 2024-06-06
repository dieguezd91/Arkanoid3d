using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] LayerMask ballsLayer;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)
        {
            GameManager.instance.Balls.Remove(other.gameObject.GetComponent<Ball>());
            other.gameObject.SetActive(false);
        }
    }
}
