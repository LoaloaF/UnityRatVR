using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0500/P0500_TrialStartMotorLearning")]

    public class P0500_TrialStartMotorLearning : FSMAction
    {
        public Queue<float> rawMovementQueue;
        public Queue<float> yawMovementQueue;
        public Queue<float> pitchMovementQueue;

        public override void Execute(BaseStateMachine stateMachine)
        {
            rawMovementQueue = new Queue<float>();
            yawMovementQueue = new Queue<float>();
            pitchMovementQueue = new Queue<float>();

            rawMovementQueue.Enqueue(0);
            yawMovementQueue.Enqueue(0);
            pitchMovementQueue.Enqueue(0);

            stateMachine._sessionManager.currentRewardNum = 0;
            stateMachine._sessionManager.rewardSucked = false;
            stateMachine._sessionManager.newTrial();
            Debug.Log("New trial started");
        }

    }
}
