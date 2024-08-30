using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    public class PlayerHuman : PlayerBase
    {

        protected override IEnumerator IE_StepProcess_Action(StepBase step, Action callback)
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