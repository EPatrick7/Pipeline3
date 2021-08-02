using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTemperature : MonoBehaviour
{
    public float Temperature;
    public float Power = 1;
    public bool DestroyBlock;
    public GameObject SubBoom;
    public bool DestroyWater;
    private void Start()
    {
        if (DestroyBlock || DestroyWater)
            Temperature *= 10;
        else if (Power==100)
        {
            Temperature *= 3;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if(DestroyBlock&&other.GetComponent<TemperatureBlock>()!=null)
        {
            if (other.GetComponent<TemperatureBlock>().BombImmune == false && other.transform != this.transform.parent)
            {
                if (SubBoom != null)
                    Instantiate(SubBoom, other.transform.position, other.transform.rotation, other.transform.parent);


                other.gameObject.GetComponent<Collider2D>().enabled = false;
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                other.gameObject.GetComponent<TemperatureBlock>().BombImmune = true;
                other.gameObject.GetComponent<TemperatureBlock>().enabled = false;
                Destroy(other.gameObject,0.1f);
            }
        }
        if (other.GetComponent<Water>() != null)
        {
            if(DestroyWater)
            {
                if (SubBoom != null)
                    Instantiate(SubBoom, other.transform.position, other.transform.rotation, other.transform.parent);
                //other.transform.position -= new Vector3(100, 100,0);
              //  other.GetComponent<TrailRenderer>().Clear();

                other.GetComponent<Water>().ResetObject();
            }

            if(other.GetComponent<Water>().Temperature< Temperature)
            other.GetComponent<Water>().Temperature +=Power;
            if(other.GetComponent<Water>().Temperature> Temperature)
            other.GetComponent<Water>().Temperature -=Power;
        }
        else if (other.GetComponent<TemperatureBlock>() != null)
        {
            if (!other.GetComponent<TemperatureBlock>().Touched)
            {
                other.GetComponent<TemperatureBlock>().Touched = true;
                other.GetComponent<TemperatureBlock>().Temperature = Temperature;
            }
            if (other.GetComponent<TemperatureBlock>().Temperature < Temperature)
                other.GetComponent<TemperatureBlock>().Temperature += Power;
            if (other.GetComponent<TemperatureBlock>().Temperature > Temperature)
                other.GetComponent<TemperatureBlock>().Temperature -= Power;

        }
    }
}
