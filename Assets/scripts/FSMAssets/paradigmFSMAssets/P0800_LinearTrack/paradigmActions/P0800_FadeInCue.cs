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
        public override void Execute(BaseStateMachine stateMachine)
        {

            if (!cueShouldFadeIn)
                return;

            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");

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
                                mesh.material.color = new Color(1, 1, 1, 1 - (distance-20)/fadeDistance);
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
