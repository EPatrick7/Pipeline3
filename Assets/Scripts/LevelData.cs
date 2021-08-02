using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Json;

[System.Serializable]
public class LevelData
{
    public Level lev;
    [System.Serializable]
   public class BlockPlacement
    {
        public int x;
        public int y;

        public int ID;
    }
    [System.Serializable]
    public class Level
    {
        public bool isLocked;
        public string name;
        public string version;
        public int BackgroundID;
     
        public BlockPlacement[] Blocks;
        public override string ToString()
        {
            string BText = "";
            foreach(BlockPlacement b in Blocks)
            {
                BText += ",ID:" + b.ID + ",x:" + b.x + ",y:" + b.y + "";
            }
          return "{name:"+name+","+"version:"+version+"Blocks:{"+BText+"}}";
        }
    }
    public string toCode(Level level)
    {
        string json = JsonUtility.ToJson(level);

        string result = Base64Encode(json);
        return result;
    }
    public Level toLevel(string code)
    {
        Level level=null;
        try
        {
            string json = Base64Decode(code);
             level = JsonUtility.FromJson<Level>(json);
        }
        catch
        {
            CodeManager.ValueInput = "Invalid Code";
        }

    //    Debug.Log(level);
        return level;
    }
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
