using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1200/P1200_MovementInQueue")]

    public class P1200_MovementInQueue : FSMAction
    {
        public P1200_MovementInitiation movementInitiation;
        public float rawMovementTemp;
        public float yawMovementTemp;
        public float pitchMovementTemp;
        public P0800_TrialInitLinearTrack trialInitLinearTrack;

        public override void Execute(BaseStateMachine stateMachine)
        {
            int queueSize = 1;
            if (movementInitiation.rawMovementQueue.Count > queueSize)
            {
                movementInitiation.rawMovementQueue.Dequeue();
                movementInitiation.yawMovementQueue.Dequeue();
                movementInitiation.pitchMovementQueue.Dequeue();
            }

            rawMovementTemp = stateMachine._playerMovement.XYZvelInput[0] * stateMachine._playerMovement.ballForwardNormToCentimeter / queueSize;
            yawMovementTemp = stateMachine._playerMovement.XYZvelInput[1] * trialInitLinearTrack.ballSidewaysNormToCentimeter / queueSize;
            pitchMovementTemp = stateMachine._playerMovement.XYZvelInput[2] * trialInitLinearTrack.ballRotatationNormToCentimeter / queueSize;

            movementInitiation.rawMovementQueue.Enqueue(Mathf.Abs(rawMovementTemp));
            movementInitiation.yawMovementQueue.Enqueue(Mathf.Abs(yawMovementTemp));
            movementInitiation.pitchMovementQueue.Enqueue(Mathf.Abs(pitchMovementTemp));
        }

    }
}
