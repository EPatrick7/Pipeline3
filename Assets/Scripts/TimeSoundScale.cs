using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSoundScale : MonoBehaviour
{
    public bool NoScale;
    public enum SoundType { Ambient,Music}
    public SoundType Type = SoundType.Ambient;
    float defaultP=1;
    AudioSource MySource;
    void Start()
    {
        MySource = this.GetComponent<AudioSource>();
        if (MySource != null)
        {
            defaultP = MySource.pitch;
            defaultV = MySource.volume;
            if (MySource != null)
            {
                if (Type == SoundType.Ambient)
                {
                    if (UIButton.AmbientMute)
                    {

                        MySource.enabled = false;
                        MySource.volume = 0;
                    }
                    else
                    {

                        MySource.enabled = true;
                        MySource.volume = defaultV;
                    }
                }
                else if (Type == SoundType.Music)
                {
                    if (UIButton.MusicMute)
                    {

                        MySource.enabled = false;
                        MySource.volume = 0;
                    }
                    else
                    {
                        MySource.enabled = true;
                        MySource.volume = defaultV;
                    }
                }
            }
            }
    }
    float defaultV;
    public bool DoDisable;
    void Update()
    {
        if (MySource != null)
        {
            if (Type == SoundType.Ambient)
            {
                if (UIButton.AmbientMute)
                {
                    if (DoDisable)
                        MySource.enabled = false;
                    MySource.volume = 0;
                }
                else
                {

                    if (DoDisable)
                        MySource.enabled = true;
                    MySource.volume = defaultV;
                }
            }
            else if (Type == SoundType.Music)
            {
                if (UIButton.MusicMute)
                {

                    MySource.enabled = false;
                    MySource.volume = 0;
                }
                else
                {
                    MySource.enabled = true;
                    MySource.volume = defaultV;
                }
            }
            if (!NoScale)
            {
                if (TimeMod.MaxTime <= 0.9)
                {

                    MySource.pitch = Vector2.Lerp(new Vector2(MySource.pitch, 0), new Vector2(Mathf.Max(Mathf.Min(1, TimeMod.MaxTime) + 0.2f, 0), 0.1f), 0.5f).x;
                }
                else
                    MySource.pitch = Vector2.Lerp(new Vector2(MySource.pitch, 0), new Vector2(defaultP, 0), 0.5f).x;
            }
        }

    }
}
