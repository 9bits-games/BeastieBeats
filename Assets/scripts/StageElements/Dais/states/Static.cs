using UnityEngine;
using System;
using System.Collections;
using StateMachines;


namespace StageElements.Dais.States
{
    public class Static: StageElementState
    {
        public float changeFactor = 0.01f;
        public float percentageToStartRaising = 0.3f;

        void Update() {
            if (!entering && !exiting) {
            }
        }

        public override void updateStatus(float percentageToTarget) {
            if (!entering && !exiting) {
                if (percentageToTarget >= percentageToStartRaising) {
                    this.stateMachine.SetState<Raising>();
                }
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