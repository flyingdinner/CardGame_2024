using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    public enum PlayerStatesType
    {
        none,
        stan,
        life,
        dead,
    }
    

    public class PlayerBase : MonoBehaviour
    {  
        public int initiative = 1;//To do

        [field: SerializeField] public bool isHuman { get; private set; } = false;
        [field: SerializeField] public List<CardBase> cardsInHend { get; private set; }

        private void OnValidate()
        {
            if (cardsInHend == null)
            {
                cardsInHend = new List<CardBase>();
            }
        }

        public void StartStep(StepBase step, Action callback)
        {
            Debug.Log("PlayerBase >> " + gameObject.name + " > start step :" + step.type);
            switch (step.type)
            {
                case StepType.start:
                    StepProcess_Start(step, callback);
                    break;

                case StepType.dice:
                    StepProcess_Dice(step, callback);
                    break;

                case StepType.action:
                    StepProcess_Action(step, callback);
                    break;

                case StepType.move:
                    StepProcess_Move(step, callback);
                    break;

                case StepType.shop:
                    StepProcess_Shop(step, callback);
                    break;

                case StepType.end:
                    StepProcess_End(step, callback);
                    break;
            }
        }

        protected virtual void StepProcess_Start(StepBase step, Action callback)
        {
            StartCoroutine(IE_StepProcess_Start(step,callback));
        }

        protected virtual void StepProcess_Dice(StepBase step, Action callback)
        {
            //TO DO
            callback?.Invoke();

        }
        protected virtual void StepProcess_Action(StepBase step, Action callback)
        {
            StartCoroutine(IE_StepProcess_Action(step, callback));
        }

        protected virtual void StepProcess_Move(StepBase step, Action callback)
        {
            //TO DO
            callback?.Invoke();
        }

        protected virtual void StepProcess_Shop(StepBase step, Action callback)
        {
            //TO DO
            callback?.Invoke();
        }

        protected virtual void StepProcess_End(StepBase step, Action callback)
        {
            StartCoroutine(IE_StepProcess_End(step, callback));
        }

        private IEnumerator IE_StepProcess_Start(StepBase step, Action callback)
        {
            for (int i = 0; i < cardsInHend.Count; i++)
            {
                yield return new WaitForSeconds(0.05f);
                yield return IE_CheckCardForStepAction(cardsInHend[i], step);
            }

            // Start Animation
            yield return new WaitForSeconds(0.05f);
            callback.Invoke();
        }


        private IEnumerator IE_StepProcess_End(StepBase step, Action callback)
        {
            for (int i = 0; i < cardsInHend.Count; i++)
            {
                yield return new WaitForSeconds(0.05f);
                yield return IE_CheckCardForStepAction(cardsInHend[i], step);
            }

            // End animation
            Debug.Log("IE_StepProcess_End >> ------------------------------------------ ");
            yield return new WaitForSeconds(0.5f);
            callback.Invoke();
        }

        private IEnumerator IE_CheckCardForStepAction(CardBase card, StepBase step)
        {
            if (card.HaveStateEvents(step.type, out List<CartEventSE> eventsSE))
            {
                //do card action eventsSE
                yield return new WaitForSeconds(0.05f);
            }

            yield return null;
        }

        protected virtual IEnumerator IE_StepProcess_Action(StepBase step, Action callback)
        {
            if (isHuman)
            {
                for (int i = 0; i < cardsInHend.Count; i++)
                {
                    if (cardsInHend[i].isActiveCard)
                    {
                        cardsInHend[i].ShineAnimationPlay();
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }

            // End animation
            yield return new WaitForSeconds(0.05f);
            callback.Invoke();
        }

    }
}