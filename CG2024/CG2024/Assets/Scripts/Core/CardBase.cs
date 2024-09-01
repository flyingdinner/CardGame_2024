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

    public enum CardReaction
    {
        onGetHeal,
        onGetDamage,
        onDoDamage,
    }

    [Serializable]
    public class CartEventSE
    {

        public StepType stepType;
        public CartEventBase cevent;
    }

    [Serializable]
    public class PassiveStats
    {
        public bool haveDamageBonus = false;

        public int maxHP = 0;
        public int armor = 0;        
        public int armorTemporarily = 0;
        public float damageMultiplier = 0;
        public float rangeBonus = 0;
        public float AoERadius = 0;
    }

    [Serializable]
    public class PfxContainer
    {
        public bool usePFX;
        public ParticleSystem pfxStart;
        public ParticleSystem pfxMove;
        public ParticleSystem pfxEnd;
        public ParticleSystem pfxUse;

    }

    public class CardBase : MonoBehaviour
    {
        public bool isActiveCard => IsActiveCard();

        public PassiveStats passiveStats;
        [field: SerializeField] public CardType types { get; private set; }
        [field: SerializeField] public CartEventSE[] cardEventsSE { get; private set; }

        [field: SerializeField] public PfxContainer pfxContainer { get; private set; }
                
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

        public void PlaySinglPulse()
        {

        }

        public void UseCartEvent(StepType stepType , PlayerBase player)
        {
            for (int i = 0; i < cardEventsSE.Length; i++)
            {
                if (cardEventsSE[i].stepType == stepType)
                {
                    cardEventsSE[i].cevent.CartEventStart(player,()=> { });
                }
            }
        }
    }
}