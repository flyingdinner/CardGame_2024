using System;
using UnityEngine;

namespace Cards
{
    public class CE_AddEnergy : CartEventBase
    {
        [SerializeField] private int _energy;

        public override float CartEventStart(PlayerBase player, Action callback)
        {
            player.AddBoost(0, _energy);
            callback?.Invoke();
            return 0.3f;//animation time
        }

    }
}