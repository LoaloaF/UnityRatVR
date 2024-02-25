using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatVR.Scene
{
    public class WallData
    {
        private string uid;
        public string UID { get { return uid; } set { uid = value; } }

        private float version = 1.0f;
        public float Version { get { return version; } set { version = value; } }

        private float height = 10f;
        public float Height { get { return height; } set { height = value; } }

        private List<string> textures;
        public List<string> Textures { get { return textures; } set { textures = value; } }

        private string texture;
        public string Texture { get { return texture; } set { texture = value; } }

        public WallData(JSONObject data)
        {
            if (data.HasKey("type") && data["type"] == "wall")
            {
                uid = data.HasKey("uid") ? data["uid"].Value : throw new System.Exception("Scene file has no UID");
                version = data.HasKey("version") ? data["name"].AsFloat : throw new System.Exception("no version file specified");
                height = data.HasKey("height") ? data["height"].AsFloat : height = 10f;
                if (data.HasKey("texture"))
                {
                    texture = data["texture"];
                }
                else if (texture == null)
                {
                    throw new System.Exception("No texture for pillar object provided");
                }
            }
            else
            {
                throw new System.Exception("Wrong WallData format");
            }
        }

        public WallData(float version, string texture, float height)
        {
            this.uid = System.Guid.NewGuid().ToString();
            this.version = version;
            this.texture = texture;
            this.height = height;
        }

        public JSONNode WallDataJson()
        {
            JSONNode data = new JSONObject();
            data.Add("uid", uid);
            data.Add("type", "wall");
            data.Add("version", version);
            data.Add("height", height);
            data.Add("texture", texture);

            return data;
        }
    }
}