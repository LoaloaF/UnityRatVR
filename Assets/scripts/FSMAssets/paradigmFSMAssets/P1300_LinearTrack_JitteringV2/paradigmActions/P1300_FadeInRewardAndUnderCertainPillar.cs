using FSM;
using RatVR.Scene;
using UnityEngine;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_FadeInRewardAndUnderCertainPillar")]
    public class P1300_FadeInRewardAndUnderCertainPillar : FSMAction
    {
        [SerializeField] private string pillarIdentifier;
        public string cueName;
        public string cueName2;
        public bool cueShouldFadeIn = false;
        public float fadeDistance = 100f;
        public P1300_TrialInitLinearTrack trialInitLinearTrack;

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (!cueShouldFadeIn)
                return;

            if (!RatPassCertainPillar(stateMachine))
                return;

            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            

            foreach (GameObject pillar in pillars)
            {
                if (!pillar.name.StartsWith("Pillar" + cueName + "_") && !pillar.name.StartsWith("Pillar" + cueName2 + "_"))
                    continue;

                    MeshRenderer[] meshRenderers = pillar.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mesh in meshRenderers)
                {
                    if (mesh.gameObject.name == "Cylinder")
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

        private bool RatPassCertainPillar(BaseStateMachine stateMachine)
        {
            int childcount = stateMachine.transform.childCount;
            
            for (int i = 0; i < childcount; i++)
            {
                Transform child = stateMachine.transform.GetChild(i);

                if (!child.name.StartsWith("Pillar" + pillarIdentifier + "_"))
                    continue;

                if (child.position.z < stateMachine._playerMovement.transform.position.z)
                    return true;
                else
                    return false;
            }

            return false;
        }
    }  
}
