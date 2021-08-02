using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeGrow : MonoBehaviour
{
    public Gradient Color;
    public bool Inverted;
    public float WaterValue;
    bool g;
    public float MaxVal = 3;
    IEnumerator DelayedUpdate()
    {
        yield return new WaitForSeconds(0.25f);
        g = false;
    }
    void FixedUpdate()
    {
        if(!g)
        {
            g = true;
            StartCoroutine(DelayedUpdate());
            WaterValue -= 0.25f;
            WaterValue = Mathf.Max(0, WaterValue);
        }
        if(!Water.ShowTemp)
        this.GetComponent<SpriteRenderer>().color = Color.Evaluate(WaterValue / (MaxVal * 10));
        float val= 1 * Mathf.Min(WaterValue, MaxVal*10) / 10f;

        if(Inverted)
        {
            val = MaxVal - val;
        }

        this.transform.localScale = Vector3.Lerp(this.transform.localScale,new Vector3(Mathf.Max(1,val), Mathf.Max(1, val),1),0.1f);
    }
}
