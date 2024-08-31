using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class DropPlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerBase player;


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PicUp picUp))
            {
                if (picUp.collected) return;

                if(picUp.dropType == DropType.boost)
                {
                    player.AddBoost(picUp.hpBonus, picUp.energyBonus);
                    picUp.SetCollectDrop();
                    return;
                }

                if (picUp.dropType == DropType.card)
                {
                    player.AddCardInHend(picUp.card);
                    picUp.SetCollectDrop();
                    return;
                }

                
            }
        }
    }
}