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
        
        public Dictionary<string, Material> materials = new Dictionary<string, Material>();
        public ExcelSessionMetaData sessionMetaData;
        public SceneGeometryData scene;
        public  GameObject Lighting;
        [HideInInspector] public SessionManager _sessionManager;

        private Color color;   
        private Material transparentMaterial;

        private void Awake() {
            _sessionManager = GetComponent<SessionManager>();
        }

        public void LoadExcelScene(string path)
        {
            // load materials from resources folder
            // this adjusts the path when exec from build subfolder 
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            if (Directory.Exists(Path.Combine(projectPath, "Assets")) == false) 
                projectPath = System.IO.Directory.GetParent(projectPath).FullName;
            if (Directory.Exists(Path.Combine(projectPath, "Assets")) == false) 
                projectPath = System.IO.Directory.GetParent(projectPath).FullName;
            loadMaterials(Path.Combine(projectPath, "Assets/Resources/materials"));

            // generate the scene object from the excel file
            ExcelSceneFileHandler excelScene = new ExcelSceneFileHandler(path);

            // get sceneMetaData (walls and the environment parameters) from the excel
            ExcelSceneMetaData sceneMetaData = excelScene.GetExcelSceneMetaData();

            // get sessionMetatData from the excel and initialize the sessionManager
            sessionMetaData = excelScene.GetExcelSessionMetaData();
            UnityEngine.Debug.Log(sessionMetaData);
            _sessionManager.InitializeSessionManager(sessionMetaData);

            // get excelObjects (pillarMetaData from Envparameters) from the excel and generate the pillars
            List<ExcelObjectData> excelObjects = excelScene.GetExcelSceneObjects();
            List <PillarData> pillars = PillarData.PillarDataFromExcel(excelObjects, excelScene.ScenePlacement());

            // generate sceneGeometryData
            scene = new SceneGeometryData(1, 
                                          sceneMetaData.baseLength, 
                                          sceneMetaData.size,
                                          sceneMetaData.wallTop, 
                                          sceneMetaData.wallBot, 
                                          sceneMetaData.wallRight, 
                                          sceneMetaData.wallLeft, 
                                          pillars, 
                                          sceneMetaData.wallZone,
                                          sceneMetaData.wallZoneCollideDistance);
            
            // construct the scene in Unity
            UnityEngine.Debug.Log("Pillars:" + scene.Pillars);
            LoadScene(scene);
        }

        private void LoadScene(SceneGeometryData sceneData)
        {
            // wallzone size and position
            wallZone.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength);
            wallZone.transform.position = new Vector3(0, 0, 0);

            floor.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength - 0.2f*scene.WallZone * sceneData.BaseLength, 
                                                     1, 
                                                     sceneData.Size.y * 0.1f * sceneData.BaseLength - 0.2f*scene.WallZone * sceneData.BaseLength);
            floor.GetComponent<MeshRenderer>().material.mainTextureScale = 0.1f * (sceneData.Size - new Vector2(scene.WallZone, scene.WallZone)*2)*sceneData.BaseLength;
            floor.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(floor.GetComponent<MeshRenderer>().material.mainTextureScale.x,
                                                                                       floor.GetComponent<MeshRenderer>().material.mainTextureScale.y * 1.732f);

            floor.transform.position = new Vector3(0, 0.01f, 0);
            
            ceiling.transform.localScale = new Vector3(sceneData.Size.x * 0.1f * sceneData.BaseLength, 1, sceneData.Size.y * 0.1f * sceneData.BaseLength);

            // due to different wall rotation setup, the same position could have different effects
            wallTop.transform.rotation = Quaternion.Euler(90, 0, 180);
            wallBottom.transform.rotation = Quaternion.Euler(90, 0, 0);
            wallRight.transform.rotation = Quaternion.Euler(90, 0, 90);
            wallLeft.transform.rotation = Quaternion.Euler(90, 0, -90);

            meshTop.transform.rotation = Quaternion.Euler(90, 0, 90);
            meshBottom.transform.rotation = Quaternion.Euler(90, 0, -90);
            meshRight.transform.rotation = Quaternion.Euler(90, 90, 90);
            meshLeft.transform.rotation = Quaternion.Euler(90, 90, -90);

            // wall height depends on the wall height from excel sheet, which should be fixed in different size of arenas 60cm maybe
            // also remember the ceiling height
            wallTop.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.y, 1, 0.1f * sceneData.TopWall.Height);          
            wallBottom.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.y, 1, 0.1f * sceneData.BottomWall.Height);
            wallRight.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.x, 1, 0.1f * sceneData.RightWall.Height);
            wallLeft.transform.localScale = new Vector3(0.1f * sceneData.BaseLength * sceneData.Size.x, 1, 0.1f * sceneData.LeftWall.Height);
            
            
            // wallTop.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x, 0.5f* sceneData.TopWall.Height, 0);
            wallTop.transform.localPosition = new Vector3(0, 0.5f*sceneData.LeftWall.Height, 0.5f * sceneData.BaseLength * sceneData.Size.y);
            // wallBottom.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x, 0.5f*sceneData.BottomWall.Height, 0);
            wallBottom.transform.localPosition = new Vector3(0, 0.5f*sceneData.RightWall.Height, -0.5f * sceneData.BaseLength * sceneData.Size.y);
            // wallRight.transform.localPosition = new Vector3(0, 0.5f*sceneData.RightWall.Height, -0.5f * sceneData.BaseLength * sceneData.Size.y);
            wallRight.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x, 0.5f* sceneData.TopWall.Height, 0);
            // wallLeft.transform.localPosition = new Vector3(0, 0.5f*sceneData.LeftWall.Height, 0.5f * sceneData.BaseLength * sceneData.Size.y);
            wallLeft.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x, 0.5f*sceneData.BottomWall.Height, 0);

            
            //Adjusted ceiling position for the tree
            ceiling.transform.position = new Vector3(0, sceneData.TopWall.Height + 180f, 0);

            meshTop.transform.localPosition = new Vector3(0.5f * sceneData.BaseLength * sceneData.Size.x -5f, 0.5f* sceneData.TopWall.Height, 0);
            meshBottom.transform.localPosition = new Vector3(-0.5f * sceneData.BaseLength * sceneData.Size.x +5f, 0.5f*sceneData.BottomWall.Height, 0);
            meshRight.transform.localPosition = new Vector3(0, 0.5f*sceneData.RightWall.Height, -0.5f * sceneData.BaseLength * sceneData.Size.y +5f);
            meshLeft.transform.localPosition = new Vector3(0, 0.5f*sceneData.LeftWall.Height, 0.5f * sceneData.BaseLength * sceneData.Size.y -5f);
           
            // wall textures
            wallTop.GetComponent<Renderer>().material = materials[sceneData.TopWall.Texture];
            wallBottom.GetComponent<Renderer>().material = materials[sceneData.BottomWall.Texture];
            wallRight.GetComponent<Renderer>().material = materials[sceneData.RightWall.Texture];
            wallLeft.GetComponent<Renderer>().material = materials[sceneData.LeftWall.Texture];


            foreach (PillarData pd in sceneData.Pillars)
            {
                GameObject pillar = Instantiate(Resources.Load<GameObject>("Pillar"), this.transform); // the pillar object should be stored in the folder "Resources" 
                
                pillar.name = "Pillar" + pd.UID.ToString();
                Vector2 tranformedPos = CoordinateTransform(sceneData, new Vector2(pd.Position.x, pd.Position.y));

                pillar.transform.localPosition = new Vector3(tranformedPos.x * sceneData.BaseLength, pd.Position.z + pd.Height, tranformedPos.y * sceneData.BaseLength * -1f);
                if (pd.Height != 0)
                {
                    Transform CylinderTransform = pillar.transform.Find("Cylinder");
                    CylinderTransform.localScale = new Vector3(pd.Radius*2, pd.Height, pd.Radius*2);
                    Transform ColliderTransform = pillar.transform.Find("Collider");
                    ColliderTransform.localScale = new Vector3(pd.RewardRadius*3f, 0.2f*sceneData.Size.x, pd.RewardRadius*3f);
                    Transform GroundCylinderTransform = pillar.transform.Find("GroundCylinder");
                    GroundCylinderTransform.localScale = new Vector3(pd.RewardRadius*2, GroundCylinderTransform.localScale.y, pd.RewardRadius*2);

                    if (pd.ShowGround != 1)
                        GroundCylinderTransform.gameObject.SetActive(false);

                    if (pd.IsMoving != 1)
                        pillar.GetComponent<PillarMovement>().enabled = false;

                    GroundCylinderTransform.position = new Vector3(GroundCylinderTransform.position.x, 0, GroundCylinderTransform.position.z);
                    // set the texture of the GroundCylinder to gray
                    GroundCylinderTransform.GetComponent<MeshRenderer>().material = materials["GroundCylinder"];

                    if (!materials.ContainsKey(pd.Texture)) {
                        throw new Exception("Material not found: " + pd.Texture);
                    }

                    // pillar texture using materials from resources folder
                    // setting the transparency
                    transparentMaterial = materials[pd.Texture];
                    color = materials[pd.Texture].color;

                    // pillar texture using 4 textures
                    color.a = pd.Transparency;
                    transparentMaterial.color = color;

                    pillar.GetComponentInChildren<MeshRenderer>().material = transparentMaterial;
                    CylinderTransform.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(pd.Height/2, pd.Height/2);

                    // for the visible pillars, we use cube mesh instead of cylinder (only for paradigm 1300)
                    if (pillar.name.StartsWith("Pillar2_") || pillar.name.StartsWith("Pillar104_") || pillar.name.StartsWith("Pillar4_") || pillar.name.StartsWith("Pillar10_")) 
                    {
                        GameObject tempCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        CylinderTransform.GetComponent<MeshFilter>().mesh = tempCube.GetComponent<MeshFilter>().mesh;
                        Destroy(tempCube);
                        CylinderTransform.localScale = new Vector3(pd.Radius * 2, pd.Height * 2, pd.Radius * 2);
                    }
                }
                else
                {
                    pillar.transform.localScale = new Vector3(pd.Radius, 1, pd.Radius);
                    pillar.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
                
            }
        }

        public static Vector2 CoordinateTransform(SceneGeometryData sceneData, Vector2 pos)
        {
            Vector2 translation = -0.5f * sceneData.Size;
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
                Material loadedMaterial = Resources.Load<Material>("materials/" + name);

                if (loadedMaterial != null)
                    materials[name] = loadedMaterial;
            }        
            UnityEngine.Debug.Log("materials count: " + materials.Count);

        }
    }
}
