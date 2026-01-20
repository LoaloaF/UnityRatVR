using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1200/P1200_MovementInitiation")]

    public class P1200_MovementInitiation : FSMAction
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
        }

    }
}
