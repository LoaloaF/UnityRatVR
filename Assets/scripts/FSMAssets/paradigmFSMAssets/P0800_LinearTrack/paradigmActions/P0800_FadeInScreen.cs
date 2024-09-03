using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_FadeInScreen")]

    public class P0800_FadeInScreen : FSMAction
    {
        public FadeScreen fadeScreen;
        public override void Execute(BaseStateMachine stateMachine)
        {
            fadeScreen = FindObjectOfType<FadeScreen>();
            fadeScreen.StartFadeInScreen(0.5f);
        }
    }

}


