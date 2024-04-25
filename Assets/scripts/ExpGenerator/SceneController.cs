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
using System.Diagnostics;
using System.Drawing.Text;
// using UnityEditor.Experimental.GraphView;
// using UnityEditor.PackageManager;


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

        public GameObject floor, ceiling;

        public GameObject wallTop, wallBottom, wallRight, wallLeft;
        // public Material vStripes, hStripes, whiteDots, blackDots;
        private Material  transparentMaterial;
        public Dictionary<string, Material> materials = new Dictionary<string, Material>();
        private Color color;    
        public SceneGeometryData scene;
        public string DeathZoneAction;
        public  GameObject deathzone;
        public  float successSequenceLength = 3.3f;
        public  float maximumTrialLength = 30.4f;

        public void LoadExcelScene(string path)
        {
            loadMaterials("Assets/Resources/materials");

            ExcelSceneFileHandler excelScene = new ExcelSceneFileHandler(path);

            ExcelSceneMetaData excelMeta = excelScene.GetExcelSceneMetaData();
            List<ExcelObjectData> excelObjects = excelScene.GetExcelSceneObjects();

            /*
            foreach (var key in excelScene.ScenePlacement().Keys)
            {
                UnityEngine.Debug.Log("key: " + key);
                UnityEngine.Debug.Log("values:" + excelScene.ScenePlacement()[key]);
            }
            UnityEngine.Debug.Log("key count: " + excelScene.ScenePlacement().Keys.Count);
            */

            List < PillarData> pillars = PillarData.PillarDataFromExcel(excelObjects, excelScene.ScenePlacement());
            // pillars.

            scene = new SceneGeometryData(1, excelMeta.baseLength, excelMeta.size, excelMeta.startLocation, excelMeta.agentLocation,
                                                            excelMeta.wallTop, excelMeta.wallBot, excelMeta.wallRight, excelMeta.wallLeft, pillars, excelMeta.deathzone, excelMeta.rewardDelay, excelMeta.rewardLength);
            DeathZoneAction = excelMeta.DeathZoneAction;
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
            // deathzone size and position
            deathzone.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength);
            deathzone.transform.position = new Vector3(0, -0.1f, 0);

            
            // floor.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength);
            // floor.GetComponent<MeshRenderer>().material.mainTextureScale = 0.5f * sceneData.Size;
            // floor size with death zone
            floor.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength - 0.2f*scene.DeathZone, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength - 0.2f*scene.DeathZone);
            floor.GetComponent<MeshRenderer>().material.mainTextureScale = 0.1f * (sceneData.Size - new Vector2(scene.DeathZone, scene.DeathZone)*2);

            ceiling.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength);
            ceiling.transform.position = new Vector3(0, 0.2f * sceneData.BaseLength * sceneData.Size.y, 0);

            // TODO: Change tiling of wall textures
            //wallTop.transform.localPosition = new Vector3(0, -25, 0.5f * sceneData.BaseLength * sceneData.Size.y);

            /*
            wallTop.transform.localPosition = new Vector3(0, 0, 0.5f * sceneData.BaseLength * sceneData.Size.y);
            wallBottom.transform.localPosition = new Vector3(0, -25, -0.5f * sceneData.BaseLength * sceneData.Size.y);
            wallLeft.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x, -25, 0);
            wallRight.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x, -25, 0);
            */


            // due to different wall rotation setup, the same position could have different effects
            wallTop.transform.rotation = Quaternion.Euler(90, 0, 90);
            wallBottom.transform.rotation = Quaternion.Euler(90, 0, -90);
            wallRight.transform.rotation = Quaternion.Euler(90, 90, 90);
            wallLeft.transform.rotation = Quaternion.Euler(90, 90, -90);

            // wall size, wall size should change with the arena size accrodingly
            wallTop.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.y, 1, 0.2f * 0.1f * sceneData.BaseLength * sceneData.Size.y);
            wallBottom.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.y, 1, 0.2f * 0.1f * sceneData.BaseLength * sceneData.Size.y);
            wallRight.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.x, 1, 0.2f * 0.1f * sceneData.BaseLength * sceneData.Size.x);
            wallLeft.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.x, 1, 0.2f * 0.1f * sceneData.BaseLength * sceneData.Size.x);

           

            // wall position
            wallTop.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x, 0.1f  * sceneData.BaseLength * sceneData.Size.y, 0);
            wallBottom.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x, 0.1f * sceneData.BaseLength * sceneData.Size.y, 0);
            wallRight.transform.localPosition = new Vector3(0, 0.1f * sceneData.BaseLength * sceneData.Size.x, -0.5f * sceneData.BaseLength * sceneData.Size.y);
            wallLeft.transform.localPosition = new Vector3(0, 0.1f * sceneData.BaseLength * sceneData.Size.x, 0.5f * sceneData.BaseLength * sceneData.Size.y);
            
             // use wall height from excel sheet
             /*
            wallTop.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x, sceneData.TopWall.Height, 0);
            wallBottom.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x, sceneData.BottomWall.Height, 0);
            wallRight.transform.localPosition = new Vector3(0, sceneData.RightWall.Height, -0.5f * sceneData.BaseLength * sceneData.Size.y);
            wallLeft.transform.localPosition = new Vector3(0, sceneData.LeftWall.Height, 0.5f * sceneData.BaseLength * sceneData.Size.y);
            // change ceiling height
            ceiling.transform.position = new Vector3(0, sceneData.TopWall.Height, 0);
            */

            // wall textures
            wallTop.GetComponent<Renderer>().material = materials[sceneData.TopWall.Texture];
            wallBottom.GetComponent<Renderer>().material = materials[sceneData.BottomWall.Texture];
            wallRight.GetComponent<Renderer>().material = materials[sceneData.RightWall.Texture];
            wallLeft.GetComponent<Renderer>().material = materials[sceneData.LeftWall.Texture];


            // _random = new System.Random();
            // pillars = new Dictionary<Tuple<int, int>, GameObject>();
            // checkpointPillars = new List<Tuple<int, int>>();

            foreach (PillarData pd in sceneData.Pillars)
            {
                GameObject pillar = Instantiate(Resources.Load<GameObject>("Pillar"), this.transform); // the pillar object should be stored in the folder "Resources" 
                

                // pillar.GetComponentInChildren<Transform>().sc = new Vector3(pd.RewardZone, 2, pd.RewardZone);

                pillar.name = "Pillar" + pd.UID.ToString();
                Vector2 tranformedPos = CoordinateTransform(sceneData, new Vector2(pd.Position.x, pd.Position.y));
                pillar.transform.localPosition = new Vector3(tranformedPos.x, pd.Position.z + pd.Height, tranformedPos.y);
                if (pd.Height != 0)
                {
                    Transform CylinderTransform = pillar.transform.Find("Cylinder");
                    CylinderTransform.localScale = new Vector3(pd.Radius, pd.Height, pd.Radius);
                    Transform ColliderTransform = pillar.transform.Find("Collider");
                    ColliderTransform.localScale = new Vector3(pd.RewardZone, 0.1f*sceneData.Size.x, pd.RewardZone);
                    // pillar.GetComponentInChildren<MeshRenderer>().material = materialDict[pd.Texture];
                    
                    if (!materials.ContainsKey(pd.Texture)) {
                        throw new Exception("Material not found: " + pd.Texture);
                    }

                    // pillar texture using materials from resources folder
                    // setting the transparency
                    transparentMaterial = materials[pd.Texture];
                    color = materials[pd.Texture].color;

                    // pillar texture using 4 textures
                    // transparentMaterial = materialDict[pd.Texture];
                    // color = materialDict[pd.Texture].color;
                    color.a = pd.Transparency;
                    transparentMaterial.color = color;

                    pillar.GetComponentInChildren<MeshRenderer>().material = transparentMaterial;
                    CylinderTransform.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(pd.Height, pd.Height);

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

        private void loadMaterials(string material_path)
        {
            // load all the materials from material path
            string[] PathsArray = Directory.GetFiles(material_path, "*.mat");

            foreach (string path in PathsArray)
            {
                string name = path.Substring(0, path.IndexOf("."));
                name = name.Substring(material_path.Length+1);
                // UnityEngine.Debug.Log("name: " + name);

                Material loadedMaterial = Resources.Load<Material>("materials/" + name);
                if (loadedMaterial != null)
                {
                    materials[name] = loadedMaterial;
                }

            }        
            UnityEngine.Debug.Log("materials count: " + materials.Count);

        }
    }
}
