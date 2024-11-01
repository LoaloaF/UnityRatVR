using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/P0500_MaxRewardReached")]
    public class P0500_MaxRewardReached : Decision
    {
        public P0500_MovementInQueue movementInQueue;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            int maxRewardNum = int.Parse(stateMachine._sessionManager.trialVariablesDict["MRN"]);

            if (stateMachine._sessionManager.currentRewardNum < maxRewardNum)
            {
                return false;
            }
            else
            {
                Debug.Log("Maximum number of rewards reached. Trial ends.");
                Color white = new Color(1, 1, 1, 1);
                stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
                return true;
            }
        }

    }

}


