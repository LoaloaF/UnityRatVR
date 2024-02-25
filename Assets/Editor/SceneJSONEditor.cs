using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.AccessControl;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

namespace RatVR.Scene
{
    public class SceneJSONEditor : EditorWindow
    {
        private int sizeX = 100;
        private int sizeY = 100;
        private int baseLength = 1;

        private Vector2 playerPos = Vector2.zero;
        private Vector2 agentPos = Vector2.one * -5f;

        private float wallHeight = 20f;
        private string textureLeft = "vstripes";
        private string textureRight = "hstripes";
        private string textureTop = "white dots";
        private string textureBottom = "black dots";

        private string texturePillars = "vstripes";

        private float radiusMin = 1, radiusMax = 2;
        private float heightMin = 2, heightMax = 3;
        private float zPosMin = 2, zPosMax = 5;

        private int distanceX = 10;
        private int distanceY = 10;

        private string path2Json = "C:\\Users\\RatVR\\VirtualReality\\VRmaze_v2\\Assets\\Resources\\scene2.json";

        #region UI 
        [MenuItem("Tools/JsonGenerator")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<SceneJSONEditor>(false, "JsonGenerator", true);
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("JSON Scene File Generator", EditorStyles.boldLabel);
            sizeX = EditorGUILayout.IntField("Size x:", sizeX);
            sizeY = EditorGUILayout.IntField("Size y:", sizeY);
            baseLength = EditorGUILayout.IntField("Base length:", baseLength);

            playerPos = EditorGUILayout.Vector2Field("Player position", playerPos);
            agentPos = EditorGUILayout.Vector2Field("Player position", agentPos);

            EditorLine();

            EditorGUILayout.LabelField("Walls", EditorStyles.boldLabel);
            wallHeight = EditorGUILayout.FloatField("Height:", wallHeight);
            textureLeft = EditorGUILayout.TextField("Texture left", textureLeft);
            textureTop = EditorGUILayout.TextField("Texture top", textureTop);
            textureRight = EditorGUILayout.TextField("Texture right", textureRight);
            textureBottom = EditorGUILayout.TextField("Texture bottom", textureBottom);

            EditorLine();

            EditorGUILayout.LabelField("Grid Creator", EditorStyles.boldLabel);


            MinMaxField(ref radiusMin, ref radiusMax, "Radius of Pillars");
            MinMaxField(ref heightMin, ref heightMax, "Height of Pillars");
            MinMaxField(ref zPosMin, ref zPosMax, "zPos of Pillars");
            texturePillars = EditorGUILayout.TextField("Texture right", texturePillars);

            distanceX = EditorGUILayout.IntField("Distance between Pillars in x:", distanceX);
            distanceY = EditorGUILayout.IntField("Distance between Pillars in y:", distanceY);

            GUILayout.FlexibleSpace();

            path2Json = EditorGUILayout.TextField(path2Json);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Generate"))
            {
                GenerateJsonSceneFile();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void EditorLine()
        {
            EditorGUILayout.Space(); // Optional: Add some space before the divider
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            EditorGUILayout.Space(); // Optional: Add some space after the divider
        }

        private void MinMaxField(ref float min, ref float max, string label)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);

            var minMax = MinMaxChecker(min, max);
            min = minMax.Item1;
            max = minMax.Item2;
            min = EditorGUILayout.FloatField("min", min);
            max = EditorGUILayout.FloatField("max", max);
            GUILayout.EndHorizontal();
        }
        private Tuple<float, float> MinMaxChecker(float min, float max, bool negValuesAllowed = false)
        {
            if (min > max)
            {
                max = min;
            }
            if (!negValuesAllowed)
            {
                if (min < 0) min = 0;
                if (max < 0) max = 0;
            }

            return new Tuple<float, float>(min, max);
        }

        #endregion

        #region SceneGenerator
        private void GenerateJsonSceneFile()
        {
            WallData topWall = new WallData(1, textureTop, wallHeight);
            WallData rightWall = new WallData(1, textureRight, wallHeight);
            WallData bottomWall = new WallData(1, textureBottom, wallHeight);
            WallData leftWall = new WallData(1, textureLeft, wallHeight);

            SceneGeometryData sceneData = new SceneGeometryData(1, baseLength, new Vector2(sizeX,sizeY), playerPos, agentPos, topWall, bottomWall, rightWall, leftWall, GeneratePillarData());
            Debug.Log(sceneData.CreateJSONString());
            System.IO.File.WriteAllText(path2Json, sceneData.CreateJSONString());
        }

        private List<PillarData> GeneratePillarData()
        {
            List<PillarData> pillars = new List<PillarData>();

            for (int x = distanceX; x < sizeX; x += distanceX)
            {
                for (int y = distanceY; y < sizeY; y += distanceY)
                {
                    float z = UnityEngine.Random.Range(zPosMin, zPosMax);
                    float height = UnityEngine.Random.Range(heightMin, heightMax);
                    float radius = UnityEngine.Random.Range(radiusMin, radiusMax);
                    Vector3 pos = new Vector3(x, y, z);
                    PillarData pd = new PillarData(1, height, radius, pos, texturePillars);
                    pillars.Add(pd);
                }
            }
            return pillars;
        }

        #endregion
    }
}