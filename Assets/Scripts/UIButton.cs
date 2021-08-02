using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIButton : MonoBehaviour
{
    private Color Normal;
    public Color Changed;
    public int LevelReq = -1;
    public bool DisableIfLocked;
    public enum Type
    {
        Reset,Exit,Selection,Load,Story,MMusic,MAmbient,Tutorial, Disable
    };
    private void Start()
    {
        if (AllButtons == null)
            AllButtons = new List<UIButton>();
        AllButtons.Add(this);
        if (this.GetComponent<Image>()!=null)
        Normal = this.GetComponent<Image>().color;


         if (type == Type.Disable)
        {
            if(TileBuilder.WorldLocked&&this.GetComponent<Button>()!=null)
            this.GetComponent<Button>().Select();
        }

    }
    private void Update()
    {
        if (LevelReq > 0)
        {
            if (SaveManager.LEVELMAX < LevelReq)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    public int Mods;
    public static bool SceneLoading;
    public IEnumerator DelayedRun(int SceneID)
    {
        SceneLoading = true;
        if (BoxTransition.me != null)
        {
            BoxTransition.TransitionDone = false;
            BoxTransition.me.Shrink = false;
        }
        while (BoxTransition.me != null && !BoxTransition.TransitionDone)
        {
            yield return new WaitForEndOfFrame();
        }
        SceneLoading = false;
        if (type == Type.Reset)
        {
            TileBuilder.Tile = null;
            TileBuilder.SelectedIcon = null;
            GraphicSlider.BackgroundGraphic = 0;
            if (GraphicSlider.me != null)
                GraphicSlider.me.b = false;
        }
        else if (type == Type.Story)
        {
            TileBuilder.Tile = null;
            TileBuilder.SelectedIcon = null;
        }
        else if (type == Type.Load)
        {

            TileBuilder.Tile = null;
            TileBuilder.SelectedIcon = null;
        }
            SceneManager.LoadSceneAsync(SceneID, LoadSceneMode.Single);
    }
    public void OnClick()
    {
        if (type == Type.Reset)
        {
            if (!SceneLoading)
            {
                SceneLoading = true;
                StartCoroutine(DelayedRun(SceneManager.GetActiveScene().buildIndex));
            }
        }
        else if (type == Type.Selection)
        {
            TileBuilder.Tile = Selection;
        }
        else if (type == Type.Exit)
        {
            Application.Quit();
        }
        else if (type == Type.Load)
        {
            if (!SceneLoading)
            {
                SceneLoading = true;

                if (Mods >= 0)
                    StartCoroutine(DelayedRun(Mods));
                else
                    StartCoroutine(DelayedRun(SceneManager.GetActiveScene().buildIndex + 1));
            }
        }
        else if (type == Type.Story)
        {

            if (!SceneLoading)
            {
                SceneLoading = true;
                StartCoroutine(DelayedRun(LevelSlider.SelectedVal));
              //  SceneManager.LoadSceneAsync(LevelSlider.SelectedVal, LoadSceneMode.Single);
            }
        }
        else if (type == Type.MAmbient)
        {
            AmbientMute = !AmbientMute;
            if (AmbientMute)
                PlayerPrefs.SetInt("AMBIENTMUTE", 1);
            else
                PlayerPrefs.SetInt("AMBIENTMUTE", 0);
        }
        else if (type == Type.MMusic)
        {
            MusicMute = !MusicMute;
            if (MusicMute)
                PlayerPrefs.SetInt("MUSICMUTE", 1);
            else
                PlayerPrefs.SetInt("MUSICMUTE", 0);
        }
        else if (type == Type.Tutorial)
        {
            Selection.GetComponent<RectTransform>().localPosition = new Vector3(0, Mods, 0);
        }
        else if (type == Type.Disable)
        {
            TileBuilder.WorldLocked = !TileBuilder.WorldLocked;
            TileBuilder.WorldChanged = true;
            if (!TileBuilder.WorldLocked)
            {
                foreach (UIButton b in AllButtons)
                {
                    if (b != null)
                        b.gameObject.SetActive(true);
                }
            }
        }

    }

    public static List<UIButton> AllButtons;
    private Color Targ;
    private void FixedUpdate()
    {

        if (DisableIfLocked && TileBuilder.WorldLocked)
            this.gameObject.SetActive(false);
        if (type == Type.MAmbient)
        {

            if (AmbientMute)
                Targ = Changed;
            else
                Targ = Normal;

            this.GetComponent<Image>().color = Color.Lerp(this.GetComponent<Image>().color, Targ, 0.5f);
        }
        else if (type == Type.MMusic)
        {
            if (MusicMute)
                Targ = Changed;
            else
                Targ = Normal;


            this.GetComponent<Image>().color = Color.Lerp(this.GetComponent<Image>().color, Targ, 0.5f);
        }
        else if (type==Type.Disable)
        {
            if(TileBuilder.DisableLocked)
            {
                foreach (UIButton b in AllButtons)
                {
                    if (b != null)
                    {
                        if (b.type == Type.Disable)
                        {
                            if (b != this)
                            {
                                b.gameObject.SetActive(false);
                            }
                        }
                    }
                }
                this.gameObject.SetActive(false);
                TileBuilder.DisableLocked = false;
            }
        }
    }
    public static bool AmbientMute;
    public static bool MusicMute;
    public GameObject Selection;
    public Type type;
}
