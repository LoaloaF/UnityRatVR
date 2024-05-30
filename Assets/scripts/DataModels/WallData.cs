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

        public WallData(float version, string texture, float height)
        {
            this.uid = System.Guid.NewGuid().ToString();
            this.version = version;
            this.texture = texture;
            this.height = height;
        }
    }
}