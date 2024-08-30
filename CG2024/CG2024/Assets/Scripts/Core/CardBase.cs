using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    public enum CardType
    {
        none,
        active,// можно використати один раз    
        pasive,// дає постійній ефект
        hot,   // дає ефект/дію одразу після покупки
    }

    [Serializable]
    public class CartEventSE
    {
        public StepType stepType;

    }

    public class CardBase : MonoBehaviour
    {
        public bool isActiveCard => IsActiveCard();


        [field: SerializeField] public CardType types { get; private set; }
        [field: SerializeField] public CartEventSE[] cardEventsSE { get; private set; }

        public bool HaveStateEvents(StepType stepType , out List<CartEventSE> eventsSE)
        {
            eventsSE = new List<CartEventSE>();

            for (int i = 0; i < cardEventsSE.Length; i++)
            {
                if (cardEventsSE[i].stepType == stepType)
                    eventsSE.Add(cardEventsSE[i]);
            }

            return eventsSE.Count > 0;
        }

        public bool IsActiveCard()
        {
            for (int i = 0; i < cardEventsSE.Length; i++)
            {
                if (cardEventsSE[i].stepType == StepType.action)
                    return true;                    
            }

            return false;
        }

        public void ShineAnimationPlay(bool loop = false)
        {

        }
    }
}