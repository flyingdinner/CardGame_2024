using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    public enum PlayerSelectState
    {
        notControllStep,
        actionStap,
        shopStep,
    }

    public class PlayerHuman : PlayerBase
    {
        public event Action<PlayerHuman> OnStatusCheng;

        public delegate void PlayerDelegate();

        private PlayerDelegate dellStepProcess;

        public PlayerSelectState currentPlayerSelectStatus = PlayerSelectState.notControllStep;

        [SerializeField] private Button3D[] buttons3D;

        protected override void Start()
        {
            base.Start();
            ShowButtons3D(false);            
        }

        protected override IEnumerator IE_StepProcess_Action(StepBase step, Action callback)
        {
            ShowButtons3D(false);

            dellStepProcess = () => { 
                callback.Invoke();
                Debug.Log("PlayerDelegate");
            };

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

            OnStatusCheng?.Invoke(this);
            currentPlayerSelectStatus = PlayerSelectState.actionStap;
            // End animation
            yield return new WaitForSeconds(0.05f);

            ShowButtons3D(true);
            //
            //ActionCompleted();
        }
        


        public void MoveTo(MoveButton moveb)
        {
            if (TryUseDice(DiceValue.move))
            {
                StartCoroutine(IeMoveTo(moveb));
                return;
            }

            if (TryUseDice(DiceValue.action))
            {
                StartCoroutine(IeMoveTo(moveb));
                return;
            }

            ShowButtons3D(false);
        }

        public void ShowButtons3D(bool on)
        {
            foreach(Button3D b3 in buttons3D)
            {
                b3.gameObject.SetActive(on);
            }

            PlayerService.instanse.OnClickOnAtackTarget -= OnAttackTargetSelected;
            if (CheckActionDice())
            {
                PlayerService.instanse.OnClickOnAtackTarget += OnAttackTargetSelected;
            }
        }

        private void OnAttackTargetSelected(IAtackTarget target)
        {
            if(Vector3.Distance(target.Position(), transform.position) <= _baseAttackRange && !target.IsDead())
            {
                if (TryUseDice(DiceValue.action))
                {
                    PlayerService.instanse.OnClickOnAtackTarget -= OnAttackTargetSelected;
                    ShowButtons3D(false);
                    StartCoroutine(IE_Attack(target));
                    return;
                }
            }
                Debug.Log(">>>>> ------- DEAD or Distanse ------------");
        }

        protected IEnumerator IE_Attack(IAtackTarget target)
        {
            //animation

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
                yield return null; // ������ ���������� �����
            }

            // � ���� ����������� ������������ ������� �������
            _attackBullet.transform.position = targetPoint;
            yield return new WaitForSeconds(0.1f);

            _attackBullet.SetActive(false);
            UseAttackOnTarget(target);
            yield return null;

            if (!CheckActionDice() && !CheckDice_Move())
            {
                ActionCompleted();
            }
            else
            {
                ReInitTurn();
            }
        }

        protected IEnumerator IeMoveTo(MoveButton moveb)
        {
            ShowButtons3D(false);
            Vector3 startPosition = transform.position;
            Vector3 targetPoint = moveb.point;
            float timeElapsed = 0f;
            float duration = 0.4f;

            while (timeElapsed < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPoint, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null; // ������ ���������� �����
            }

            // � ���� ����������� ������������ ������� �������
            transform.position = targetPoint;
            yield return new WaitForSeconds(0.1f);

            if(!CheckActionDice() && !CheckDice_Move())
            {
                ActionCompleted();
            }
            else
            {
                ReInitTurn();
            }
        }

        private void ReInitTurn()
        {
            ShowButtons3D(true);
        }


        private void ActionCompleted()
        {
            dellStepProcess.Invoke();
            currentPlayerSelectStatus = PlayerSelectState.notControllStep;

            //---
            OnStatusCheng?.Invoke(this);
        }
    }
}