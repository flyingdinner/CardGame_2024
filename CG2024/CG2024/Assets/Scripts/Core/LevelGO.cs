using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class LevelGO : MonoBehaviour
    {
        [field: SerializeField] public Transform _pointPlayer { get; private set; }
        [field: SerializeField] public PlayerAI[] _NPC { get; private set; }


    }
}