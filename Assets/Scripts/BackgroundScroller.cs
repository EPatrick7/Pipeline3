using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float Speed=1;
    private void FixedUpdate()
    {
        this.transform.position += new Vector3(Speed/1000f, 0, 0);
        if (this.transform.position.x >= 100)
            this.transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }
}
