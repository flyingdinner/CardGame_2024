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

        public float attackRange => _baseAttackRange + cardHolder.bonusDamage.rangeBonus;

        public List<CardBase> cardsInHend => cardHolder.cardsInHend;

        [field: SerializeField] public bool isHuman { get; private set; } = false;
        [field: SerializeField] public float _baseAttackRange { get; private set; }
        [field: SerializeField] public List<DiceValue> currentDices { get; private set; }
        [field: SerializeField] public GameObject _attackBullet { get; private set; }
        [field: SerializeField] public int baseDammage { get; private set; } = 1;
        [field: SerializeField] public int baseBonusDammage { get; private set; } = 2;
        [field: SerializeField] public HPService HP { get; private set; }
        [field: SerializeField] public PlayerAnimator anim { get; private set; }

        [field: SerializeField] public int energy { get; private set; }

        [field: SerializeField] public CardHolder cardHolder { get; private set; }

        

        private void OnEnable()
        {
            anim.PlayAlive();
            HP.OnDead += HP_OnDead;
        }


        private void OnDisable()
        {
            HP.OnDead -= HP_OnDead;
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
            CardBonusDamage cardBonusDamage = new CardBonusDamage();

            foreach (CardBase cb in cardsInHend)
                cardBonusDamage.TryAddDamageCard(cb);

            cardHolder.SetCardBonusDamage(cardBonusDamage);

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
            //Debug.Log("IE_StepProcess_End >> ------------------------------------------ ");
            yield return new WaitForSeconds(0.5f);
            callback.Invoke();
        }

        private IEnumerator IE_CheckCardForStepAction(CardBase card, StepBase step)
        {
            if (card.HaveStateEvents(step.type, out List<CartEventSE> eventsSE))
            {
                //do card action eventsSE
                card.UseCartEvent(step.type, this);

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

            // Debug.Log("IE_StepProcess_Dice ");
            for (int i = 0; i < cardsInHend.Count; i++)
            {
                yield return new WaitForSeconds(0.05f);
                yield return IE_CheckCardForStepAction(cardsInHend[i], step);
            }

            while (TryUseDice(DiceValue.energy))
            {
                energy++;
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(0.05f);
            callback.Invoke();
        }


        protected virtual IEnumerator IE_StepProcess_Action(StepBase step, Action callback)
        {
            Debug.Log(" BASE :: IE_StepProcess_Action");

            yield return new WaitForSeconds(0.05f);
            callback.Invoke();
        }

        protected IEnumerator IE_Attack(IAtackTarget target)
        {
            //animation
            CardBonusDamage cardBonusDamage = new CardBonusDamage();

            foreach(CardBase cb in cardsInHend)
            {
                if (cardBonusDamage.TryAddDamageCard(cb))
                {
                    cb.PlaySinglPulse();
                    yield return new WaitForSeconds(0.1f);
                }
            }

            //            
            cardHolder.SetCardBonusDamage(cardBonusDamage);

            yield return new WaitForSeconds(0.1f);

            _attackBullet.SetActive(true);
            _attackBullet.transform.position = transform.position;
            Vector3 startPosition = transform.position;
            Vector3 targetPoint = target.Position();
            float timeElapsed = 0f;
            float duration = 0.2f;


            while (timeElapsed < duration)
            {
                _attackBullet.transform.position = Vector3.Lerp(startPosition, targetPoint, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null; // чекаємо наступного кадру
            }

            // У кінці гарантовано встановлюємо цільову позицію
            _attackBullet.transform.position = targetPoint;
            yield return new WaitForSeconds(0.1f);

            _attackBullet.SetActive(false);

            yield return UseAttackOnTarget(target);   

            if (!CheckActionDice() && !CheckDice_Move())
            {
                ActionCompleted();
            }
            else
            {
                ReInitTurn();
            }
        }


        protected virtual void ReInitTurn()
        {
        }

        protected virtual void ActionCompleted()
        {  
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
            if (damage > 0)
            {
                HP.AddDamage(cardHolder.UseCardArmor(damage));
            } else
            {
                HP.AddDamage(damage);
            }
        }
        //---------------------------------------------------

        // Button3D
        public override void OnClick()
        {
            OnTryClick?.Invoke(this);
        }

        protected IEnumerator UseAttackOnTarget(IAtackTarget target)
        {
            //кістку асктіон вже використали

            int realDamage = cardHolder.UseCardBonusDamage(baseDammage + baseBonusDammage * GetDiceCountByType(DiceValue.bonusDamage));
            target.IncomingDamage(realDamage);
            yield return new WaitForSeconds(0.1f);
            Debug.Log("cardHolder.HaveAoEBonus : " + cardHolder.HaveAoEBonus());
            if (cardHolder.HaveAoEBonus())
            {
                List<IAtackTarget> aoeList =  PlayerService.instanse.GetAllAliveInRange(target.Position(), cardHolder.bonusDamage.AoERadius, target);
                foreach (IAtackTarget iat in aoeList)
                {
                    Debug.Log(">>> CardHolder.HaveAoEBonus >>>");
                    iat.IncomingDamage(realDamage);
                    yield return new WaitForSeconds(0.05f);
                }
            }
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

        public void AddDices(List<DiceValue> dices)
        {
            currentDices.AddRange(dices);
        }

        public void AddBoost(int hp, int e)
        {
            //Debug.Log(" Boost up >> hp : " + hp + " >> e : " +e);
            IncomingDamage(-hp);
            energy += e;
        }

        public void AddCardInHend(CardBase card)
        {
            if(cardHolder.TryAddInHolder(card))
            {

            }
            else
            {
                //pri add in bug
            }
        }
    }
}