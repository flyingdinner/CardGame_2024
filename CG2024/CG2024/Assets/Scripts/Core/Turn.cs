using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public class Turn 
    {
        public event Action<Turn> OnTurnEnd;
        public event Action<Turn> OnTurnStart;

        [field: SerializeField] public PlayerBase player { get; private set; }
        [field: SerializeField] public List<StepBase> steps { get; private set; }

        [SerializeField] private int _stepCounter;
        [SerializeField] private StepBase _currentStep;
        public void Init(List<StepBase> turnSteps, PlayerBase p)
        {
            steps = turnSteps;
            player = p;
            _stepCounter = 0;
        }

        public void TurnStart()
        {
            _stepCounter = 0;
            NextStep();
        }

        private void OnPlayerStepDone(StepBase step)
        {
            _stepCounter++;
            NextStep();
        }

        private void NextStep()
        {
            if(_stepCounter>= steps.Count)
            {
                OnTurnEnd?.Invoke(this);
                return;
            }
            _currentStep = steps[_stepCounter];
            player.StartStep(_currentStep, () => OnPlayerStepDone(_currentStep));
        }
    }
}