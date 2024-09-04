using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = ("Cards/CardCollection"))]
    public class LevelCollectionSO : ScriptableObject
    {
        public GameObject[] levelsGO;
    }
}