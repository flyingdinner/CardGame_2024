using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public enum StepType
    {
        none,
        start,
        dice,
        move,
        action,
        shop,
        end,
        tutorial,
    }

    [Serializable]
    public class StepBase
    {
        [field: SerializeField] public StepType type { get; private set; }
 
        public StepBase(StepType t)
        {
            type = t;
        }
    }
}