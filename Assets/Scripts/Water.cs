using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{


    public float Temperature;
    public Gradient GradColor;


    Rigidbody2D rig;
    Collider2D col;
    SpriteRenderer ren;
    public bool IsWater;
    TrailRenderer trail;
    SpriteMask mask;
    [Range(1,100)]
    public float Conductivity=25;
    public bool IsAcid;
    public int SurfaceTension=1;
    public float Bouancy = 0.1f;
    public bool CanConduct;
    public float Electricity;
    public Gradient ElectricityColor;
    

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(state==State.Liquid&&CanConduct)
        {
            if(collision.collider.GetComponent<Electrical>()!=null)
            {
                if (Electricity > 0)
                    collision.collider.GetComponent<Electrical>().Interacted = true;

                Electricity += collision.collider.GetComponent<Electrical>().ChargeStrength;
                Electricity = Mathf.Min(Mathf.Max(0, Electricity),100);
                XOffset = collision.collider.transform.position.x;

                if (!this.transform.parent.GetComponent<AudioSource>().isPlaying&&Electricity>0)
                {
                    this.transform.parent.GetComponent<AudioSource>().clip = null;
             
                        this.transform.parent.GetComponent<AudioSource>().clip = Zap;
   

                    if (this.transform.parent.GetComponent<AudioSource>().enabled)
                        this.transform.parent.GetComponent<AudioSource>().Play();
                }
            }
        }
        if(collision.collider.tag=="Float"&&state==State.Liquid)
        {
            float bt = 2;
            collision.otherRigidbody.velocity += new Vector2(0, bt);
            if(transform.position.y>collision.transform.position.y)
            collision.transform.position = Vector2.Lerp(collision.transform.position, transform.position, 0.1f);
            rig.velocity -= new Vector2(0, bt);
        }
        else if (collision.collider.GetComponent<Water>()!=null)
        {
            UpdatesSinceCollision = 0;


            Water other = collision.collider.GetComponent<Water>();

            if(other.state==State.Liquid&&state==State.Liquid&&MouseCursor.FPS>=20)
            {
                if(IsAcid&&!other.IsAcid&&other.IsWater)
                {
                    other.Spawner = null;
                    Spawner = null; 
                    other.IsAcid = true;
                    other.GradColor = GradColor;
                    other.BoilingPoint = BoilingPoint;
                    other.CanConduct = CanConduct;
                    other.ElectricityColor = ElectricityColor;
                    other.FreezingPoint = FreezingPoint;
                    //  other.GetComponent<SpriteRenderer>().color = ren.color;
                    Instantiate(AcidConvert, (this.transform.position+other.transform.position)/2f, this.transform.rotation, this.transform.parent);

                }
                else if (other.IsAcid&&!IsAcid&&other.IsWater)
                {
                    Spawner = null;

                    other.Spawner = null;
                    IsAcid = true;
                    GradColor = other.GradColor;
                    ElectricityColor = other.ElectricityColor;
                    CanConduct = other.CanConduct;
                    BoilingPoint = other.BoilingPoint;
                    Instantiate(AcidConvert, (this.transform.position + other.transform.position) / 2f, this.transform.rotation, this.transform.parent);
                    FreezingPoint = other.FreezingPoint;
                 //   ren.color = other.ren.color;
                }
            }

          //  Debug.Log(other == this);
            if (other.Temperature != Temperature)
            {
                if (other.state == State.Liquid || state == State.Liquid)
                {
                    if (other.state == State.Liquid && state != State.Liquid)
                    {
                        other.rig.velocity -= new Vector2(0, other.Bouancy);
                        rig.velocity += new Vector2(0, Bouancy);
                    }
                    else if (other.state != State.Liquid && state == State.Liquid)
                    {
                        other.rig.velocity += new Vector2(0, other.Bouancy);
                        rig.velocity -= new Vector2(0, Bouancy);

                    }
                    else
                    {
                        if (other.Temperature > this.Temperature)
                        {
                            other.rig.velocity += new Vector2(0, other.Bouancy);
                            rig.velocity -= new Vector2(0, Bouancy);
                        }
                        else
                        {

                            other.rig.velocity -= new Vector2(0, other.Bouancy);
                            rig.velocity += new Vector2(0, Bouancy);
                        }
                    }
                }
                


                float Split = ((Temperature - other.Temperature)) * (Conductivity / 100f);



                Split *= 10;
                Split = (int)Split;

                Split /= 10f;
               // Debug.Log(Temperature + " vs " + other.Temperature);

                Temperature -= Split;
                other.Temperature += Split;
            }
            //            ren.color = Color.red;

            if (SurfaceTension != 0)
            {
                if (other.state == State.Liquid && state == State.Liquid)
                {
                    //      ren.color = Changed;

                    if(other.CanConduct)
                    {
                        float dif = (Electricity + other.Electricity) / 2f;
                        float dif2 = (XOffset + other.XOffset) / 2f;
                        XOffset = dif2;
                        other.XOffset = dif2;
                        other.Electricity = dif;
                        Electricity = dif;
                            other.Electricity = Mathf.Min(Mathf.Max(0, other.Electricity),100);
                Electricity = Mathf.Min(Mathf.Max(0, Electricity),100);
                    }

                    Vector2 Difference = collision.collider.transform.position - this.transform.position;

                    Difference *= SurfaceTension;



                    collision.rigidbody.velocity -= Difference;
                    rig.velocity += Difference;
                }
            }
        }
        else
        {
            if(collision.collider.name== Mathf.RoundToInt(transform.position.x)+","+ Mathf.RoundToInt(transform.position.y) )
            {
                if (collision.collider.tag != "NoKill" && Vector2.Distance(collision.collider.transform.position, this.transform.position) <= 0.8f)
                {
                    if (Spawner!=null&&Vector2.Distance(this.transform.position,Spawner.transform.position)>=4)
                        ResetObject();
                    else
                        Destroy(this.gameObject);
                }
            }
            else if (collision.collider.GetComponent<TemperatureBlock>()!=null)
            {
                TemperatureBlock temp = collision.collider.GetComponent<TemperatureBlock>();
                if(temp.AcidCracked&&IsAcid&&state==State.Liquid)
                {
                    GameObject g =Instantiate(AcidConvert,temp.transform.position,temp.transform.rotation,temp.transform.parent);
                    g.name = temp.name;
                    Instantiate(AcidConvert,this.transform.position,temp.transform.rotation,this.transform.parent);
                    Destroy(temp.gameObject);
                }
                if (temp.Touched)
                {

                    float Split = ((Temperature - temp.Temperature)) * (Conductivity / 100f);



                    Split *= 10;
                    Split = (int)Split;

                    Split /= 10f;
                    // Debug.Log(Temperature + " vs " + other.Temperature);

                    Temperature -= Split;
                    temp.Temperature += Split;
                }
                else
                {
                    temp.Touched = true;
                    temp.Temperature = Temperature;
                }
            }

        }
        if (collision.gameObject.tag == "Sponge"&&state==State.Liquid)
        {
            if (collision.gameObject.GetComponent<SpongeGrow>() != null)
            {
                if (collision.gameObject.GetComponent<SpongeGrow>().WaterValue <=(collision.gameObject.GetComponent<SpongeGrow>().MaxVal*10f)+5)
                {
                    collision.gameObject.GetComponent<SpongeGrow>().WaterValue++;
                    Destroy(this.gameObject);
                }
            }
        }
    }
    private float TempStart;

    public enum State
    {
        Solid,
        Liquid,
        Gas
    }
    float XOffset=-50;
    public State state = State.Liquid;

    [Min(-273)]
    public float FreezingPoint = 0;
    public float BoilingPoint=100;

    private void Awake()
    {

        this.GetComponent<CircleCollider2D>().radius = 0.25f;
        TempStart =Temperature;
        rig = this.GetComponent<Rigidbody2D>();
        rig.simulated = true;
        col = this.GetComponent<Collider2D>();
        ren = this.GetComponent<SpriteRenderer>();
        mask = this.GetComponent<SpriteMask>();
        trail=this.GetComponent<TrailRenderer>();
    }
    public float JiggleScale=1;
    public float JiggleDamp=10;
    public Color Target;
    bool Visible;
    private void OnBecameVisible()
    {
        Visible = true;
    }
    private void OnBecameInvisible()
    {
        Visible = false;
    }
    private Vector2 LastPos;
    int UpdatesSinceCollision;
    public static bool ShowTemp;
    private State old;
    public Gradient TempColor;
    private float Lerp(float x,float y,float t)
    {
        float final=x;

        if(x<y)
        {
            x += t;

            x = Mathf.Max(x, y);
        }
        else
        {
            x -= t;
            x = Mathf.Min(x, y);
        }

        return final;
    }
    [HideInInspector]
    public GameObject Spawner;
    public AudioClip Freeze;
    public AudioClip Liqiud;
    public AudioClip Steam;
    public AudioClip Zap;
   // public void FixedUpdate()
    //{
      //  WaterUpdates();
   // }
    public void WaterUpdates()
    {
        if(Resets)
        {

            this.GetComponent<Rigidbody2D>().simulated = true;
            Resets = false;
            this.GetComponent<TrailRenderer>().Clear();

            this.GetComponent<TrailRenderer>().enabled = true;
        }

        if (LastPos == Vector2.zero)
            LastPos = this.transform.position;

        if(Mathf.Abs(rig.velocity.magnitude)>4)
        foreach (Transform ob in TileBuilder.Me.transform)
        {
                if (new Vector2(Mathf.RoundToInt(ob.transform.position.x), Mathf.RoundToInt(ob.position.y)) == new Vector2(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y)))
                {
                    if (ob.tag != "NoKill" && Vector2.Distance(ob.transform.position, this.transform.position) <= 0.8f)
                    {
                        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        this.transform.position = LastPos;
                     //   Destroy(this.gameObject);
                    }
                }
        }
        LastPos = this.transform.position;

        float amount = 8f;
        if (this.GetComponent<Rigidbody2D>().velocity.x > amount)
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(amount, this.GetComponent<Rigidbody2D>().velocity.y);

        if (this.GetComponent<Rigidbody2D>().velocity.x < -amount)
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(-amount, this.GetComponent<Rigidbody2D>().velocity.y);

        if (this.GetComponent<Rigidbody2D>().velocity.y < -amount)
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, -amount);
        if (this.GetComponent<Rigidbody2D>().velocity.y > amount)
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, amount);


        if (Temperature >= 250&&state==State.Liquid)
        {
            TileBuilder.addQueue(this.transform.position, Temperature);
        }

        if (Temperature <= -273)
            Temperature = -273;

        if(state==State.Gas)
        this.gameObject.layer = 12;
        else if (state==State.Solid)
            this.gameObject.layer = 14;
        else
            this.gameObject.layer = 4;

        State temp = state;
        if (Temperature <= FreezingPoint)
            state = State.Solid;
        else if (Temperature > FreezingPoint && Temperature >= BoilingPoint)
            state = State.Gas;
        else
           state = State.Liquid;
        if(state!=temp&&this.transform.parent!=null)
        {
            if (!this.transform.parent.GetComponent<AudioSource>().isPlaying)
            {
                this.transform.parent.GetComponent<AudioSource>().clip = null;
                if (state == State.Solid)
                {
                    this.transform.parent.GetComponent<AudioSource>().clip = Freeze;
                }
                else if (state == State.Liquid)
                {
                    this.transform.parent.GetComponent<AudioSource>().clip = Liqiud;
                }
                else if (state == State.Gas)
                {
                    this.transform.parent.GetComponent<AudioSource>().clip = Steam;
                }

                if (this.transform.parent.GetComponent<AudioSource>().enabled)
                    this.transform.parent.GetComponent<AudioSource>().Play();
            }
            else if (this.transform.parent.parent.Find("Graphic")!=null)
            {
                Transform tran = this.transform.parent.parent.Find("Graphic");

                tran.GetComponent<AudioSource>().clip = null;
                if (state == State.Solid)
                {
                    tran.GetComponent<AudioSource>().clip = Freeze;
                }
                else if (state == State.Liquid)
                {
                    tran.GetComponent<AudioSource>().clip = Liqiud;
                }
                else if (state == State.Gas)
                {
                    tran.GetComponent<AudioSource>().clip = Steam;
                }
                if (tran.GetComponent<AudioSource>().enabled)
                tran.GetComponent<AudioSource>().Play();
            }
        }

        if (ShowTemp)
        {
            Target = TempColor.Evaluate((Mathf.Max(-100, Mathf.Min(100, Temperature-FreezingPoint)) + 100) / 200f);
        }
        else
        Target = GradColor.Evaluate((Mathf.Max(-100, Mathf.Min(100, Temperature-FreezingPoint)) + 100) / 200f);
  
        if(state==State.Solid)
        {
            ren.rendererPriority = 0;
            rig.velocity /= new Vector2(1000, 1);
            rig.gravityScale = 1;
            rig.mass = 10;
            this.GetComponent<CircleCollider2D>().radius = Lerp(this.GetComponent<CircleCollider2D>().radius, 0.1f,0.1f);

            this.GetComponent<CircleCollider2D>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (state==State.Liquid)
        {
            
            ren.rendererPriority = 1;
            rig.gravityScale = 1;
            rig.mass = 1;
            this.GetComponent<CircleCollider2D>().radius = Lerp(this.GetComponent<CircleCollider2D>().radius, 0.25f, 0.1f);
            this.GetComponent<CircleCollider2D>().enabled = true;
            this.GetComponent<BoxCollider2D>().enabled = false;

        }
        else if (state==State.Gas)
        {
            ren.rendererPriority = 2;
            rig.gravityScale = 0;
            rig.mass = 0.1f;
        
            this.GetComponent<CircleCollider2D>().radius = Lerp(this.GetComponent<CircleCollider2D>().radius, 0.1f,0.1f);
            this.GetComponent<CircleCollider2D>().enabled = true;
            this.GetComponent<BoxCollider2D>().enabled = false;

        }
        if(old!= state)
        {
            if (state == State.Gas)
            {
                rig.velocity += new Vector2(0, 2);
            }
        }
        old = state;
              



        if (state == State.Gas)
        {
            Target = Color.Lerp(Target, Camera.main.backgroundColor,0.4f);
            Target.a = 0.25f;
            mask.enabled = false;
        }
        else
        {
            mask.enabled = true;
        }

        Color c = ren.color;
        c.a = Target.a;
        ren.color = c;

        ren.color = Color.Lerp(ren.color, Target, 0.1f);

        if (state == State.Liquid && Electricity > 0)
        {
            Electricity = Mathf.Min(Random.Range(Electricity - 5, Electricity+5),Electricity);
            Electricity = Mathf.Max(Mathf.Min(Electricity, 100), 0);
            
                ren.color = Color.Lerp(ren.color, ElectricityColor.Evaluate(((Mathf.Abs(this.transform.position.x - XOffset) + (Time.time * 10)) % 10) / 10f), 0.5f);
        }
        else
            Electricity = 0;
      
        trail.startColor = ren.color;
        trail.endColor = ren.color;


        if (state == State.Gas)
        {
                rig.velocity += (this.transform.up / new Vector2(2, 2)) * (Random.Range(-JiggleScale, JiggleScale) / JiggleDamp);

        }
        if (Mathf.Abs(rig.velocity.y) <= 0.1&&state!=State.Solid)
        {
            float mod = 1;
            mod += Mathf.Min(5,(Mathf.Max(FreezingPoint,Temperature)-(10f+FreezingPoint))/10f);

            rig.velocity += (this.transform.up/ new Vector2(1,5))*(Random.Range(-JiggleScale, JiggleScale) / JiggleDamp);


        }

        if (Vector2.Distance(this.transform.position, Camera.main.transform.position) > Camera.main.orthographicSize + 10)
        {
            if (!Visible)
            {

                if (MouseCursor.FPS >= 16)
                    ResetObject();
                else
                    Destroy(this.gameObject);
            }
        }
        if (UpdatesSinceCollision > 100&&state!=State.Gas&&Physics2D.gravity!=Vector2.zero)
        {
            ResetObject();
        }
        UpdatesSinceCollision++;
    }
    public GameObject AcidConvert;
    private float TimesReset=0;
    bool Resets;
    public void ResetObject()
    {
        Electricity = 0;
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.GetComponent<Rigidbody2D>().angularVelocity = 0;
        this.GetComponent<Rigidbody2D>().simulated = false;
        if (Spawner != null&&TimesReset<=5)
        {

            this.GetComponent<CircleCollider2D>().radius = 0.25f;
            this.GetComponent<TrailRenderer>().enabled = false;
            this.GetComponent<TrailRenderer>().Clear();
            


            this.transform.position = Spawner.transform.position;
            Temperature = TempStart;
            Resets = true;
            TimesReset++;
            this.GetComponent<SpriteRenderer>().color = GradColor.Evaluate((Mathf.Max(-100, Mathf.Min(100, Temperature - FreezingPoint)) + 100) / 200f);
        }
        else
            Destroy(this.gameObject);
    }
}
