using UnityEngine;
using System;
using System.Collections;

namespace StateMachines {

    public class State: MonoBehaviour9Bits {
        //Triggered when the state has fully entered:
        public delegate void StateEntered(State state);
        public event StateEntered OnStateEntered;

        //Triggered when the state has fully exited:
        public delegate void FullyExited(State state);
        public event FullyExited OnFullyExited;

        public StateMachine stateMachine;

        protected bool entering;
        protected bool exiting;

//        public State(): base() {
//            this.enabled = false;
//        }

        void Start() {
            this.enabled = false;
            this.entering = false;
            this.exiting = false;

            OnStateEntered = null;
            OnFullyExited = null;
        }

        public virtual void Enter() {
            this.entering = true;
            FullyEnter();
        }

        public virtual void Exit() {
            this.exiting = true;
            FullyExit();
        }

        protected virtual void FullyEnter() {
            this.enabled = true;
            this.entering = false;
            if (OnStateEntered != null)
                OnStateEntered(this);
        }

        protected virtual void FullyExit() {
            this.enabled = false;
            this.exiting = false;
            if (OnFullyExited != null)
                OnFullyExited(this);
        }
    }

}

