using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
public class SliderVal : MonoBehaviour
{

    public bool LoadNext;
    public int NextLevelID;
    public float TargetWater=300;
    public static float CurrentWater=0;
    public bool IsDOODLE;
    public bool LOADDISABLED;
    private void Awake()
    {
        SliderSource = this.GetComponent<AudioSource>();
        CurrentWater = 0;
        if (!IsDOODLE)
        {
            if( SceneManager.GetActiveScene().buildIndex>SaveManager.LEVELMAX)
            PlayerPrefs.SetInt("LEVELMAX", SceneManager.GetActiveScene().buildIndex);
            if (SceneManager.GetActiveScene().buildIndex != SaveManager.LEVELSELECT)
                PlayerPrefs.SetInt("LEVELSELECT", SceneManager.GetActiveScene().buildIndex);
            if (LoadNext)
            {
                NextLevelID = SceneManager.GetActiveScene().buildIndex + 1;
            }
        }


        
        if (Analytics.enabled)
        {
            Debug.Log("Message Sent");
            Analytics.SendEvent("level_complete", PlayerPrefs.GetInt("LEVELMAX", 2));
            //Analytics.CustomEvent("level_complete", ""+PlayerPrefs.GetInt("LEVELMAX", 2));
        }
        else
        {

            Debug.Log("Message Could Not Be Sent!");
        }
    }

    public Image BG;
    public static Color lastcol=Color.blue;
    float i;
    public static AudioSource SliderSource;
    public IEnumerator DelayedRun(int SceneID)
    {
        UIButton.SceneLoading = true;
        if (BoxTransition.me != null)
        {
            BoxTransition.me.Shrink = false;
            BoxTransition.TransitionDone = false;
        }
        if(this.GetComponent<AudioSource>()!=null)
        {
            this.GetComponent<AudioSource>().Play();
        }
        while (BoxTransition.me != null && !BoxTransition.TransitionDone)
        {
            yield return new WaitForEndOfFrame();
        }
        UIButton.SceneLoading = false;
        SceneManager.LoadSceneAsync(SceneID, LoadSceneMode.Single);
    }
    void Update()
    {
        BG.color = Color.Lerp(BG.color, lastcol, 0.1f);
        i+=0.1f;
        if (i >= 500)
            i = 1;
        CurrentWater = Mathf.Min(Mathf.Max(0, CurrentWater),TargetWater);
        this.GetComponent<Slider>().maxValue = TargetWater;
        this.GetComponent<Slider>().value = CurrentWater;

        if(CurrentWater>=TargetWater&&!IsDOODLE&&!LOADDISABLED)
        {
            CurrentWater = 0;
            this.enabled = false;
            if(!UIButton.SceneLoading)
            {
                UIButton.SceneLoading = true;
                StartCoroutine(DelayedRun(NextLevelID));
            }
        }
    }
}
