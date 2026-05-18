using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_FadeInZone")]

    public class P1300_FadeInZone : FSMAction
    {
        public string cueNameLeft;
        public string cueNameRight;
        public string cueNameCeiling;

        public string zoneDefinition; //reward or cue

        public bool cueShouldFadeIn = false;
        public float fadeDistance = 0f;
        public P1300_TrialInitLinearTrack trialInitLinearTrack;
        private MeshRenderer cueEnterPillarMesh;

        public override void Execute(BaseStateMachine stateMachine)
        {
            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            
            if (!cueShouldFadeIn){

                foreach (GameObject pillar in pillars)
                {
                    if (pillar.name.StartsWith("Pillar" + cueNameCeiling + "_") || pillar.name.StartsWith("Pillar" + cueNameLeft + "_") || pillar.name.StartsWith("Pillar" + cueNameRight + "_"))
                    {
                        MeshRenderer[] meshRenderer = pillar.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer mesh in meshRenderer)
                        {
                            if (mesh.gameObject.name == "Cylinder")
                            {
                                if (zoneDefinition == "cueZone")
                                {
                                    mesh.material.color = new Color(1, 1, 1, 1);
                                    if (stateMachine._sessionManager.trialLogDict["CZV_FID"] == "")
                                    {
                                    stateMachine._sessionManager.trialLogDict["CZV_FID"] = Time.frameCount.ToString();
                                    stateMachine._sessionManager.trialLogDict["CZV_PCT"] = stateMachine._sessionManager.getUnixTimestampMicroseconds().ToString();
                                    }


                                }
                                else if (zoneDefinition == "rewardZone")
                                {
                                    mesh.material.color = new Color(1, 1, 1, 1);
                                    if (stateMachine._sessionManager.trialLogDict["RZV_FID"] == "")
                                    {
                                        stateMachine._sessionManager.trialLogDict["RZV_FID"] = Time.frameCount.ToString();
                                        stateMachine._sessionManager.trialLogDict["RZV_PCT"] = stateMachine._sessionManager.getUnixTimestampMicroseconds().ToString();
                                    }
                                }     
                            }
                        }
                    }
                }
                return;
            }
            
            
            // fadeinlogic, deprecated and not fully updated
            if (cueNameCeiling == "1" || cueNameCeiling == "101" || cueNameCeiling == "102")
                cueEnterPillarMesh = trialInitLinearTrack.pillarCylinderMeshes["1"];
            else if (cueNameCeiling == "2" || cueNameCeiling == "201" || cueNameCeiling == "202")
                cueEnterPillarMesh = trialInitLinearTrack.pillarCylinderMeshes["2"];

            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar" + cueNameCeiling + "_") || pillar.name.StartsWith("Pillar" + cueNameLeft + "_") || pillar.name.StartsWith("Pillar" + cueNameRight + "_"))
                {
                    if (stateMachine._playerMovement.transform.position.z < pillar.transform.position.z)
                    {
                        float distance = pillar.transform.position.z - stateMachine._playerMovement.transform.position.z;
                        MeshRenderer[] meshRenderer = pillar.GetComponentsInChildren<MeshRenderer>();

                        foreach (MeshRenderer mesh in meshRenderer)
                        {
                            if (mesh.gameObject.name == "Cylinder")
                            {
                                float enterToCueDistance = pillar.transform.position.z - cueEnterPillarMesh.transform.position.z;

                                if (1 - (distance-enterToCueDistance)/fadeDistance < 0)
                                    mesh.material.color = new Color(1, 1, 1, 0);
                                else
                                    mesh.material.color = new Color(1, 1, 1, 1);
                                    // mesh.material.color = new Color(1, 1, 1, 1 - (distance-enterToCueDistance)/fadeDistance);
                            }
                        }
                    }
                    else
                    {
                        MeshRenderer[] meshRenderer = pillar.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer mesh in meshRenderer)
                        {
                            if (mesh.gameObject.name == "Cylinder")
                            {
                                mesh.material.color = new Color(1, 1, 1, 1);
                            }
                        }
                    }
                }
            }






        }
    }
}



