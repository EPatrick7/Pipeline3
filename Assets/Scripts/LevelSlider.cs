using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSlider : MonoBehaviour
{
    public static int SelectedVal;
    private void Start()
    {

        SaveManager.LEVELMAX = PlayerPrefs.GetInt("LEVELMAX", 2);
        SaveManager.LEVELSELECT = PlayerPrefs.GetInt("LEVELSELECT", SaveManager.LEVELMAX);
        this.GetComponent<Slider>().value = SaveManager.LEVELSELECT-1;
    }
    void Update()
    {
        SelectedVal = ((int)this.GetComponent<Slider>().value+1);
        if (this.GetComponent<Slider>().value>SaveManager.LEVELMAX-1)
        {
            this.GetComponent<Slider>().value = SaveManager.LEVELMAX-1;
        }
    }
}
