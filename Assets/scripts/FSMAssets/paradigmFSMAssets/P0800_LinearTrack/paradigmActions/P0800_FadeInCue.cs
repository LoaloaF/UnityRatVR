using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_FadeInCue")]

    public class P0800_FadeInCue : FSMAction
    {
        public string cueName;
        public bool cueShouldFadeIn = false;
        public float fadeDistance = 100f;
        public P0800_TrialInitLinearTrack trialInitLinearTrack;
        private int doubleReward;
        private MeshRenderer cueEnterPillarMesh;

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (stateMachine._sessionManager.trialVariablesDict.ContainsKey("DR"))
            {
                doubleReward = int.Parse(stateMachine._sessionManager.trialVariablesDict["DR"]);
            }
            else
            {
                doubleReward = 0;
            }

            if (!cueShouldFadeIn || doubleReward == 1)
                return;

            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            
            if (cueName == "1")
                cueEnterPillarMesh = trialInitLinearTrack.pillarCylinderMeshes["6"];
            else if (cueName == "2")
                cueEnterPillarMesh = trialInitLinearTrack.pillarCylinderMeshes["10"];
 


            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar" + cueName + "_"))
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
                                    mesh.material.color = new Color(1, 1, 1, 1 - (distance-enterToCueDistance)/fadeDistance);
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
