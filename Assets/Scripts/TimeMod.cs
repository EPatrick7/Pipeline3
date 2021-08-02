using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TimeMod : MonoBehaviour
{
    public static float MaxTime=1;
    public float Strength = 0.5f;
    
    public AudioClip Clip;
    public AudioClip Clip2;
    bool ValAdded;
   
    
    private void OnDestroy()
    {
        //Debug.Log(MaxTime+" , "+Strength);
        if(isApplied)
        {
            if (ValAdded)
            {
                MaxTime += Strength;
                Time.timeScale = Mathf.Max(TIMEMIN, Mathf.Min(1, MaxTime));
            }
            if(this.transform.parent!=null)
            {
                if(this.transform.parent.GetComponent<AudioSource>()!=null)
                {
                    this.transform.parent.GetComponent<AudioSource>().clip = Clip;
                    this.transform.parent.GetComponent<AudioSource>().pitch = 2;
                    this.transform.parent.GetComponent<AudioSource>().Play();
                }
            }
        }
        else
        {
            if (ValAdded)
            {
              //  MaxTime -= Strength;
             //   Time.timeScale = Mathf.Max(TIMEMIN, Mathf.Min(1, MaxTime));
            }
            if (this.transform.parent != null)
            {
                if (this.transform.parent.GetComponent<AudioSource>() != null)
                {
                    this.transform.parent.GetComponent<AudioSource>().clip = Clip2;
                    this.transform.parent.GetComponent<AudioSource>().pitch = 2;
                    if (this.transform.parent.GetComponent<AudioSource>().enabled)
                    this.transform.parent.GetComponent<AudioSource>().Play();
                }
            }
        }
    }
    float TIMEMIN = 0.05f;
    private void Start()
    {
        ValAdded = false;
        isApplied = false;
        Circlez = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
    }
    bool isApplied;
    SpriteRenderer Circlez;
    void Update()
    {
        if(Circlez!=null)
        {
            if(isApplied)
            {
                if(Strength>=0)
                Circlez.transform.localScale = Vector3.Lerp(Circlez.transform.localScale,new Vector3(1000f,1000f,1000f), 0.01f);
                else
                    Circlez.transform.localScale = Vector3.Lerp(Circlez.transform.localScale, new Vector3(0.1f, 0.1f, 0.1f), 0.05f);
            }
            else
            {
                if(Strength>=0)
                Circlez.transform.localScale = Vector3.Lerp(Circlez.transform.localScale,new Vector3(0.1f, 0.1f, 0.1f), 0.05f);
                else
                    Circlez.transform.localScale = Vector3.Lerp(Circlez.transform.localScale, new Vector3(1000f, 1000f, 1000f), 0.01f);
            }
        }

        if (IsButton.ButtonState||IsButton.NumButtons<=0)
        {
            if(!isApplied)
            {
                ValAdded = true;
                this.GetComponent<AudioSource>().clip = Clip2;
                if(this.GetComponent<AudioSource>().enabled)
                this.GetComponent<AudioSource>().Play();
                MaxTime -= Strength;
                Time.timeScale = Mathf.Max(TIMEMIN, Mathf.Min(1, MaxTime));
                isApplied = true;
            }
        }
        else
        {
            if (isApplied)
            {
                ValAdded = true;
                this.GetComponent<AudioSource>().clip = Clip;
                if (this.GetComponent<AudioSource>().enabled)
                    this.GetComponent<AudioSource>().Play();

                MaxTime += Strength;
                Time.timeScale = Mathf.Max(TIMEMIN, Mathf.Min(1, MaxTime));
                isApplied = false;
            }
        }
    }
}
