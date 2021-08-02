using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLimiter : MonoBehaviour
{
    public bool IsWall;
    public Quaternion Start;
    public bool IsDucky;
    private void Awake()
    {
        if(this.GetComponent<BoxCollider2D>().size.y== 0.08716691f)
        {
            this.GetComponent<BoxCollider2D>().size= new Vector2(this.GetComponent<BoxCollider2D>().size.x, 0.2203771f);
            this.GetComponent<BoxCollider2D>().offset= new Vector2(this.GetComponent<BoxCollider2D>().offset.x, -0.1119987f);
        }
        Start = this.transform.rotation;
    }
    private void FixedUpdate()
    {
        if(IsWall)
        {
            this.transform.localPosition = Vector3.zero;
        }
        if (IsDucky)
        {

            if (Vector2.Distance(this.transform.position, Camera.main.transform.position) > Camera.main.orthographicSize + 10)
            {
                this.transform.localPosition = Vector2.zero;
                this.transform.localEulerAngles = Vector3.zero;
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                this.GetComponent<Rigidbody2D>().angularVelocity = 0;
            }
        }

        if (Mathf.Abs(Quaternion.Angle(this.transform.rotation, Start)) > 10)
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Start, 0.1f);
    }
}
