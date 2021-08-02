using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
public class SaveManager : MonoBehaviour
{
    public static int LEVELMAX;
    public static int LEVELSELECT;
    public static bool BOOTED = false;

    void Start()
    {
        TileBuilder.WorldLocked = false;
        if (!BOOTED)
        {
            BOOTED = true;
        }
        LEVELMAX = PlayerPrefs.GetInt("LEVELMAX", 2);
        LEVELSELECT = PlayerPrefs.GetInt("LEVELSELECT", LEVELMAX);
        UIButton.AmbientMute = PlayerPrefs.GetInt("AMBIENTMUTE", 0) == 1;
        UIButton.MusicMute = PlayerPrefs.GetInt("MUSICMUTE", 0) == 1;


        Debug.Log("Prefs Loaded: " + LEVELSELECT + "/" + LEVELMAX+" | AM: " + UIButton.AmbientMute+ ", MM:" + UIButton.MusicMute );
    }

    private void LateUpdate()
    {
        PlayerPrefs.SetInt("LEVELMAX", SaveManager.LEVELMAX);
        PlayerPrefs.SetInt("LEVELSELECT", SaveManager.LEVELSELECT);
    }

}
