using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    public abstract class CartEventBase : MonoBehaviour
    {
        //CE_HPRegeneration
        public abstract float CartEventStart(PlayerBase player,Action callback);
        
    }
}