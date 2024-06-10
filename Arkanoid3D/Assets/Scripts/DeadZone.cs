using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] LayerMask ballsLayer;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball") BallPool.instance.RemoveItem(other.gameObject.GetComponent<Ball>());
    }
}
