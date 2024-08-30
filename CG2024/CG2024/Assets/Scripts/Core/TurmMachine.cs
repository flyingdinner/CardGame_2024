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
        private void Start()
        {
            //for test~
            StartTurmMachine();
        }

        public void StartTurmMachine()
        {
            turns = new List<Turn>();

            Array.Sort(_players, (x, y) => x.initiative.CompareTo(y.initiative));
            
            foreach(PlayerBase pb in _players)
            {
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
            OnPlyerTurnStart?.Invoke(_currentTurn);
            _currentTurn.TurnStart();
            _currentTurn.OnTurnEnd += CurrentTurn_OnTurnEnd;
        }

        private void CurrentTurn_OnTurnEnd(Turn obj)
        {
            obj.OnTurnEnd += CurrentTurn_OnTurnEnd;
            OnPlyerTurnEnd?.Invoke(_currentTurn);
            // TO DO ANIMATION
            StartNextTurn();
        }

        private void StartNextTurn()
        {
            _counter++;
            _currentTurn = turns[_counter];
            CurrentTurnStart();
        }


    }
}
