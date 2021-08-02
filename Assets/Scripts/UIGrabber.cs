using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
public class UIGrabber : MonoBehaviour
{
    public Slider slider;
    public string Prefix;
    public enum Type {Slider,LevelSlider}
    public Type type;
    public string[] list;
    void FixedUpdate()
    {
        if(type==Type.Slider||type==Type.LevelSlider)
        {
            this.GetComponent<TextMeshProUGUI>().text = Prefix + slider.value;
        }
        if(type==Type.LevelSlider)
        {
            this.GetComponent<TextMeshProUGUI>().text += ": "+list[Mathf.Min(Mathf.Max((int)slider.value-1,0),(int)slider.maxValue-1)];
        }
    }
}
