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
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_FadeInCue")]

    public class P1300_FadeInCue : FSMAction
    {
        public string cueName;
        public bool cueShouldFadeIn = false;
        public float fadeDistance = 100f;
        public P1300_TrialInitLinearTrack trialInitLinearTrack;

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (!cueShouldFadeIn)
                return;

            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            

            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar" + cueName + "_"))
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
