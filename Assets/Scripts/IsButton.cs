using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsButton : MonoBehaviour
{
    public static bool ButtonState;
    public static int SupportOn;
    [HideInInspector]
    public bool g;
    public bool IsOn;
    bool b;
    public bool IsTouched;
    public static int NumButtons;
    public IEnumerator Delayed()
    {
        yield return new WaitForSecondsRealtime(1);
        IsTouched = false;
        g = false;  
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
       if(collision.gameObject.layer==9|| collision.gameObject.layer == 15)
        {
            if (!IsElectric)
            {
                StopAllCoroutines();
                g = false;
                IsTouched = true;
            }
        }
    }
   
    private void OnDestroy()
    {


        if(i)
        NumButtons--;
        if(b)
        SupportOn--;

        
        
            ButtonState = (SupportOn >= NumButtons);
    }

    public Sprite On;
    bool i;
    public Sprite Off;
    private void Start()
    {
        i = true;
        NumButtons++;
        IsOn = DefaultState;
    }
    public bool DefaultState = false;
    public AudioSource me;
    public bool IsElectric;
    void Update()
    {
        
        if (DefaultState)
            IsOn = !IsTouched;
        else
            IsOn = IsTouched;

        if(b!=IsOn)
        {
            if(me.enabled)
            me.Play();
            if (!DefaultState)
            {
                if (b)
                {
                    SupportOn--;
                }
                else
                    SupportOn++;

                b = IsOn;
            }
            else
            {

                if (b)
                {
                    SupportOn--;
                }
                else
                    SupportOn++;

                b = IsOn;
            }
        }

        if(!IsOn)
        {
            if(SupportOn<=0)
            ButtonState = false;
            this.GetComponent<SpriteRenderer>().sprite = Off;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = On;

            ButtonState = (SupportOn >= NumButtons);
        }


        if (!g)
        {

            g = true;
            StartCoroutine(Delayed());
        }
    }
}
