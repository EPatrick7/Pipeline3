using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CodeManager : MonoBehaviour
{
    public static string ValueInput;
    public void Update()
    {
        if(ValueInput!="")
        {

            this.GetComponent<TMP_InputField>().text = ValueInput;
            ValueInput = "";
        }
        if (TileBuilder.WorldChanged)
        {

            TileBuilder.WorldChanged = false;
            if (TileBuilder.Me.transform.childCount <= 0)
            {

                this.GetComponent<TMP_InputField>().text = "";
            }
            else
            {
                TileBuilder.Me.SaveWorld();
                this.GetComponent<TMP_InputField>().text = TileBuilder.Me.JSONOUTPUT;
            }
        }
    }
}
