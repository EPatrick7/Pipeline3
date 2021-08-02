using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInlet : MonoBehaviour
{
    private void Start()
    {
        Adjust();

    }
    public int Power=1;
    public void Adjust()
    {
        if (this.transform.position.y <= (Camera.main.transform.position.y))
        {
            this.transform.GetChild(0).transform.localPosition = new Vector3(this.transform.GetChild(0).transform.localPosition.x, -Mathf.Abs(this.transform.GetChild(0).transform.localPosition.y), this.transform.GetChild(0).transform.localPosition.z);
        }
        else
        {
            this.transform.GetChild(0).transform.localPosition = new Vector3(this.transform.GetChild(0).transform.localPosition.x, Mathf.Abs(this.transform.GetChild(0).transform.localPosition.y), this.transform.GetChild(0).transform.localPosition.z);

        }
    }
    private void Awake()
    {
        Adjust();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Water>()!=null)
        {
            if (!this.GetComponent<AudioSource>().isPlaying&& this.GetComponent<AudioSource>().enabled&&!UIButton.SceneLoading)
                this.GetComponent<AudioSource>().Play();
            SliderVal.lastcol = collision.gameObject.GetComponent<SpriteRenderer>().color;
            SliderVal.CurrentWater+= Power;
            Destroy(collision.gameObject);
        }
    }
}
