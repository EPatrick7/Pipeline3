using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelGrabber : MonoBehaviour
{
    void Update()
    {
        this.GetComponent<TextMeshProUGUI>().text = "Level " + (SceneManager.GetActiveScene().buildIndex-1);
    }
}
