using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public interface IAtackTarget
    {
        public Vector3 Position();

        public bool IsDead();

        public void IncomingDamage(int damage);
    }
}