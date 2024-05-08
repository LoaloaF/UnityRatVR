using FSM;
using UnityEngine;

namespace ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/RandomSccessEntry")]
    public class RandomSccessEntry : Decision
    {
        private float timer = 0f;
        private bool decisionCall = false;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (decisionCall)
            {
                decisionCall = false;
                timer = 0f;
            }

            if(timer > 2f)
            {
                decisionCall =  (Random.Range(0, 2) == 0);
                timer = 0f;
                Debug.Log("Random decision call: " + decisionCall);
            }

            timer += Time.deltaTime;
            Debug.Log(timer);
            
            return decisionCall;

            
        }
    }
}
