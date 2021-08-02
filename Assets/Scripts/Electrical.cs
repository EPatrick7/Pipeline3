using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrical : MonoBehaviour
{
    public float ChargeStrength;
    public bool Interacted;
    public ParticleSystem[] Systems;
    public bool IsButtonType;
    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);

        Interacted = false;
        b = false;
    }
    bool b;
    private void FixedUpdate()
    {
        if(IsButtonType)
        {
            foreach(ParticleSystem p in Systems)
            {
                p.enableEmission=Interacted;
            }
        }
        if(Interacted)
        {
            if (!b)
            {
                b = true;
                StartCoroutine(Delay());
            }
            if (this.GetComponent<IsButton>() != null)
            {
                this.GetComponent<IsButton>().IsTouched = true;
                this.GetComponent<IsButton>().g = false;
                this.GetComponent<IsButton>().StopAllCoroutines();
            }
        }
    }
}
