using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards 
{
    public enum DropType
    {
        boost,
        card,

    }

    public class PicUp : MonoBehaviour
    {

        [field: SerializeField] public bool collected { get; private set; }
        [field: SerializeField] public DropType dropType { get; private set; }
        [field: SerializeField] public int hpBonus { get; private set; }
        [field: SerializeField] public int energyBonus { get; private set; }
        [field: SerializeField] public CardBase card { get; private set; }


        public void SetCollectDrop()
        {
            collected = true;
            gameObject.SetActive(false);
        }
    }
}