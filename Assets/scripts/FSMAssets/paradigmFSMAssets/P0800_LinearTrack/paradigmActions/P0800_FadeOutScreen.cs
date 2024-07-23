using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_FadeOutScreen")]

    public class P0800_FadeOutScreen : FSMAction
    {
        public FadeScreen fadeScreen;

        public override void Execute(BaseStateMachine stateMachine)
        {
            fadeScreen = FindObjectOfType<FadeScreen>();
            fadeScreen.StartFadeOutScreen(1f);
        }
    }

}


