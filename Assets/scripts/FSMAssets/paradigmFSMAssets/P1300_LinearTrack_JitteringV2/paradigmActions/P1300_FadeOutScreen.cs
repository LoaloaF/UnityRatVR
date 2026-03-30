using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_FadeOutScreen")]

    public class P1300_FadeOutScreen : FSMAction
    {
        public FadeScreen fadeScreen;

        public override void Execute(BaseStateMachine stateMachine)
        {
            fadeScreen = FindObjectOfType<FadeScreen>();
            fadeScreen.StartFadeOutScreen(0.5f);
        }
    }

}


