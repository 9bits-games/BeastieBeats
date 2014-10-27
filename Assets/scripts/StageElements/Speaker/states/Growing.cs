using UnityEngine;
using System;
using System.Collections;
using StateMachines;


namespace StageElements.SpeakerSE.States
{
    public class Growing: StageElementState {
        public float percentageForMaxScale = 0.7f;
        public float percentageForMinScale = 0.1f;

        public float beginingTargetScale = 1f;
        public float finalTargetScale = 1.5f;
        public Vector3 baseScale = Vector3.one;

        private Vector3 actualTarget3DScale = Vector3.zero;

        void Update() {
            if (!entering && !exiting) {
            
                if (transform.localScale != actualTarget3DScale) {
                    Vector3 diff = actualTarget3DScale - transform.localScale;

                    if (diff.magnitude < 0.01f) {
                        transform.localScale = actualTarget3DScale;
                    } else {
                        transform.localScale += diff * 0.2f;
                    }
                }
            }
        }

        public override void updateStatus(float percentageToTarget) {

            float scale = Mathf.Lerp(beginingTargetScale, finalTargetScale,
                Mathf.InverseLerp(percentageForMinScale, percentageForMaxScale, percentageToTarget));

            actualTarget3DScale = scale * baseScale;
        }

        public override void Enter() {
            transform.localScale = actualTarget3DScale;

            base.Enter();
        }

        public override void Exit() {
            base.Exit();
        }
    }
}



