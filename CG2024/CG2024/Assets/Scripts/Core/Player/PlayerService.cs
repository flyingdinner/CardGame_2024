using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class PlayerService : MonoBehaviour
    {
        

        public static PlayerService instanse;
        public event Action<IAtackTarget> OnClickOnAtackTarget;

        public List<IAtackTarget> aliveTargets => GetAllAlive();

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

        public List<IAtackTarget> GetAllAlive()
        {
            List<IAtackTarget> allTargets = new List<IAtackTarget>();
            List<IAtackTarget> aliveTargets = new List<IAtackTarget>();
            allTargets.AddRange(playersHuman);
            allTargets.AddRange(playersAI);
            
            foreach(IAtackTarget iat in allTargets)
            {
                if (iat == null) continue;

                aliveTargets.Add(iat);
            }

            return aliveTargets;
        }

        public PlayerBase GetNearestHumanPlayer(Vector3 point)
        {
            if(playersHuman.Count == 1)
            {
                return playersHuman[0];
            }

            return null;//TO DO
        }

        public List<IAtackTarget> GetAllAliveInRange(Vector3 point, float range, IAtackTarget centraIAT = null)
        {

            List<IAtackTarget> targetsInRange = new List<IAtackTarget>();

            foreach (IAtackTarget target in aliveTargets) // Предполагается, что у вас есть список всех целей
            {
                float distance = Vector3.Distance(point, target.Position()); // Расстояние от точки до цели

                if (distance <= range) // Если цель в пределах диапазона
                {
                    if (centraIAT == null || target != centraIAT) // Исключаем центральную цель, если она задана
                    {
                        targetsInRange.Add(target);
                    }
                }
            }

            // Сортировка по расстоянию от ближайших к дальним
            return targetsInRange.OrderBy(target => Vector3.Distance(point, target.Position())).ToList();
        }

        public Vector3 GetNearestPointToMove(Vector3 start, Vector3 target)
        {
            Vector3[] vectorsToMove =
                {
                Vector3.back,
                Vector3.down,
                Vector3.right,
                Vector3.left,
                new Vector3(1,1,0),
                new Vector3(1,-1,0),
                new Vector3(-1,1,0),
                new Vector3(-1,-1,0),
            };

            Vector3 nearestPosition = Vector3.zero;
            float nearestDistance = 99999f;

            foreach (Vector3 move in vectorsToMove)
            {
                Vector3 newPosition = start + move;

                if (CheckPointIsBusy(newPosition))
                    continue;

                float distance = Vector3.Distance(newPosition, target);

                if (distance < nearestDistance)
                {
                    nearestPosition = newPosition;
                    nearestDistance = distance;
                }
            }

            return nearestPosition;
        }

        public bool CheckPointIsBusy(Vector3 vector3)
        {
            foreach(IAtackTarget iat in aliveTargets)            
                if (Vector3.Distance(vector3, iat.Position()) < 0.1f)
                    return true;
            

            return false;
        }
    }
}