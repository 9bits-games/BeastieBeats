using UnityEngine;
using System;
using System.Collections;
using StateMachines;


namespace StageElements.Lights.States
{
    public class On: StageElementState
    {
        Animation a;
        void Update() {
            if (!entering && !exiting) {
                
            }
        }

        public override void updateStatus(float percentageToTarget) {
            if (!entering && !exiting) {
            }
        }

        public override void Enter() {
            base.Enter();
        }

        public override void Exit() {
            base.Exit();
        }
    }
}