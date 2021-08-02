using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public bool IsOddNow;
    private void FixedUpdate()
    {
        for(int i =0;transform.childCount>0&&i<transform.childCount;i++)
        {
            if ((i%2==0)!=IsOddNow)
            {
                transform.GetChild(i).SendMessage("WaterUpdates",SendMessageOptions.DontRequireReceiver);
            }
        }
        IsOddNow = !IsOddNow;
    }
}