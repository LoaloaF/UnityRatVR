using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatVR.ExcelData
{
    public class ExcelSceneMetaData
    {
        public Vector2 size;
        public float baseLength;
        public int deathzone;
        public Vector2 startLocation;
        public Vector2 agentLocation;
        public float lengthFlash;
        public float lengthSound;

        public ExcelWallData wallTop;
        public ExcelWallData wallRight;
        public ExcelWallData wallBot;
        public ExcelWallData wallLeft;

        public bool cylinder;
        public int rewardDelay;
        public int rewardLength;
        public string DeathZoneAction;

        // public ExcelSceneMetaData(Vector2 size, float baseLength, int deathzone, Vector2 startLocation, Vector2 agentLocation, int lengthFlash, int lengthSound, ExcelWallData wallTop, ExcelWallData wallRight, ExcelWallData wallBot, ExcelWallData wallLeft, bool cylinder)
        public ExcelSceneMetaData(Vector2 size, float baseLength, int deathzone, Vector2 agentLocation, ExcelWallData wallTop, ExcelWallData wallRight, ExcelWallData wallBot, ExcelWallData wallLeft, 
                                        int rewardDelay, int rewardLength, string DeathZoneAction){
            this.size = size;
            this.baseLength = baseLength;
            this.deathzone = deathzone;
            
            // this.startLocation = startLocation;
            this.agentLocation = agentLocation;
            // this.lengthFlash = lengthFlash;
            // this.lengthSound = lengthSound;
            this.wallTop = wallTop;
            this.wallRight = wallRight;
            this.wallBot = wallBot;
            this.wallLeft = wallLeft;
            // this.cylinder = cylinder;
            this.rewardDelay = rewardDelay;
            this.rewardLength = rewardLength;
            this.DeathZoneAction = DeathZoneAction;
        }
    }

    public class ExcelWallData
    {
        public float height;
        public string texture;

        public ExcelWallData(float height, string texture)
        {
            this.height = height;
            this.texture = texture;
        }

        public static implicit operator WallData(ExcelWallData excelWall)
        {
            return new WallData(1, excelWall.texture, excelWall.height);
        }
    }

    public class ExcelObjectData
    {
        public string object_name;
        public float radius;
        public float height;
        public float zPos;
        public string texture;
        public float transparency;
        public int isReward;
        public int isAirpuff;
        public int rewardZone;

        public ExcelObjectData(string object_name, float radius, float height, float zPos, string texture, float transparency, int isReward, int isAirpuff, int rewardZone)
        {
            this.object_name = object_name;
            this.radius = radius;
            this.height = height;
            this.zPos = zPos;
            this.texture = texture;
            this.transparency = transparency;
            this.isReward = isReward;
            this.isAirpuff = isAirpuff;
            this.rewardZone = rewardZone;
        }

        public ExcelObjectData(List<string> values)
        {
            this.object_name = values[0];
            this.radius = float.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture);
            this.height = float.Parse(values[2], System.Globalization.CultureInfo.InvariantCulture);
            this.zPos = float.Parse(values[3], System.Globalization.CultureInfo.InvariantCulture);
            this.texture = values[4];
            this.transparency = float.Parse(values[5], System.Globalization.CultureInfo.InvariantCulture);
            this.isReward = int.Parse(values[6]);
            this.isAirpuff = int.Parse(values[7]);
            this.rewardZone = int.Parse(values[8]);
        }

        public override string ToString()
        {
            return object_name + "r: " + radius.ToString() + ", h: " + height.ToString() + ", zPos: " + zPos.ToString();
        }
    }
    

    
}