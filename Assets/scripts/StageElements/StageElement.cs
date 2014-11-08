using UnityEngine;
using System;
using System.Collections;
using StateMachines;

namespace StageElements
{
    public abstract class StageElement : StateMachine {
        protected float percentageToTarget;

        new protected StageElementState State {
            get{ return base.State as StageElementState; }
            set{ base.State = value; }
        }

//        protected override void StateRegistered(State state) {
//            base.StateRegistered(state);
//        }

        public virtual void updateStatus(float percentageToTarget) {
            this.percentageToTarget = percentageToTarget;
//            Debug.Log("Perc " + percentageToTarget);

            State.updateStatus(percentageToTarget);
        }
    }
}
