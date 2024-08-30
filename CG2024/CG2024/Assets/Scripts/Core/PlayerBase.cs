using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    public class PlayerBase : MonoBehaviour
    {
        [field: SerializeField] public List<CardBase> cardsInHend { get; private set; }

        public void StartStep(StepBase step, Action callback)
        {
            switch (step.type)
            {
                case StepType.start:
                    StepProcess_Start(callback);
                    break;

                case StepType.dice:
                    StepProcess_Dice(callback);
                    break;

                case StepType.action:
                    StepProcess_Action(callback);
                    break;

                case StepType.move:
                    StepProcess_Move(callback);
                    break;

                case StepType.shop:
                    StepProcess_Shop(callback);
                    break;

                case StepType.end:
                    StepProcess_End(callback);
                    break;
            }
        }

        protected virtual void StepProcess_Start(Action callback)
        {

        }

        protected virtual void StepProcess_Dice(Action callback)
        {

        }
        protected virtual void StepProcess_Action(Action callback)
        {

        }

        protected virtual void StepProcess_Move(Action callback)
        {

        }
        protected virtual void StepProcess_Shop(Action callback)
        {

        }

        protected virtual void StepProcess_End(Action callback)
        {

        }
    }
}