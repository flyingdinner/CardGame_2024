using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{

    public class CardHolder : MonoBehaviour
    {
        [Serializable]
        private class HolderPoint
        {
            public Transform point;
            public CardBase card;
        }
        [field: SerializeField] public List<CardBase> cardsInHend { get; private set; }
                
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
            foreach(HolderPoint hp in _cardPositions)
            {
                if (hp.card == null)
                {
                    AddInPoint(hp, card);

                    return true;
                }
            }

            return false;
        }

        private void AddInPoint(HolderPoint hp ,CardBase card)
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

            foreach(CardBase card in cardsInHend)
            {
               armor += card.passiveStats.armor;
            }

            int result = Math.Clamp(damage - armor, 0, damage);

            Debug.Log("UseCardsArmore : damage " + damage +  " - armore " + armor + " = result : " + result);
            return result;            
        }

    }
}