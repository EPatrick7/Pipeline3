using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOverLapper : MonoBehaviour
{
    public static bool ISOVER;
    private void OnMouseOver()
    {
        ISOVER = true;
    }
    private void OnMouseExit()
    {
        ISOVER = false;
    }
    private void Awake()
    {
        ISOVER = false;
    }
}
