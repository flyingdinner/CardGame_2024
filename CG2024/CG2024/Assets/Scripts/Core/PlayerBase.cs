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

    public enum DiceValue
    {
        heart,
        energy,
        action,
        fail,
        move,
        bonusDamage,
    }
    

    public class PlayerBase : Button3D, IAtackTarget
    {
        public event Action<PlayerBase> OnTryClick;

        public int initiative = 1;//To do

        [field: SerializeField] public bool isHuman { get; private set; } = false;
        [field: SerializeField] public List<CardBase> cardsInHend { get; private set; }
        [field: SerializeField] public float _baseAttackRange { get; private set; }
        [field: SerializeField] public List<DiceValue> currentDices { get; private set; }
        [field: SerializeField] public GameObject _attackBullet { get; private set; }
        [field: SerializeField] public int baseDammage { get; private set; } = 1;
        [field: SerializeField] public int baseBonusDammage { get; private set; } = 2;
        [field: SerializeField] public HPService HP { get; private set; }
        [field: SerializeField] public PlayerAnimator anim { get; private set; }

        private void OnEnable()
        {
            anim.PlayAlive();
            HP.OnDead += HP_OnDead;
        }


        private void OnDisable()
        {
            HP.OnDead -= HP_OnDead;
        }

        private void OnValidate()
        {
            if (cardsInHend == null)
            {
                cardsInHend = new List<CardBase>();
            }
        }
        private void HP_OnDead(HPService obj)
        {
            anim.PlayDead();
        }

        public void StartStep(StepBase step, Action callback)
        {

            //Debug.Log("PlayerBase >> " + gameObject.name + " > start step :" + step.type);
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
            StartCoroutine(IE_StepProcess_Dice(step, callback));
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

        protected virtual IEnumerator IE_StepProcess_Dice(StepBase step, Action callback)
        {
            currentDices = new List<DiceValue>
            {
                DiceValue.action,
                DiceValue.move,
            };

            Debug.Log("IE_StepProcess_Dice ");
            yield return new WaitForSeconds(0.05f);
            callback.Invoke();
        }


        protected virtual IEnumerator IE_StepProcess_Action(StepBase step, Action callback)
        {
            Debug.Log(" BASE :: IE_StepProcess_Action");

            yield return new WaitForSeconds(0.05f);
            callback.Invoke();
        }

        //IAtackTarget---------------------------------------
        public Vector3 Position()
        {
            return transform.position; 
        }

        public bool IsDead()
        {
            return !HP.alive;
        }

        public void IncomingDamage(int damage)
        {
            HP.AddDamage(damage);
        }
        //---------------------------------------------------

        // Button3D
        public override void OnClick()
        {
            OnTryClick?.Invoke(this);
        }

        protected void UseAttackOnTarget(IAtackTarget target)
        {
            //кістку асктіон вже використали
            int realDamage = baseDammage + baseBonusDammage * GetDiceCountByType(DiceValue.bonusDamage);
            target.IncomingDamage(realDamage);
        }

        protected bool TryUseDice(DiceValue value)
        {
            if (currentDices.Contains(value))
            {
                currentDices.Remove(value);
                return true;
            }

            return false;
        }

        protected int GetDiceCountByType(DiceValue value)
        {
            int count = 0;
            foreach(DiceValue d in currentDices)            
                if (d == value) count++;            

            return count;
        }

        protected bool CheckDice_Move()
        {
            if (currentDices.Contains(DiceValue.move))
                return true;

            return false;
        }

        protected bool CheckActionDice()
        {
            if (currentDices.Contains(DiceValue.action))
                return true;

            return false;
        }
    }
}