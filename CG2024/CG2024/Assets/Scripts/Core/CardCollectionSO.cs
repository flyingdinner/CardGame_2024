using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = ("Cards/CardCollection"))]
    public class CardCollectionSO : ScriptableObject
    {
        public CardBase[] collection;
    }
}
