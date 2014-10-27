using UnityEngine;
using System;
using System.Collections;
using StateMachines;

namespace StageElements
{
    public abstract class StageElementState: State
    {
        public abstract void updateStatus(float percentageToTarget);

    }
}


