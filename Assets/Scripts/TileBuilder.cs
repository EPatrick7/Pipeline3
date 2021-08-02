using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileBuilder : MonoBehaviour
{
    public Tile tile;
    public string JSONOUTPUT;
    public LevelData Data;
    public AudioClip LoadCustom;
    public GameObject[] Prefabs;
    public Sprite[] Backgrounds;
    public static bool WorldLocked;
    public static bool WorldChanged;
    public void UpdateJSON(string update)
    {
        JSONOUTPUT = update;
    }
    public void SaveWorld()
    {

        Data = ToData();

        JSONOUTPUT = Data.toCode(Data.lev);
    }
        

    public static bool ReloadingNewData;
    public void LoadWorld()
    {
        if (JSONOUTPUT != null&&Input.GetKey(KeyCode.Return))
        {
            LevelData.Level l = Data.toLevel(JSONOUTPUT);
            if (l != null)
            {
                ReloadingNewData = true;
                TEMPJSON = JSONOUTPUT;
                if(!UIButton.SceneLoading)
                {
                    UIButton.SceneLoading = true;
                    StartCoroutine(DelayedRun(SceneManager.GetActiveScene().buildIndex));
                }
                
            }
        }
    }
    public IEnumerator DelayedRun(int SceneID)
    {
        UIButton.SceneLoading = true;

        BoxTransition.me.Shrink = false;
        BoxTransition.TransitionDone = false;
        while (BoxTransition.me != null && !BoxTransition.TransitionDone)
        {
            yield return new WaitForEndOfFrame();
        }
        UIButton.SceneLoading = false;
        SceneManager.LoadSceneAsync(SceneID, LoadSceneMode.Single);
    }
    public static bool DisableLocked;
    public void FromData(LevelData.Level data)
    {
        int bg = data.BackgroundID;
        WorldLocked = data.isLocked;
        DisableLocked = WorldLocked;
        GraphicSlider.BackgroundGraphic = bg;
        this.transform.parent.Find("Graphic").gameObject.GetComponent<SpriteRenderer>().sprite = Backgrounds[bg];
        for(int i =0;i<this.transform.childCount;i++)
        {
            Destroy(this.transform.GetChild(i).gameObject, 0.1f);
        }
        int LastID = -1;
        int LastPos = -1;
        foreach(LevelData.BlockPlacement b in data.Blocks)
        {
            if (b.ID > 0)
            {
                Vector3 pos = new Vector3(b.x, b.y, 0);
                string name = b.x + "," + b.y;

                GameObject g = null;
                int ID = b.ID;
                if (ID == LastID)
                    g = Prefabs[LastPos];
                else
                {
                    for(int i=0;i<Prefabs.Length&&g==null;i++)
                    {
                        if(Prefabs[i].GetComponent<TemperatureBlock>().ID==ID)
                        {
                            LastPos = i;
                            LastID = ID;
                            g = Prefabs[i];
                        }
                    }
                }
                if (g != null)
                {
                    GameObject ob = Instantiate(g, pos, g.transform.rotation, this.transform);
                    ob.name = name;
                    ob.AddComponent<TileImmune>();
                    ob.GetComponent<TemperatureBlock>().BombImmune = true;
                }
            }
        }
    }

    public void Awake()
    {
        if(TileBuilder.Me!=null&&TileBuilder.Me.enabled==false)
        {
            TileBuilder.Me = this;
        }
    }

    public LevelData ToData()
    {
        LevelData data = new LevelData();
        
        List<LevelData.BlockPlacement> blocks = new List<LevelData.BlockPlacement>();

        foreach(Transform c in this.transform)
        {
            if(c.GetComponent<TemperatureBlock>()!=null)
            {
                if(c.GetComponent<TemperatureBlock>().ID>0)
                {
                    LevelData.BlockPlacement bl = new LevelData.BlockPlacement();
                    TemperatureBlock tb = c.GetComponent<TemperatureBlock>();
                    bl.ID = tb.ID;
                    bl.x = (int)tb.transform.position.x;
                    bl.y = (int)tb.transform.position.y;
                    blocks.Add(bl);
                }
            }
        }
        int match = 0;

        for(int i=0;i<Backgrounds.Length;i++)
        {
            if (this.transform.parent.Find("Graphic").GetComponent<SpriteRenderer>().sprite.Equals(Backgrounds[i]))
            {
                match = i;
                i = Backgrounds.Length;
            }
        }
        data.lev = new LevelData.Level();

        data.lev.isLocked = WorldLocked;
        data.lev.BackgroundID = match;
        data.lev.Blocks = blocks.ToArray();
        data.lev.name = SceneManager.GetActiveScene().name;
        data.lev.version = Application.version;
        return data;
    }

    public static GameObject Tile;
    public Vector3 MousePos;
    public GameObject DefaultSelect;
    public static Sprite SelectedIcon;
    public static TileBuilder Me;
    public AudioClip Place;
    public AudioClip Remove;
    public static string TEMPJSON;
    bool Completed = true;
    public Gradient Colors;
    public static void addQueue(Vector2 Pos, float Temperature)
    {

        if (Temperature>400)
        {
            if(TileBuilder.Me!=null)
            {
                    TileBuilder.Me.setTile(new Vector2Int(Mathf.RoundToInt(Pos.x), Mathf.RoundToInt(Pos.y)), Temperature);
                    // TileBuilder.Queue.Add(new DrawHeat(Pos, Temperature));
              
            }
        }
    }
    public void setTile(Vector2Int Coords,float Temp)
    {
        
        tile.color = Colors.Evaluate(Temp / 2000f);

        if (!TileBuilder.Me.ContainsTile(Coords))
            Tiles.Add((new Vector3Int(Coords.x, Coords.y, 0)));
        Checked.Remove((new Vector3Int(Coords.x, Coords.y, 0)));
        //   this.GetComponent<Tilemap>().SetTile(new Vector3Int(Coords.x, Coords.y, 0), null);
        if (TileBuilder.Me.ContainsTile(Coords))
        {
            tile.color = Color.Lerp(this.GetComponent<Tilemap>().GetColor(new Vector3Int(Coords.x, Coords.y, 0)), tile.color, 0.5f);
            this.GetComponent<Tilemap>().SetTile(new Vector3Int(Coords.x, Coords.y, 0), tile);
        }
        else
        this.GetComponent<Tilemap>().SetTile(new Vector3Int(Coords.x, Coords.y, 0), tile);
    }
    private List<Vector3Int> Checked;
    private List<Vector3Int> Excluded;
    public IEnumerator RemoveExlusion(Vector3Int v)
    {
        yield return new WaitForSeconds(0.25f);
        Excluded.Remove(v);
        Checked.Add(v);

    }
    public IEnumerator Render()
    {
        Completed = false;
        
       Tilemap map= this.GetComponent<Tilemap>();
             for(int i =0;i< Tiles.Count&&Tiles.Count>0;i++)
        {
            if (i < 0)
                i = 0;
            if(map.GetTile(Tiles[i])!=null)
            {
                Color c =map.GetColor(Tiles[i]);
                tile.color = c;
                if (Checked.Contains(Tiles[i]))
                    tile.color = Color.Lerp(c, new Color(c.r, c.g, c.b, 0), 0.25f);
                else
                {
                    if (!Excluded.Contains(Tiles[i]))
                    {
                        Excluded.Add(Tiles[i]);
                        StartCoroutine(RemoveExlusion(Tiles[i]));
                    }
                }

                map.SetTile(Tiles[i], null);
                if (tile.color.a <= 0.1f)
                {
                    Tiles.Remove(Tiles[i]);
                    i--;
                }
                else
                {
                    //map.SetTile(Tiles[i], null);
                    map.SetTile(Tiles[i], tile);
                }
//                map.RefreshTile(Tiles[i]);
            }
            if ((i + 1) % 1000 == 0)
                yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.1f);

        Completed = true;
    }
 //   public List<Vector3Int> Pos;
//    public List<TileBase> Base;
//    public Vector3Int[] PosLast;
//    public TileBase[] BaseLast;
    public bool ContainsTile(Vector2 Spot)
    {
        return this.GetComponent<Tilemap>().GetTile(new Vector3Int(Mathf.RoundToInt(Spot.x), Mathf.RoundToInt(Spot.y), 0))!=null;
    }
    private void Start()
    {
        Me = this;
        Completed = true;
        SelectedIcon = null;
        WorldChanged = true;
        Excluded = new List<Vector3Int>();
        Checked = new List<Vector3Int>();
        Tiles = new List<Vector3Int>();
     //   Queue = new List<DrawHeat>();
        Tile = DefaultSelect;
        if (ReloadingNewData)
        {
            this.transform.parent.Find("Load").GetComponent<AudioSource>().Stop();
            this.transform.parent.Find("Load").GetComponent<AudioSource>().clip = LoadCustom;
            this.transform.parent.Find("Load").GetComponent<AudioSource>().Play();
            ReloadingNewData = false;
            FromData(Data.toLevel(TEMPJSON));

            TEMPJSON = "";
        }

    }
    public List<Vector3Int> Tiles;
    public static bool Available;
    void Update()
    {

        if (TileBuilder.Me != null)
        {
            TileBuilder.Me = this;
        }
            if (Tiles.Count>0)
        {
            if(Completed)
            {
                Completed = false;
                StartCoroutine(Render());
            }
        }
        GameObject SelectedTile = null;
        if (Tile != null)
        {
            SelectedTile=Tile;


            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (Tile != null)
                {
                    if (Tile.GetComponent<TemperatureBlock>() != null)
                    {
                        if (Tile.GetComponent<TemperatureBlock>().Alternate != null)
                        {
                            SelectedTile = Tile.GetComponent<TemperatureBlock>().Alternate;
                        }
                    }
                }
            }
        }
        if(SelectedTile!=null&&SelectedTile.GetComponent<TemperatureBlock>()!=null)
        SelectedIcon = SelectedTile.GetComponent<TemperatureBlock>().Icon;
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos = new Vector3(Mathf.RoundToInt(MousePos.x), Mathf.RoundToInt(MousePos.y), this.transform.position.z);

        Available = (this.transform.Find((int)MousePos.x + "," + (int)MousePos.y) != null);


        if (CanvasOverLapper.ISOVER&& (Input.GetMouseButton(0)|| Input.GetMouseButton(1)))
        {
            if (this.transform.Find((int)MousePos.x + "," + (int)MousePos.y) != null)
            {
                GameObject g = this.transform.Find((int)MousePos.x + "," + (int)MousePos.y).gameObject;

                if (g != null && g.GetComponent<TileImmune>() == null)
                {
                    if (this.GetComponent<AudioSource>() != null)
                    {

                        if (!this.GetComponent<AudioSource>().isPlaying)
                        {
                            this.GetComponent<AudioSource>().pitch = 1;
                            this.GetComponent<AudioSource>().clip = Remove;
                            this.GetComponent<AudioSource>().Play();
                        }

                    }
                    WorldChanged = true;
                    Destroy(g.gameObject);
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0)&&SelectedTile!=null)
            {
                if (this.transform.Find((int)MousePos.x + "," + (int)MousePos.y) == null)
                {
                    if(this.GetComponent<AudioSource>()!=null)
                    {

                        if (!this.GetComponent<AudioSource>().isPlaying)
                        {
                            this.GetComponent<AudioSource>().pitch = 1;
                            this.GetComponent<AudioSource>().clip = Place;
                            if(this.GetComponent<AudioSource>().enabled)
                            this.GetComponent<AudioSource>().Play();
                        }

                    }
                    WorldChanged = true;
                    GameObject ob = Instantiate(SelectedTile, MousePos, SelectedTile.transform.rotation, this.transform);
                    ob.name = (int)MousePos.x + "," + (int)MousePos.y;

                }
            }
            if (Input.GetMouseButton(1))
            {
                if (this.transform.Find((int)MousePos.x + "," + (int)MousePos.y) != null)
                {
                    GameObject g = this.transform.Find((int)MousePos.x + "," + (int)MousePos.y).gameObject;

                    if (g != null&&g.GetComponent<TileImmune>()==null)
                    {
                        if (this.GetComponent<AudioSource>() != null)
                        {

                            if (!this.GetComponent<AudioSource>().isPlaying)
                            {
                                this.GetComponent<AudioSource>().pitch = 1;
                                this.GetComponent<AudioSource>().clip = Remove;
                                if (this.GetComponent<AudioSource>().enabled)
                                this.GetComponent<AudioSource>().Play();
                            }

                        }

                        WorldChanged = true;
                        Destroy(g.gameObject);
                    }
                }
            }
        }

    }
}
