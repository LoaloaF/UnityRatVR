using UnityEngine;

namespace FSM
{
    public abstract class Decision : ScriptableObject
    {
        public string switchDescription;
        public abstract bool Decide(BaseStateMachine state);
    }
}