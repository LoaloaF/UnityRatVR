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

        private int rewardZone = 0;
        public int RewardZone { get { return rewardZone; } set { rewardZone = value; } }
        

        public PillarData(JSONObject data)
        {
            if (data.HasKey("type") && data["type"] == "pillar")
            {
                uid = data.HasKey("uid") ? data["uid"].Value : throw new System.Exception("Scene file has no UID");
                version = data.HasKey("version") ? data["name"].AsFloat : throw new System.Exception("no version file specified");
                height = data.HasKey("height") ? data["height"].AsFloat : height = 10f;
                radius = data.HasKey("radius") ? data["radius"].AsFloat : radius = 1f;
                textures = data.HasKey("textures") ? data["textures"].AsStringList : null;
                if (data.HasKey("textures"))
                {
                    textures = data["textures"].AsStringList;
                    texture = textures[UnityEngine.Random.Range(0, textures.Count)];
                } 
                if (data.HasKey("texture"))
                {
                    texture = data["texture"];
                } 
                else if (texture == null)
                {
                    throw new System.Exception("No texture for pillar object provided");
                }

                float x = data.HasKey("x") ? data["x"].AsFloat : throw new System.Exception("No Pillar x position specified");
                float y = data.HasKey("y") ? data["y"].AsFloat : throw new System.Exception("No Pillar y position specified");
                float z = data.HasKey("z") ? data["z"].AsFloat : throw new System.Exception("No Pillar z position specified");

                position = new Vector3(x, y, z);
            }
            else
            {
                throw new System.Exception("Wrong PillarData format");
            }
        }

        public PillarData(float version, float height, float radius, Vector3 pos, string texture)
        {
            this.uid = System.Guid.NewGuid().ToString();
            this.version = version;
            this.height = height;
            this.radius = radius;
            this.position = pos;
            this.texture = texture;
        }

        public PillarData(ExcelObjectData excelObject, Vector2 pos)
        {
            this.uid = excelObject.object_name + "_" + System.Guid.NewGuid().ToString();
            version = 1;
            height = excelObject.height;
            radius = excelObject.radius;
            position = new Vector3(pos.x, pos.y, excelObject.zPos);
            texture = excelObject.texture;
            transparency = excelObject.transparency;
            isReward = excelObject.isReward;
            isAirpuff = excelObject.isAirpuff;
            rewardZone = excelObject.rewardZone;
        }

        public static List<PillarData> PillarDataFromExcel(List<ExcelObjectData> objects, Dictionary<string, List<Vector2>> scenePlacement)
        {
            List<PillarData> pillars = new List<PillarData>();
            foreach(ExcelObjectData excelObject in objects)
            {
                // Debug.Log("excel object: " + excelObject);
                foreach (Vector2 pos in scenePlacement[excelObject.object_name])
                {
                    // Debug.Log("pos: " + pos);
                    pillars.Add(new PillarData(excelObject, pos));
                    // Debug.Log(new PillarData(excelObject, pos).texture);
                }
            }
            return pillars;
        }

        public JSONNode PillarDataJson()
        {
            JSONNode data = new JSONObject();
            data.Add("uid", uid);
            data.Add("type", "pillar");
            data.Add("version", version);
            data.Add("height", height);
            data.Add("radius", radius);
            data.Add("texture", texture);
            data.Add("x", position.x);
            data.Add("y", position.y);
            data.Add("z", position.z);

            return data;
        }
    }
}