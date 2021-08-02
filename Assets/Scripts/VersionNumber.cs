using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VersionNumber : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().text =Application.version;       
    }
}
