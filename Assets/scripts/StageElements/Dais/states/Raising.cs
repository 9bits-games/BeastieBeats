using UnityEngine;
using System;
using System.Collections;
using StateMachines;


namespace StageElements.Dais.States
{
    public class Raising: StageElementState
    {
        public float percentageForMaxScale = 0.8f;
        public float percentageForMinScale = 0.3f;
        public float factorSpeed = 0.1f;

        public float beginingTargetScale = -0.24f;
        public float finalTargetScale = 1.5f;

        private Vector3 actualTarget3DScale = Vector3.zero;

        void Update() {
            if (!entering && !exiting) {

                if (transform.position != actualTarget3DScale) {
                    Vector3 diff = actualTarget3DScale - transform.position;

                    if (diff.magnitude < 0.01f) {
                        transform.position = actualTarget3DScale;
                    } else {
                        transform.position += diff * factorSpeed;
                    }
                }

            }
        }


        public override void updateStatus(float percentageToTarget) {
            float scale = Mathf.Lerp(beginingTargetScale, finalTargetScale,
                Mathf.InverseLerp(percentageForMinScale, percentageForMaxScale, percentageToTarget));


            actualTarget3DScale = transform.position;
            actualTarget3DScale.y = scale;
        }

        public override void Enter() {
            base.Enter();
        }

        public override void Exit() {
            base.Exit();
        }
    }
}