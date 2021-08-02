using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FinaleLevelManager : MonoBehaviour
{
    public Slider WaterSlider;
    public GameObject[] Modes;
    public GameObject WaterManager;
    public ParticleSystem Load;
    public AudioSource NextMode;
    int mode = 0;
    void Update()
    {
        
        if (SliderVal.CurrentWater>=WaterSlider.maxValue-2)
        {
            bool b =UpdateMode();
            if (b)
                SliderVal.CurrentWater = 0;
            else
                enabled = false;
        }

        WaterSlider.GetComponent<SliderVal>().LOADDISABLED = (mode < Modes.Length);
    }
    public bool UpdateMode()
    {
        Load.Play();
        Load.GetComponent<AudioSource>().Play();
        NextMode.Play();
        mode++;
        if (mode >= 0 && mode < Modes.Length)
        {
            if(mode-1>=0&&Modes[mode-1].transform.Find("Builds")!=null)
            {
                foreach(Transform c in Modes[mode - 1].transform.Find("Builds").transform)
                    {
                    Destroy(c.gameObject);
                }
            }

            foreach (GameObject g in Modes)
            {
                g.SetActive(false);
            }

            foreach (Transform c in WaterManager.transform)
            {
                if(c!=null&&c.GetComponent<Water>()!=null)
                { 
                c.GetComponent<Water>().Spawner = null;
                c.GetComponent<Water>().ResetObject();
                  }
            }
            Modes[mode].SetActive(true);
        }

        return (mode >= 0 && mode < Modes.Length);
    }
}
