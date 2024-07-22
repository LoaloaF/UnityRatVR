using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using RatVR.ExcelData;
using System;
// using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;

namespace RatVR.Scene
{
    public class PillarData
    {
        private string uid;
        public string UID { get { return uid; } set { uid = value; } }

        private float version = 1.0f;
        public float Version { get { return version; } set { version = value; } }

        private float height = 10f;
        public float Height { get { return height; } set { height = value; } }

        private float radius = 1f;
        public float Radius { get { return radius; } set { radius = value; } }

        private Vector3 position;
        public Vector3 Position { get { return position; } set { position = value; } }

        private List<string> textures;
        public List<string> Textures { get { return textures; } set { textures = value; } }

        private string texture;
        public string Texture { get { return texture; } set { texture = value; } }

        private float transparency = 1f;
        public float Transparency { get { return transparency; } set { transparency = value;  } }

        private int isReward = 0;
        public int IsReward { get { return isReward; } set { isReward = value; } }

        private int isAirpuff = 0;
        public int IsAirpuff { get { return isAirpuff; } set { isAirpuff = value; } }

        private float rewardRadius = 0;
        public float RewardRadius { get { return rewardRadius; } set { rewardRadius = value; } }
        
        private int showGround = 0;
        public int ShowGround { get { return showGround; } set { showGround = value; } }

        private int isMoving = 0;
        public int IsMoving { get { return isMoving; } set { isMoving = value; } }

        public PillarData(ExcelObjectData excelObject, Vector2 pos)
        {
            this.uid = excelObject.object_name + "_" + System.Guid.NewGuid().ToString();
            version = 1;
            height = excelObject.height;
            radius = excelObject.radius;
            position = new Vector3(pos.x, pos.y, excelObject.zPos);
            texture = excelObject.texture;
            rewardRadius = excelObject.rewardRadius;
            showGround = excelObject.pillarShowGround;
            isMoving = excelObject.pillarIsMoving;
            transparency = excelObject.transparency;
            isReward = excelObject.isReward;
            isAirpuff = excelObject.isAirpuff;
        }

        public static List<PillarData> PillarDataFromExcel(List<ExcelObjectData> objects, Dictionary<string, List<Vector2>> scenePlacement)
        {
            List<PillarData> pillars = new List<PillarData>();
            foreach(ExcelObjectData excelObject in objects)
            {
                foreach (Vector2 pos in scenePlacement[excelObject.object_name])
                    pillars.Add(new PillarData(excelObject, pos));
            }
            return pillars;
        }

    }
}