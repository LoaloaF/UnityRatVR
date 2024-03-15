using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using Experiment;
using System;
using Unity.VisualScripting;
using System.Reflection;
using Cathei.BakingSheet;
using RatVR.ExcelData;

namespace RatVR.Scene
{
    /// <summary>
    /// This class manages the loading and saving of a session scene 
    /// </summary>
    public sealed class SceneController : MonoBehaviour
    {
        // Singleton pattern
        public static readonly SceneController Instance = new SceneController();
        private SceneController() { }

        public string scene_path = "";
        public GameObject floor;
        public GameObject wallTop, wallBottom, wallRight, wallLeft;
        public Material vStripes, hStripes, whiteDots, blackDots;

        Dictionary<string, Material> materialDict = new Dictionary<string, Material>();

        private void Start()
        {
            materialDict.Add("vstripes", vStripes);
            materialDict.Add("hstripes", hStripes);
            materialDict.Add("white dots", whiteDots);
            materialDict.Add("black dots", blackDots);

            if (scene_path.Contains(".xlsx"))
            {
                LoadExcelScene(scene_path);
            } 
            else if (scene_path.Contains(".json"))
            {
                LoadJSONScene(scene_path);
            }
        }

        private void LoadExcelScene(string path)
        {
            ExcelSceneFileHandler excelScene = new ExcelSceneFileHandler(path);

            ExcelSceneMetaData excelMeta = excelScene.GetExcelSceneMetaData();
            List<ExcelObjectData> excelObjects = excelScene.GetExcelSceneObjects();

            List<PillarData> pillars = PillarData.PillarDataFromExcel(excelObjects, excelScene.ScenePlacement());

            SceneGeometryData scene = new SceneGeometryData(1, excelMeta.baseLength, excelMeta.size, excelMeta.startLocation, excelMeta.agentLocation,
                                                            excelMeta.wallTop, excelMeta.wallBot, excelMeta.wallRight, excelMeta.wallLeft, pillars);

            LoadScene(scene);
        }

        private void LoadJSONScene(string path)
        {

            string scene = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
            JSONObject sceneJSON = (JSONObject)JSON.Parse(scene);

            LoadScene(new SceneGeometryData(sceneJSON));
        }

        private void LoadScene(SceneGeometryData sceneData)
        {
            floor.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength);
            floor.GetComponent<MeshRenderer>().material.mainTextureScale = 0.5f * sceneData.Size;

            // TODO: Change tiling of wall textures
            wallTop.transform.localPosition = new Vector3(0, -25, 0.5f * sceneData.BaseLength * sceneData.Size.y);
            wallBottom.transform.localPosition = new Vector3(0, -25, -0.5f * sceneData.BaseLength * sceneData.Size.y);
            wallLeft.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x, -25, 0);
            wallRight.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x, -25, 0);

            // _random = new System.Random();
            // pillars = new Dictionary<Tuple<int, int>, GameObject>();
            // checkpointPillars = new List<Tuple<int, int>>();

            foreach (PillarData pd in sceneData.Pillars)
            {
                GameObject pillar = Instantiate(Resources.Load<GameObject>("Pillar"), this.transform);

                pillar.name = "Pillar" + pd.UID.ToString();
                Vector2 tranformedPos = CoordinateTransform(sceneData, new Vector2(pd.Position.x, pd.Position.y));
                pillar.transform.localPosition = new Vector3(tranformedPos.x, pd.Position.z + -3.0f + pd.Height, tranformedPos.y);
                if (pd.Height != 0)
                {
                    pillar.transform.localScale = new Vector3(pd.Radius, pd.Height, pd.Radius);
                    pillar.GetComponentInChildren<MeshRenderer>().material = materialDict[pd.Texture];
                }
                else
                {
                    pillar.transform.localScale = new Vector3(pd.Radius, 1, pd.Radius);
                    pillar.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
                
                //pillar.GetComponentInChildren<MeshRenderer>().material = Materials[_random.Next(Materials.Count)];
                //pillar.GetComponent<Pillar>().Index = index;
            }
        }

        public static Vector2 CoordinateTransform(SceneGeometryData sceneData, Vector2 pos)
        {
            Vector2 translation = -0.5f*sceneData.BaseLength * sceneData.Size;
            //pos *= 0.5f*sceneData.BaseLength;
            return pos+translation;
        }
    }
}
