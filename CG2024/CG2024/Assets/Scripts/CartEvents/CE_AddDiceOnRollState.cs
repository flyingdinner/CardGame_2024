using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CE_AddDiceOnRollState : CartEventBase
    {
        [SerializeField] private bool _isBonusDices = true;
        [SerializeField] private List<DiceValue> dices;
        //[SerializeField] private bool random; TODO

        public override float CartEventStart(PlayerBase player, Action callback)
        {
            player.AddDices(dices,_isBonusDices);
            callback?.Invoke();
            return 0.3f;//animation time
        }
    }
}