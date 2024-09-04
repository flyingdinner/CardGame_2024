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
        [field: SerializeField] public List<PlayerBase> playersAI { get; private set; } = new List<PlayerBase>();

        [SerializeField] private GridHolder _gridHolder;

        private void OnValidate()
        {
            playersHuman = new List<PlayerBase>();            
            PlayerBase[] players = FindObjectsByType<PlayerBase>(FindObjectsSortMode.None);

            foreach(PlayerBase pb in players)            
                if (pb.isHuman)                
                    playersHuman.Add(pb);     
        }

        private void OnEnable()
        {
            instanse = this;

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

        public bool AllAIEnemyIsDead()
        {
            foreach (PlayerBase pb in playersAI)
            {
                if (!pb.IsDead())
                {
                    return false;
                }
            }

            return true;
        }

        public void OnLevelLoaded(LevelGO level)
        {
            foreach (PlayerBase pb in playersAI)
            {
                pb.OnTryClick -= PlayersAI_OnTryClick;
            }

            RemoveOldPlayers();

            _gridHolder = level.GridHolder;

            playersHuman[0].transform.parent = null;
            playersHuman[0].transform.position = level._pointPlayer.position;

            playersAI = new List<PlayerBase>();

            foreach (PlayerBase pb in level._NPC)
            {
                playersAI.Add((PlayerBase)pb);
            }

            foreach (PlayerBase pb in playersAI)
            {
                pb.OnTryClick += PlayersAI_OnTryClick;
            }
        }

        private void RemoveOldPlayers()
        {

            if (playersAI != null)
            {
                if (playersAI.Count > 0)
                {
                    for (int i = 0; i < playersAI.Count; i++)
                    {
                        Debug.Log(" playersAI[i] " + playersAI[i].name);
                        Destroy(playersAI[i].gameObject);//TODO pool
                    }
                }
            }
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

            foreach (IAtackTarget target in aliveTargets) // ��������������, ��� � ��� ���� ������ ���� �����
            {
                float distance = Vector3.Distance(point, target.Position()); // ���������� �� ����� �� ����

                if (distance <= range) // ���� ���� � �������� ���������
                {
                    if (centraIAT == null || target != centraIAT) // ��������� ����������� ����, ���� ��� ������
                    {
                        targetsInRange.Add(target);
                    }
                }
            }

            // ���������� �� ���������� �� ��������� � �������
            return targetsInRange.OrderBy(target => Vector3.Distance(point, target.Position())).ToList();
        }

        public Vector3 GetNearestPointToMove(Vector3 start, Vector3 target)
        {
            Vector3[] vectorsToMove =
                {
                Vector3.back,
                Vector3.forward,
                Vector3.right,
                Vector3.left,
                new Vector3(1,0,1),
                new Vector3(1,0,-1),
                new Vector3(-1,0,1),
                new Vector3(-1,0,-1),
            };

            Vector3 nearestPosition = Vector3.zero;
            float nearestDistance = 99999f;

            foreach (Vector3 move in vectorsToMove)
            {
                Vector3 newPosition = start + move;

                if (!GridHolder.CanMoveToPoint(newPosition))
                    continue;

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