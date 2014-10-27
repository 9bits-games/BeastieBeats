using UnityEngine;
using System;
using System.Collections;

namespace StateMachines
{
    public class StateMachine: MonoBehaviour9Bits
    {
        private State state;

        protected State State {
            get{ return state; }
            set{ state = value; }
        }

        private State[] states;

        void Awake() {
            states = GetComponents<State>();

            Array.ForEach(states, StateRegistered);
        }

        void Update() {}

        protected virtual void StateRegistered(State state) {
            state.stateMachine = this;
        }

        public void SetState<T>() where T : State {
            State newState = GetComponent<T>();

            if (state != null) {
                state.Exit();
                state.OnFullyExited += s => EnterState(newState);
            } else {
                EnterState(newState);
            }
        }

        private void EnterState(State newState) {
            if (state != null) {
                state.OnFullyExited -= EnterState;
            }
            state = newState;
            newState.Enter();
        }
    }
}

