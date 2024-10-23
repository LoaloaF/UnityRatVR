using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1100/P1100_ResetCurrentRewardInDR")]

    public class P1100_ResetCurrentRewardInDR : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            int doubleReward = int.Parse(stateMachine._sessionManager.trialVariablesDict["DR"]);
            if (doubleReward == 1)
            {
                stateMachine._sessionManager.trialVariablesDict["RN"] = "0";
            }

        }
    }
}
