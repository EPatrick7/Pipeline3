using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMouseOver : MonoBehaviour
{
    public GameObject Next;
    public int MouseButtonNeeded=-1;
    public void ButonClick()
    {
        if (MouseButtonNeeded == -1 || Input.GetMouseButton(MouseButtonNeeded))
        {
            if (Next != null)
            {
                Next.gameObject.SetActive(true);
                if (Next.GetComponent<SpriteRenderer>() != null)
                    Next.GetComponent<SpriteRenderer>().enabled = true;
            }
            Destroy(this.gameObject);
        }
    }
    private void OnMouseOver()
    {
        ButonClick();

    }
}
