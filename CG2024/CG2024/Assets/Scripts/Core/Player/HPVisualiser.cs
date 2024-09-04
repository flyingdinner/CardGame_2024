using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class HPVisualiser : MonoBehaviour
    {            
        [SerializeField] private PlayerBase player;
        [SerializeField] private GameObject[] hpPoints;

        private void OnEnable()
        {
            player.HP.OnHPChenge += HP_OnHPChenge;
        }

        private void OnDisable()
        {
            player.HP.OnHPChenge -= HP_OnHPChenge;
        }

        private void Start()
        {
            HP_OnHPChenge(player.HP);
        }

        private void HP_OnHPChenge(HPService hp)
        {
            for (int i = 0; i < hpPoints.Length; i++)
            {
                hpPoints[i].SetActive(hp.currentHP > i);
            }
        }
    }
}