using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class PlayerService : MonoBehaviour
    {
        public static PlayerService instanse;
        public event Action<IAtackTarget> OnClickOnAtackTarget;

        public bool valid;
        [field: SerializeField] public List<PlayerBase> playersHuman { get; private set; }
        [field: SerializeField] public List<PlayerBase> playersAI { get; private set; }

        private void OnValidate()
        {
            playersHuman = new List<PlayerBase>();
            playersAI = new List<PlayerBase>();
            PlayerBase[] players = FindObjectsByType<PlayerBase>(FindObjectsSortMode.None);

            foreach(PlayerBase pb in players)
            {
                if (pb.isHuman)
                {
                    playersHuman.Add(pb);
                }
                else
                {
                    playersAI.Add(pb);
                }
            }
        }

        private void OnEnable()
        {
            instanse = this;
            foreach (PlayerBase pb in playersAI)
            {
                pb.OnTryClick += PlayersAI_OnTryClick;
            }
        }
        private void OnDisable()
        {
            foreach (PlayerBase pb in playersAI)
            {
                pb.OnTryClick -= PlayersAI_OnTryClick;
            }
        }

        private void PlayersAI_OnTryClick(PlayerBase obj)
        {
            OnClickOnAtackTarget?.Invoke(obj);
        }

    }
}