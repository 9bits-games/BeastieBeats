using UnityEngine;
using System;
using System.Collections;
using StageElements.Lights.States;

namespace StageElements.Lights
{
    public class Light: StageElement
    {

        void Start() {
            SetState<Off>();
        }

    }
}
