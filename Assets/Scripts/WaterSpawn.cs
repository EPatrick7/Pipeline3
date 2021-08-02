using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawn : MonoBehaviour
{
    public GameObject Water;
    [Min(1)]
    public int Speed=1;
    public int TargetNum;
    public bool ScaleWithFPS;
    public AudioClip Flow;
    private void OnDrawGizmosSelected()
    {
        Adjust();
    }
    private void Start()
    {
        Adjust();

    }
    public void Adjust()
    {
        if (this.transform.position.y  <= (Camera.main.transform.position.y))
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
    private void FixedUpdate()
    {
        if(ScaleWithFPS)
        {
            if (MouseCursor.FPS >= 70&& Camera.main.transform.Find("WaterManager").transform.childCount >= TargetNum)
                TargetNum++;
            else if(MouseCursor.FPS<=10)
            {
                TargetNum--;
                if (TargetNum < 200)
                    TargetNum = 200;
            }
        }
        if (MouseCursor.FPS >= 16)
        {
            if (Camera.main.transform.Find("WaterManager").transform.childCount < TargetNum && Water != null)
            {
                this.GetComponent<AudioSource>().clip = Flow;
                if (!this.GetComponent<AudioSource>().isPlaying&&this.GetComponent<AudioSource>().enabled)
                    this.GetComponent<AudioSource>().Play();
                for (int i = 0; i < Speed; i++)
                {
                    GameObject g = Instantiate(Water, this.transform.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, 0), Water.transform.rotation, Camera.main.transform.Find("WaterManager").transform);
                    g.GetComponent<Water>().Spawner = this.gameObject;
                    g.name = Water.name + "(" + Camera.main.transform.Find("WaterManager").transform.childCount + ")";
                }
            }
        }
        
    }
}
