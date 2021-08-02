using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbineSpin : MonoBehaviour
{
    public float Velocity;
    private void FixedUpdate()
    {
        this.GetComponent<Rigidbody2D>().angularVelocity = Velocity;
    }
}
