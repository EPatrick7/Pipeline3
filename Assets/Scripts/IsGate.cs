using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGate : MonoBehaviour
{

    public Sprite On;
    public Sprite Off;
    public bool DefaultState;
    void Update()
    {
        if(IsButton.ButtonState!=DefaultState)
        {
            this.GetComponent<SpriteRenderer>().sprite = Off;
            this.GetComponent<Collider2D>().enabled = false;
        }
        else
        {

            this.GetComponent<SpriteRenderer>().sprite = On;
            this.GetComponent<Collider2D>().enabled = true;
        }
    }
}
