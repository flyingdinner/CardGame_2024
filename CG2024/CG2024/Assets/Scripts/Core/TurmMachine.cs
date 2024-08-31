using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class TurmMachine : MonoBehaviour
    {
        public event Action<Turn> OnPlyerTurnStart;
        public event Action<Turn> OnPlyerTurnEnd;

        [SerializeField] private PlayerBase[] _players;
               
        [field:SerializeField] public List<Turn> turns { get; private set; }
        [SerializeField] private int _counter;
        [SerializeField] private Turn _currentTurn;

        [SerializeField] private PlayerService playerService;

        [SerializeField] private GameObject _currentPlayerCursor;

        private void OnValidate()
        {
            if(playerService==null)
            playerService = GetComponent<PlayerService>();
        }

        private void Start()
        {

            //for test~
            StartTurmMachine();
        }

        public void StartTurmMachine()
        {
            turns = new List<Turn>();

            List<PlayerBase> allPlayers = new List<PlayerBase>();

            allPlayers.AddRange(playerService.playersHuman);
            allPlayers.AddRange(playerService.playersAI);

            _players = allPlayers.ToArray();
            Array.Sort(_players, (x, y) => x.initiative.CompareTo(y.initiative));
            
            foreach(PlayerBase pb in _players)
            {
                if (!pb.HP.alive) continue;

                turns.Add(CreatTurn(pb));
            }

            StartGameLoop();
        }

        private Turn CreatTurn(PlayerBase pb)
        {
            Turn turn = new Turn();
         
            List<StepBase> steps = new List<StepBase>
            {
               new StepBase(StepType.start),
               new StepBase(StepType.dice),
               new StepBase(StepType.action),
               new StepBase(StepType.move),
               new StepBase(StepType.shop),
               new StepBase (StepType.end),
            };

            turn.Init(steps, pb);

            return turn;
        }

        private void StartGameLoop()
        {
            _counter = 0;
            _currentTurn = turns[_counter];
            
            CurrentTurnStart();
        }

        private void CurrentTurnStart()
        {
            if (_currentTurn.player.HP.alive)
            {

                OnPlyerTurnStart?.Invoke(_currentTurn);
                _currentTurn.TurnStart();
                _currentTurn.OnTurnEnd += CurrentTurn_OnTurnEnd;

                _currentPlayerCursor.SetActive(true);
                _currentPlayerCursor.transform.position = _currentTurn.player.transform.position;
                _currentPlayerCursor.transform.parent = _currentTurn.player.transform;
            }
            else
            {
                StartNextTurn();
            }
        }

        private void CurrentTurn_OnTurnEnd(Turn obj)
        {
            _currentPlayerCursor.SetActive(false);
            _currentPlayerCursor.transform.parent = null;

            obj.OnTurnEnd -= CurrentTurn_OnTurnEnd;
            OnPlyerTurnEnd?.Invoke(_currentTurn);
            // TO DO ANIMATION
            StartNextTurn();
        }

        private void StartNextTurn()
        {            
            _counter++;
            if (_counter >= turns.Count)
            {                
                StartTurmMachine();
                return;
            }               
            
            _currentTurn = turns[_counter];
            CurrentTurnStart();
        }
    }
}
