using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1100/P1100_MovementInQueue")]

    public class P1100_MovementInQueue : FSMAction
    {
        public P1100_MovementInitiation movementInitiation;
        public float rawMovementTemp;
        public float yawMovementTemp;
        public float pitchMovementTemp;

        public override void Execute(BaseStateMachine stateMachine)
        {
            int queueSize = 10;
            if (movementInitiation.rawMovementQueue.Count > queueSize)
            {
                movementInitiation.rawMovementQueue.Dequeue();
                movementInitiation.yawMovementQueue.Dequeue();
                movementInitiation.pitchMovementQueue.Dequeue();
            }

            rawMovementTemp = stateMachine._playerMovement.XYZvelInput[0] * stateMachine._playerMovement.ballForwardNormToCentimeter / queueSize;
            yawMovementTemp = stateMachine._playerMovement.XYZvelInput[1] * stateMachine._playerMovement.ballSidewaysNormToCentimeter / queueSize;
            pitchMovementTemp = stateMachine._playerMovement.XYZvelInput[2] * stateMachine._playerMovement.ballRotatationNormToCentimeter / queueSize;

            movementInitiation.rawMovementQueue.Enqueue(rawMovementTemp);
            movementInitiation.yawMovementQueue.Enqueue(yawMovementTemp);
            movementInitiation.pitchMovementQueue.Enqueue(pitchMovementTemp);
        }

    }
}
