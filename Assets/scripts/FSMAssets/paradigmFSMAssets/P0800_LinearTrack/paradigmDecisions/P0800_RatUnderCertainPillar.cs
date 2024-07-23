using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;



namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/P0800/P0800_RatUnderCertainPillar")]
    public class P0800_RatUnderCertainPillar : Decision
    {

        [SerializeField] private string pillarIdentifier;
        public bool returnTrue = false;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            int childcount = stateMachine.transform.childCount;
            
            for (int i = 0; i < childcount; i++)
            {
                Transform child = stateMachine.transform.GetChild(i);

                if (!child.name.StartsWith("Pillar" + pillarIdentifier))
                    continue;

                if (child.GetComponentInChildren<PillarCollision>().PlayerDetected)
                    return returnTrue;
                else
                    return !returnTrue;
            }

            return false;
        }
    }
}