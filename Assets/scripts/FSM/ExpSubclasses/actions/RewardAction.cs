using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/RewardAction")]

    public class RewardAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            Debug.Log("runs when in reward action, play sound, give reward");
        }

    }
}
