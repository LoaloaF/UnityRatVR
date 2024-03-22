using UnityEngine;

namespace FSM
{
    public class BaseState : ScriptableObject

    {
        public int stateID = -1;

        public virtual void Execute(BaseStateMachine machine) { }
    }
}