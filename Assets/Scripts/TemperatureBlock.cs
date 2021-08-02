using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureBlock : MonoBehaviour
{
    public int ID = -1;

    public bool AcidCracked;
    public float Temperature=20;
    public Gradient TempColor;
    public bool Touched;

    public GameObject Alternate;
    public Sprite Icon;
    private Color Origin;
    public Color Unset = Color.gray;
    private void Start()
    {
        if(Time.timeSinceLevelLoad<=0&&this.GetComponent<WaterSpawn>()==null&&this.GetComponent<WaterInlet>()==null)
        {
            if (this.GetComponent<SpriteRenderer>() != null && this.transform.Find("Frame") != null)
            {
                this.GetComponent<SpriteRenderer>().sortingOrder -= 2;
                this.transform.Find("Frame").GetComponent<SpriteRenderer>().sortingOrder -= 2;
            }
        }
        Origin =        this.GetComponent<SpriteRenderer>().color;

        if (this.transform.parent != null)
        {
            if (this.transform.parent.GetComponent<TemperatureBlock>() != null)
            {
                BombImmune = this.transform.parent.GetComponent<TemperatureBlock>().BombImmune;
            }
        }
        if(this.GetComponent<AudioSource>()!=null&&this.GetComponent<TimeSoundScale>()==null)
        {
            this.gameObject.AddComponent<TimeSoundScale>();
        }
    }
    int Number;
    private void OnMouseDown()
    {
        Number++;
    }
    private void OnMouseExit()
    {
        Number = 0;
    }
    public bool IsFixed = true;
    public bool BombImmune;
    
    private void LateUpdate()
    {
        if(this.GetComponent<TileImmune>()==null&&IsFixed&&Number>=2&&Alternate!=null)
        {

            if (this.transform.parent!=null)
            {
                if(this.transform.parent.GetComponent<AudioSource>()!=null)
                {
                    if (!this.transform.parent.GetComponent<AudioSource>().isPlaying&& this.transform.parent.GetComponent<AudioSource>().enabled)
                        this.transform.parent.GetComponent<AudioSource>().Play();
                }
            }
           GameObject g = Instantiate(Alternate, this.transform.position, this.transform.rotation, this.transform.parent);
            g.name = this.gameObject.name;
            g.GetComponent<TemperatureBlock>().Temperature = Temperature;
            Destroy(this.gameObject);
        }
        if (Temperature >= 250)
        {
            TileBuilder.addQueue(this.transform.position, Temperature);
        }

        if (Temperature <= -273)
            Temperature = -273;
        if (Water.ShowTemp)
        {
           if(!Touched)
            this.GetComponent<SpriteRenderer>().color = Color.Lerp(this.GetComponent<SpriteRenderer>().color, Unset,0.1f);
           else
            this.GetComponent<SpriteRenderer>().color = Color.Lerp(this.GetComponent<SpriteRenderer>().color, TempColor.Evaluate((Mathf.Max(-100, Mathf.Min(100, Temperature)) + 100) / 200f), 0.1f);
        }
        else
            this.GetComponent<SpriteRenderer>().color = Origin;
    }
}
