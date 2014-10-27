using UnityEngine;
using System;
using System.Collections;
using StateMachines;


namespace StageElements.Lights.States
{
    public class Off: StageElementState
    {
        public float percentageToTurnOn = 0.8f;

        void Update() {
            if (!entering && !exiting) {
            }
        }

        public override void updateStatus(float percentageToTarget) {
            if (!entering && !exiting) {
                if (percentageToTarget >= percentageToTurnOn) {
                    this.stateMachine.SetState<On>();
                }
            }
        }

        public override void Enter() {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.enabled = false;
            base.Enter();
        }

        public override void Exit() {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.enabled = true;
            base.Exit();
        }
    }
}