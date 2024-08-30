using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cards
{
    public abstract class PlayerBase : MonoBehaviour
    {
        [field: SerializeField] public List<CardBase> cardsInHend { get; private set; }
        
        
    }
}