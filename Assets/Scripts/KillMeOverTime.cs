using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMeOverTime : MonoBehaviour
{
    public float Time;
    public GameObject Explosion;

    public IEnumerator KillDelay()
    {
        yield return new WaitForSeconds(Time);
        Instantiate(Explosion, this.transform.position, this.transform.rotation, this.transform.parent);
        Destroy(this.gameObject);
    }
    void Start()
    {
        if (Explosion == null)
            Destroy(this.gameObject, Time);
        else
            StartCoroutine(KillDelay());
    }
    
}
