using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public bool Visible=true;
     void OnBecameInvisible()
    {
        Visible = false;
    }
     void OnBecameVisible()
    {
        Visible = true;
    }
  //  int m_frameCounter = 0;
//float m_timeCounter = 0.0f;
   // float m_lastFramerate = 0.0f;
 //   public float m_refreshTime = 0.5f;
    public static float FPS;
    public float Frames;
    private void Start()
    {
        TimeMod.MaxTime = 1;
        Time.timeScale = 1;
        FPS = 30;
        IsButton.ButtonState = false;
        IsButton.SupportOn = 0;


        if (!b)
        {
            b = true;
            StartCoroutine(DelayedRun());
        }

    }
    bool b = false;
    IEnumerator DelayedRun()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        if (Frames <= 18)
        {
            Time.timeScale = Mathf.Max(Mathf.Min(1f,TimeMod.MaxTime) - 0.01f, 0.25f);
        }
        else if (Time.timeScale < 1)
            Time.timeScale = Mathf.Min(Mathf.Max(0.1f, TimeMod.MaxTime) + 0.01f, 1);
        b = false;


        if (!b)
        {
            b = true;
            StartCoroutine(DelayedRun());
        }
    }

    void Update()
    {
        
        Water.ShowTemp = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);


        //   FPS = Time.frameCount / Time.time;
        FPS =( FPS+(1f / Time.deltaTime))/2f;
        
        /*
        FPS = m_lastFramerate;
       // Debug.Log(FPS);
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
*/
        Frames = FPS;


        this.GetComponent<SpriteRenderer>().sprite = TileBuilder.SelectedIcon;

        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos = new Vector3(Mathf.RoundToInt(MousePos.x), Mathf.RoundToInt(MousePos.y), this.transform.position.z);


        this.transform.position = MousePos;
        if(!TileBuilder.Available)
        {

            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
            this.GetComponent<SpriteRenderer>().color = Color.red;

        Color c =  this.GetComponent<SpriteRenderer>().color;
        c.a = 0.6f;
        this.GetComponent<SpriteRenderer>().color = c;
        
    }
}
