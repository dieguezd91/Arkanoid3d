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
            GameObject go = other.gameObject;
            GameManager.instance.Balls.Remove(go);
            Destroy(go);
        }

    }
}
