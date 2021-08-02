using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource MySource;
    public AudioClip[] Clips;

    public static int id=-1;
    public static float Time;
    public bool stop;
    private void Start()
    {

        if(id==-1)
        {
            id= Random.Range(0, Clips.Length);
        }

        if (id >= Clips.Length)
            id = 0;
        MySource.Stop();
            MySource.clip = Clips[id];
            MySource.time = Time;
        if(MySource.enabled)
            MySource.Play();
        
    }
    public void FixedUpdate()
    {

        if(stop)
        {
            MySource.Stop();
            stop = false;
        }

        Time = MySource.time;
        if (!MySource.isPlaying)
        {
            int ran = Random.Range(0, Clips.Length);


            if (ran == id)
                ran = Random.Range(0, Clips.Length);

            if (ran == id)
                ran = Random.Range(0, Clips.Length);

            id = ran;

            if (id >= Clips.Length)
                id = 0;

            MySource.clip = Clips[id];
            MySource.time = 0;
            if(MySource.enabled)
            MySource.Play();
        }
    }
}
