using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    public class PlayerAI : PlayerBase
    {


        protected override IEnumerator IE_StepProcess_Action(StepBase step, Action callback)
        {
            
            // End animation
            yield return new WaitForSeconds(1f);
            callback.Invoke();
        }
    }
}