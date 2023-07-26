using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class FiniteStateMachine<T>
    {
        protected Dictionary<T, BaseState<T>> FSMStates;

        protected BaseState<T> CurrentState;

        public FiniteStateMachine()
        {
            FSMStates = new Dictionary<T, BaseState<T>>();
        }

        public void Add(BaseState<T> state)
        {
            FSMStates.Add(state.StateID, state);
        }

        public void Add(T id, BaseState<T> state)
        {
            FSMStates.Add(id, state);
        }

        public BaseState<T> GetStatus(T stateID)
        {
            if (FSMStates.ContainsKey(stateID))
                return FSMStates[stateID];

            return null;
        }

        public void SetStatus(BaseState<T> state)
        {
            if (CurrentState == state)
                return;

            if (CurrentState != null)
                CurrentState.Exit();

            CurrentState = state;
            if (CurrentState != null)
                CurrentState.Enter();
        }

        public void Update()
        {
            if (CurrentState != null)
                CurrentState.Update();
        }

        public void FixedUpdate()
        {
            if (CurrentState != null)
                CurrentState.FixedUpdate();
        }
    }
}
