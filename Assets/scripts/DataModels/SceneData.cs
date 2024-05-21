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







        private int wallZone;
        public int WallZone { get { return wallZone; } set { wallZone = value; } }


        #endregion

        public SceneGeometryData(float version, 
                                float baseLength, 
                                Vector2 size, 
                                WallData topWall, 
                                WallData bottomWall, 
                                WallData rightWall, 
                                WallData leftWall, 
                                List<PillarData> pillars, 
                                int wallZone)
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
            this.wallZone = wallZone;
        }
    }

}