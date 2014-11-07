using UnityEngine;
using System;
using System.Collections;
using StateMachines;


namespace StageElements.SpeakerSE.States
{
    public class NonExistent: StageElementState
    {
        public Boolean hardEnter = true;
        public float changeFactor = 0.01f;
        public float percentageToGrow = 0.1f;

        private Vector3 targetScale = Vector3.zero;

        void Update() {
            if (!entering && !exiting) {
                if (transform.localScale != targetScale) {
                    Vector3 diff = targetScale - transform.localScale;

                    if (diff.magnitude < 0.01f) {
                        transform.localScale = targetScale;
                    } else {
                        transform.localScale += diff * changeFactor;
                    }
                }
            }
        }

        public override void updateStatus(float percentageToTarget) {
            if (!entering && !exiting) {
                if (percentageToTarget >= percentageToGrow) {
                    this.stateMachine.SetState<Growing>();
                }
            }
        }

        public override void Enter() {
            if (hardEnter) {
                transform.localScale = targetScale;
            }

            ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>();
            Array.ForEach(pss, ps => ps.Stop());

            base.Enter();
        }

        public override void Exit() {
            ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>();
            Array.ForEach(pss, ps => ps.Play());

            base.Exit();
        }
    }
}


