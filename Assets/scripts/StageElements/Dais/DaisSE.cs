using UnityEngine;
using System;
using System.Collections;
using StageElements.Dais.States;

namespace StageElements.Dais
{
    public class DaisSE: StageElement
    {
        void Start() {
            SetState<Static>();
        }
    }
}
