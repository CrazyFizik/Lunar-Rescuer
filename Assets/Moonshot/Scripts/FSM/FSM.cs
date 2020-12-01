using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public delegate void ActionState();

    [System.Serializable]
    public class FSM
    {
        public Stack<ActionState> _stack;
        public int _number = 0;

        public FSM()
        {
            _stack = new Stack<ActionState>();
        }

        public void Update()
        {
            ActionState state = GetCurrentState();
            if (state != null)
            {
                state();
            }
            _number = _stack.Count;
        }

        public ActionState PopState()
        {
            return _stack.Pop();
        }

        public void PushState(ActionState state)
        {
            if (state != GetCurrentState())
            {
                _stack.Push(state);
            }
        }

        public ActionState GetCurrentState()
        {
            if (_stack.Count > 0)
            {
                return _stack.Peek();
            }
            else
            {
                return null;
            }
        }
    }
}
