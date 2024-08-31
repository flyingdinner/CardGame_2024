using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CE_HPRegeneration : CartEventBase
    {
        [SerializeField] private int _hp;

        public override float CartEventStart(PlayerBase player, Action callback)
        {
            player.IncomingDamage(-_hp);
            callback?.Invoke();
            return 0.3f;//animation time
        }
    }
}