using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public enum CardType
    {
        none,
        active,// можно використати один раз    
        pasive,// дає постійній ефект
        hot,   // дає ефект/дію одразу після покупки
    }

    public class CardBase : MonoBehaviour
    {
        [field: SerializeField] public CardType types { get; private set; }
        
        
    }
}