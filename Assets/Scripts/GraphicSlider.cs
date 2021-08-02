using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GraphicSlider : MonoBehaviour
{
    public static int BackgroundGraphic;
    private void Start()
    {
        me = this;
    }
    [HideInInspector]
   public bool b;
    public static GraphicSlider me;
    void Update()
    {
        if(!b)
        {
            b = true;
            this.GetComponent<Slider>().value = BackgroundGraphic; 
        }


        this.GetComponent<Slider>().value = Mathf.Max(0,Mathf.Min(this.GetComponent<Slider>().value, (SaveManager.LEVELMAX-2)/11));
        TileBuilder.Me.transform.parent.Find("Graphic").GetComponent<SpriteRenderer>().sprite = TileBuilder.Me.Backgrounds[(int)this.GetComponent<Slider>().value];
        if (BackgroundGraphic != (int)this.GetComponent<Slider>().value)
        {
            BackgroundGraphic = (int)this.GetComponent<Slider>().value;
            TileBuilder.WorldChanged = true;
        }
    }
}
