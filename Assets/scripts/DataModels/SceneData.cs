using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace RatVR.Scene
{
    public class SceneGeometryData
    {
        #region Attributes
        private string uid;
        public string UID { get { return uid; } set { uid = value; } }

        private float version = 1.0f;
        public float Version { get { return version; } set { version = value; } }

        private float baseLength = 1.0f;
        public float BaseLength { get { return baseLength; } set { baseLength = value; } }

        private Vector2 size;
        public Vector2 Size { get { return size; } set { size = value; } }

        private Vector2 playerPosition;
        public Vector2 PlayerPosition { get { return playerPosition; } set { playerPosition = value; } }

        private Vector2 agentPosition;
        public Vector2 AgentPosition { get { return agentPosition; } set { agentPosition = value; } }

        private JSONArray rewardLocationPillars;
        public JSONArray RewardLocationPillars { get { return rewardLocationPillars; } set { rewardLocationPillars = value; } }

        private JSONArray rewardLocationGrid;
        public JSONArray RewardLocationGrid { get { return rewardLocationGrid; } set { rewardLocationGrid = value; } }

        private WallData topWall;
        public WallData TopWall { get { return topWall; } set { topWall = value; } }
        
        private WallData bottomWall;
        public WallData BottomWall { get { return bottomWall; } set { bottomWall = value; } }

        private WallData leftWall;
        public WallData LeftWall { get { return leftWall; } set { leftWall = value; } }
        
        private WallData rightWall;
        public WallData RightWall { get { return rightWall; } set { rightWall = value; } }

        private List<PillarData> pillars;
        public List<PillarData> Pillars { get { return pillars; } set { pillars = value; } }

        #endregion

        public SceneGeometryData(JSONObject data)
        {
            if (data.HasKey("type") && data["type"] == "geometry")
            {
                uid = data.HasKey("uid") ? data["uid"].Value : throw new System.Exception("Scene file has no UID");
                version = data.HasKey("version") ? data["name"].AsFloat : throw new System.Exception("no version file specified");
                baseLength = data.HasKey("base_length") ? data["base_length"].AsFloat : baseLength = 1f ;

                if (data.HasKey("player"))
                {
                    size.x = data["player"]["x"].AsFloat;
                    size.y = data["player"]["y"].AsFloat;        
                }
                if (data.HasKey("agent"))
                {
                    size.x = data["agent"]["x"].AsFloat;
                    size.y = data["agent"]["y"].AsFloat;
                }
                if (data.HasKey("size"))
                {
                    size.x = data["size"]["x"].AsFloat;
                    size.y = data["size"]["y"].AsFloat;
                }

                rewardLocationPillars = data.HasKey("reward") && data["reward"].HasKey("pillars") ? data["reward"]["pillars"].AsArray : null ;
                rewardLocationGrid = data.HasKey("reward") && data["reward"].HasKey("grid") ? data["reward"]["grid"].AsArray : null;

                if (rewardLocationGrid == null && rewardLocationPillars == null)
                {
                    Debug.Log("No reward locations provided");
                    //throw new System.Exception("No reward locations provided");
                }

                pillars = new List<PillarData>();
                if (data.HasKey("pillars"))
                {
                    foreach (var child in data["pillars"].AsArray)
                    {
                        pillars.Add(new PillarData(child.Value.AsObject));
                    }
                }

                if (data.HasKey("walls"))
                {
                    topWall = data["walls"].HasKey("top") ? new WallData(data["walls"]["top"].AsObject) : throw new System.Exception("No top wall provided");
                    bottomWall = data["walls"].HasKey("bottom") ? new WallData(data["walls"]["bottom"].AsObject) : throw new System.Exception("No bottom wall provided");
                    leftWall = data["walls"].HasKey("left") ? new WallData(data["walls"]["left"].AsObject) : throw new System.Exception("No left wall provided");
                    rightWall = data["walls"].HasKey("right") ? new WallData(data["walls"]["right"].AsObject) : throw new System.Exception("No right wall provided");
                }
            }
            else
            {
                throw new System.Exception("Wrong SceneGeometryData format");
            }
        }

        public SceneGeometryData(float version, float baseLength, Vector2 size, Vector2 playerPos, Vector2 agentPos,WallData topWall, WallData bottomWall, WallData rightWall, WallData leftWall, List<PillarData> pillars)
        {
            this.uid = System.Guid.NewGuid().ToString();
            this.version = version;
            this.baseLength = baseLength;
            this.topWall = topWall;
            this.bottomWall = bottomWall;
            this.rightWall = rightWall;
            this.leftWall = leftWall;
            this.pillars = pillars;
            this.size = size;
            this.playerPosition = playerPos;
            this.agentPosition = agentPos;
        }

        public string CreateJSONString()
        {
            JSONObject data = new JSONObject();
            data.Add("uid", uid);
            data.Add("type", "geometry");
            data.Add("version", version);
            data.Add("base_length", baseLength);

            data.Add("size", Vector2JSON(size));
            data.Add("player", Vector2JSON(playerPosition));
            data.Add("agent", Vector2JSON(agentPosition));

            JSONObject walls = new JSONObject();
            walls.Add("top", topWall.WallDataJson());
            walls.Add("right", rightWall.WallDataJson());
            walls.Add("bottom", bottomWall.WallDataJson());
            walls.Add("left", leftWall.WallDataJson());

            data.Add("walls", walls);
            //data.Add("reward", {'grid': [{'x':7, 'y': 5, 'sound': true}]});

            JSONArray pillars = new JSONArray();
            foreach(PillarData pd in this.pillars)
            {
                pillars.Add(pd.PillarDataJson());
            }
            data.Add("pillars", pillars);

            return data.ToString();
        }

        private JSONObject Vector2JSON(Vector2 vector)
        {
            JSONObject data = new JSONObject();
            data.Add("x", vector.x);
            data.Add("y", vector.y);

            return data;
        }
    }
}