using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTransition : MonoBehaviour
{
    public static bool TransitionDone;
    public static BoxTransition me;
    public bool Shrink;
    public float Val = 1;
    public static bool FirstOpen;
    void Start()
    {
        TransitionDone = false;

        me = this;

        foreach (Transform c in this.transform)
        {
            c.transform.localScale = new Vector3(Val, Val, Val);
        }
        if (!FirstOpen)
        {
            vs = Shrink;
            FirstOpen = true;
            Val = 0;
            foreach (Transform c in this.transform)
            {
                c.transform.localScale = new Vector3(Val, Val, Val);
                    if(this.transform.childCount<=1)
                c.GetComponent<SpriteRenderer>().color = new Color(c.GetComponent<SpriteRenderer>().color.r, c.GetComponent<SpriteRenderer>().color.g, c.GetComponent<SpriteRenderer>().color.b, Val);
            }
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, (1 - Val) * 45);
        }
    }
    bool vs;
    
    void FixedUpdate()
    {
        float SPEED = 0.1f;
        if (Val <= 0)
            Val = 0;
        if (Val >= 1)
            Val = 1;

        if(vs!=Shrink)
        {
            vs = Shrink;
            if(Shrink)
            this.GetComponent<AudioSource>().Play();
        }

        TransitionDone = false;
        if(Shrink)
        {
            if (Val > 0)
            {
                Val -= SPEED;
            }
            else
                TransitionDone = true;
            foreach (Transform c in this.transform)
            {
                c.transform.localScale = new Vector3(Val, Val, Val);
                if (this.transform.childCount <= 1)
                    c.GetComponent<SpriteRenderer>().color = new Color(c.GetComponent<SpriteRenderer>().color.r, c.GetComponent<SpriteRenderer>().color.g, c.GetComponent<SpriteRenderer>().color.b, Val);
            }

            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, (1-Val) * 45);
        }
        else
        {
            if (Val < 1)
                Val += SPEED;
            else
                TransitionDone = true;
            foreach (Transform c in this.transform)
            {
                c.transform.localScale = new Vector3(Val, Val, Val);
                if (this.transform.childCount <= 1)
                    c.GetComponent<SpriteRenderer>().color = new Color(c.GetComponent<SpriteRenderer>().color.r, c.GetComponent<SpriteRenderer>().color.g, c.GetComponent<SpriteRenderer>().color.b, Val);
            }
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, (1-Val) * 45);
        }
        if(SliderVal.SliderSource!=null)
        {
            if (SliderVal.SliderSource.isPlaying)
                TransitionDone = false;
        }
    }
}
