using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public enum StepType
    {
        start,
        dice,
        move,
        action,
        shop,
        end,
    }

    public class StepBase : MonoBehaviour
    {
        [field: SerializeField] public StepType type { get; private set; }
    }
}