using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CE_AddDiceOnRollState : CartEventBase
    {
        [SerializeField] private List<DiceValue> dices;
        //[SerializeField] private bool random; TODO

        public override float CartEventStart(PlayerBase player, Action callback)
        {
            player.AddDices(dices);
            callback?.Invoke();
            return 0.3f;//animation time
        }
    }
}