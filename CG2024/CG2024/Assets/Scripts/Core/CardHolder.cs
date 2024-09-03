using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{

    [Serializable]
    public class CardBonusDamage
    {
        public float multiplier = 1f;
        public float rangeBonus = 0f;
        public float AoERadius = 0f;

        public List<ParticleSystem> _pfxOnEnd;
        public List<ParticleSystem> _pfxOnFly;
        public List<ParticleSystem> _pfxOnStart;

        public CardBonusDamage()
        {
            multiplier = 1f;
            rangeBonus = 0f;
            AoERadius = 0;
            _pfxOnEnd = new List<ParticleSystem>();
            _pfxOnFly = new List<ParticleSystem>();
            _pfxOnStart = new List<ParticleSystem>();
        }

        public int CalculateDammage(int damage)
        {
            return (int)((float)damage * multiplier);
        }

        public bool TryAddDamageCard(CardBase cb)
        {
            if (cb.passiveStats.haveDamageBonus)
            {
                multiplier += cb.passiveStats.damageMultiplier;
                rangeBonus += cb.passiveStats.rangeBonus;
                AoERadius += cb.passiveStats.AoERadius;

                if (cb.pfxContainer == null) return true;
                if (cb.pfxContainer.usePFX)
                {
                    if (cb.pfxContainer.pfxStart != null)
                        _pfxOnStart.Add(cb.pfxContainer.pfxStart);

                    if (cb.pfxContainer.pfxMove != null)
                        _pfxOnFly.Add(cb.pfxContainer.pfxMove);

                    if (cb.pfxContainer.pfxEnd != null)
                        _pfxOnEnd.Add(cb.pfxContainer.pfxEnd);
                }
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class HolderPoint
    {
        public Transform point;
        public CardBase card;
        //
        public GameObject buttonUse;
        public GameObject buttonRemove;
    }

    //-----------------------------------------------------------------------------------
    public class CardHolder : MonoBehaviour
    {
        public HolderPoint[] cardPositions => _cardPositions;

        [field: SerializeField] public List<CardBase> cardsInHend { get; private set; }
        [field: SerializeField] public CardBonusDamage bonusDamage { get; private set; }

        [SerializeField] private HolderPoint[] _cardPositions;

        private void OnValidate()
        {
            if (cardsInHend == null)
            {
                cardsInHend = new List<CardBase>();
            }
        }

        public bool TryAddInHolder(CardBase card)
        {
            foreach (HolderPoint hp in _cardPositions)
            {
                if (hp.card == null)
                {
                    AddInPoint(hp, card);

                    return true;
                }
            }

            return false;
        }

        public void OnShopClosed(List<CardBase> cards)
        {            
            cardsInHend = new List<CardBase>();

            foreach (HolderPoint cp in _cardPositions)
            {
                cp.card = null;
            }

            Debug.Log("OnShopClosed :: cards.Count " + cards.Count + " :: _cardPositions " + _cardPositions.Length);
            for (int i = 0; i < cards.Count; i++)
            {
                AddInPoint(_cardPositions[i], cards[i]);
            }
            
        }

        private void AddInPoint(HolderPoint hp, CardBase card)
        {
            cardsInHend.Add(card);
            hp.card = card;
            card.transform.parent = hp.point;
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
        }

        public int UseCardArmor(int damage)
        {
            int armor = 0;

            foreach (CardBase card in cardsInHend)
            {
                armor += card.passiveStats.armor;
            }

            int result = Math.Clamp(damage - armor, 0, damage);

            Debug.Log("UseCardsArmore : damage " + damage + " - armore " + armor + " = result : " + result);
            return result;
        }

        public int UseCardBonusDamage(int damage)
        {

            return bonusDamage.CalculateDammage(damage);
        }

        public void SetCardBonusDamage(CardBonusDamage bd)
        {
            bonusDamage = bd;
        }

        public bool HaveAoEBonus()
        {
            return bonusDamage.AoERadius > 0;
        }
    }
}