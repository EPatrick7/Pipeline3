using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMod : MonoBehaviour
{
    bool Applied;
    public Vector2 val = new Vector2(1,0);
    private void OnDestroy()
    {
        if (Applied)
            Physics2D.gravity -= val;
    }
    void Update()
    {
     if(!Applied)
        {
            Applied = true;
            Physics2D.gravity += val;
        }
    }
}
