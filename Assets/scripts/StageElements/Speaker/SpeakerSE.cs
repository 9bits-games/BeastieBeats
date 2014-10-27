using UnityEngine;
using System;
using System.Collections;
using StageElements.SpeakerSE.States;

namespace StageElements.SpeakerSE
{
    public class SpeakerSE: StageElement
    {

        void Start() {
            SetState<NonExistent>();
        }

//        public override void updateStatus(float percentageToTarget) {
//            base.updateStatus(percentageToTarget);
//        }
    }
}

