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

        public GameObject floor, ceiling, wallZone;

        public GameObject wallTop, wallBottom, wallRight, wallLeft;
        public GameObject meshTop, meshBottom, meshRight, meshLeft;
        private Material  transparentMaterial;
        public Dictionary<string, Material> materials = new Dictionary<string, Material>();
        private Color color;    
        public SessionManager _sessionManager;
        public ExcelSessionMetaData sessionMetaData;
        public SceneGeometryData scene;
        // public  GameObject wallZone;
        
        public  GameObject Lighting;


        private void Awake() {
            _sessionManager = GetComponent<SessionManager>();
        }
        public void LoadExcelScene(string path)
        {
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            // this adjusts the path when exec from build subfolder 
            if (Directory.Exists(Path.Combine(projectPath, "Assets")) == false) {
                projectPath = System.IO.Directory.GetParent(projectPath).FullName;
            }
            loadMaterials(Path.Combine(projectPath, "Assets/Resources/materials"));

            ExcelSceneFileHandler excelScene = new ExcelSceneFileHandler(path);

            // get sceneMetaData
            ExcelSceneMetaData sceneMetaData = excelScene.GetExcelSceneMetaData();
            List<ExcelObjectData> excelObjects = excelScene.GetExcelSceneObjects();


            // get sessionMetatData
            sessionMetaData = excelScene.GetExcelSessionMetaData();
            _sessionManager.InitializeSessionManager(sessionMetaData);

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

            scene = new SceneGeometryData(1, 
                                          sceneMetaData.baseLength, 
                                          sceneMetaData.size,
                                          sceneMetaData.wallTop, 
                                          sceneMetaData.wallBot, 
                                          sceneMetaData.wallRight, 
                                          sceneMetaData.wallLeft, 
                                          pillars, 
                                          sceneMetaData.wallZone);
            
            LoadScene(scene);
        }

        private void LoadScene(SceneGeometryData sceneData)
        {
            // wallzone size and position
            wallZone.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength);
            wallZone.transform.position = new Vector3(0, 0, 0);

            
            // floor.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength);
            // floor.GetComponent<MeshRenderer>().material.mainTextureScale = 0.5f * sceneData.Size;
            // floor size with death zone
            floor.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength - 0.2f*scene.WallZone, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength - 0.2f*scene.WallZone);
            floor.GetComponent<MeshRenderer>().material.mainTextureScale = 0.1f * (sceneData.Size - new Vector2(scene.WallZone, scene.WallZone)*2);
            floor.transform.position = new Vector3(0, 0.01f, 0);
            
            ceiling.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength);
            // ceiling.transform.position = new Vector3(0, 0.2f * sceneData.BaseLength * sceneData.Size.y, 0);

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

            meshTop.transform.rotation = Quaternion.Euler(90, 0, 90);
            meshBottom.transform.rotation = Quaternion.Euler(90, 0, -90);
            meshRight.transform.rotation = Quaternion.Euler(90, 90, 90);
            meshLeft.transform.rotation = Quaternion.Euler(90, 90, -90);

            // wall size, wall size should change with the arena size accrodingly
            // wallTop.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.y, 1, 0.2f * 0.1f * sceneData.BaseLength * sceneData.Size.y);
            // wallBottom.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.y, 1, 0.2f * 0.1f * sceneData.BaseLength * sceneData.Size.y);
            // wallRight.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.x, 1, 0.2f * 0.1f * sceneData.BaseLength * sceneData.Size.x);
            // wallLeft.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.x, 1, 0.2f * 0.1f * sceneData.BaseLength * sceneData.Size.x);

            // wall height depends on the wall height from excel sheet, which should be fixed in different size of arenas 60cm maybe
            // also remember the ceiling height
            wallTop.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.y, 1, 0.1f * sceneData.TopWall.Height);          
            wallBottom.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.y, 1, 0.1f * sceneData.BottomWall.Height);
            wallRight.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.x, 1, 0.1f * sceneData.RightWall.Height);
            wallLeft.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.x, 1, 0.1f * sceneData.LeftWall.Height);
            
            wallTop.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x, 0.5f* sceneData.TopWall.Height, 0);
            wallBottom.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x, 0.5f*sceneData.BottomWall.Height, 0);
            wallRight.transform.localPosition = new Vector3(0, 0.5f*sceneData.RightWall.Height, -0.5f * sceneData.BaseLength * sceneData.Size.y);
            wallLeft.transform.localPosition = new Vector3(0, 0.5f*sceneData.LeftWall.Height, 0.5f * sceneData.BaseLength * sceneData.Size.y);

            ceiling.transform.position = new Vector3(0, sceneData.TopWall.Height, 0);

            meshTop.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x -5f, 0.5f* sceneData.TopWall.Height, 0);
            meshBottom.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x +5f, 0.5f*sceneData.BottomWall.Height, 0);
            meshRight.transform.localPosition = new Vector3(0, 0.5f*sceneData.RightWall.Height, -0.5f * sceneData.BaseLength * sceneData.Size.y +5f);
            meshLeft.transform.localPosition = new Vector3(0, 0.5f*sceneData.LeftWall.Height, 0.5f * sceneData.BaseLength * sceneData.Size.y -5f);
           

            // wall position
            // wallTop.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x, 0.1f  * sceneData.BaseLength * sceneData.Size.y, 0);
            // wallBottom.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x, 0.1f * sceneData.BaseLength * sceneData.Size.y, 0);
            // wallRight.transform.localPosition = new Vector3(0, 0.1f * sceneData.BaseLength * sceneData.Size.x, -0.5f * sceneData.BaseLength * sceneData.Size.y);
            // wallLeft.transform.localPosition = new Vector3(0, 0.1f * sceneData.BaseLength * sceneData.Size.x, 0.5f * sceneData.BaseLength * sceneData.Size.y);
            
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
                // UnityEngine.Debug.Log("Postision and height" + pd.Height);
                if (pd.Height != 0)
                {
                    Transform CylinderTransform = pillar.transform.Find("Cylinder");
                    CylinderTransform.localScale = new Vector3(pd.Radius, pd.Height, pd.Radius);
                    Transform ColliderTransform = pillar.transform.Find("Collider");
                    ColliderTransform.localScale = new Vector3(pd.RewardRadius*1.5f, 0.2f*sceneData.Size.x, pd.RewardRadius*1.5f);
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
                    CylinderTransform.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(pd.Height/2, pd.Height/2);

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
