using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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
        [SerializeField] private Button _skipButton;
        [SerializeField] private Button _buttonShopOpen;
        [SerializeField] private Button _buttonNextTurn;
        [SerializeField] private PlayerDicePanel _dicepanel;
        [SerializeField] private CardShopPanel _shop;

        protected override void Start()
        {
            base.Start();
            ShowButtons3D(false);
            _buttonShopOpen.gameObject.SetActive(false);
            _buttonNextTurn.gameObject.SetActive(false);
        }


        protected override IEnumerator IE_StepProcess_Dice(StepBase step, Action callback)
        {
            dellStepProcess = () => 
            { 
                callback.Invoke(); 
            };
            
            _currentDices = new List<DiceValue>
            {
                RollRandomDice(),
                RollRandomDice(),
                RollRandomDice(),
            };

            OnStatusCheng?.Invoke(this);

            for (int i = 0; i < cardsInHend.Count; i++)
            {
                yield return new WaitForSeconds(0.05f);
                yield return IE_CheckCardForStepAction(cardsInHend[i], step);
                OnStatusCheng?.Invoke(this);
            }

            yield return new WaitForSeconds(1f);
            _dicepanel.Initialize(this);
        }

        public void OnButtonDiceOk()
        {
            _dicepanel.ShowMainPanel(false);          
            OnStatusCheng?.Invoke(this);
            StartCoroutine(IE_EndDice());
        }

        private IEnumerator IE_EndDice()
        {
            yield return new WaitForSeconds(0.3f);
            Debug.Log("energy count: " + _energy);
            while (true)
            {
                Debug.Log("-----1 ");
                if (TryUseDice(DiceValue.energy))
                {
                    _energy++;
                    Debug.Log("energy count: " + _energy);
                    OnStatusCheng?.Invoke(this);
                }
                else
                {
                    Debug.Log("-----2 ");
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.5f);
            OnStatusCheng?.Invoke(this);
            dellStepProcess.Invoke();
            dellStepProcess = null;
        }

        protected override IEnumerator IE_StepProcess_Action(StepBase step, Action callback)
        {
            // Init Damage
            yield return new WaitForSeconds(0.05f);
            ShowButtons3D(false);
            dellStepProcess = () => { callback.Invoke(); };

            for (int i = 0; i < cardsInHend.Count; i++)            
                if (cardsInHend[i].isActiveCard)
                {
                    cardsInHend[i].ShineAnimationPlay();
                    yield return new WaitForSeconds(0.2f);
                }            

            OnStatusCheng?.Invoke(this);
            currentPlayerSelectStatus = PlayerSelectState.actionStap;
            // End animation
            yield return new WaitForSeconds(0.05f);

            ShowButtons3D(true);
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
            _skipButton.gameObject.SetActive(on);

            bool cantMove = !CheckDice_Move() && !CheckActionDice();
            foreach (Button3D b3 in buttons3D)
            {
                if (cantMove)
                {
                    b3.gameObject.SetActive(false);
                    continue;
                }

                b3.gameObject.SetActive(on);
            }

            PlayerService.instanse.OnClickOnAtackTarget -= OnAttackTargetSelected;
            if (CheckActionDice())
            {
                PlayerService.instanse.OnClickOnAtackTarget += OnAttackTargetSelected;
            }
            OnStatusCheng?.Invoke(this);
        }

        public void OnSkipButton()// from unity event
        {
            Debug.Log(">> OnSkipButton >>");
            dellStepProcess.Invoke();
            dellStepProcess = null;
            ShowButtons3D(false);
        }

        private void OnAttackTargetSelected(IAtackTarget target)
        {
            if(Vector3.Distance(target.Position(), transform.position) <= attackRange && !target.IsDead())
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
                yield return null; // чекаємо наступного кадру
            }

            // У кінці гарантовано встановлюємо цільову позицію
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

        public override DiceValue RerollDice(int id)
        {
            _currentDices[id] = RollRandomDice();
            OnStatusCheng?.Invoke(this);
            return _currentDices[id];
        }

        protected override void ReInitTurn()
        {
            ShowButtons3D(true);
        }

        protected override void ActionCompleted()
        {
            dellStepProcess.Invoke();
            dellStepProcess = null;

            currentPlayerSelectStatus = PlayerSelectState.notControllStep;

            //---
            OnStatusCheng?.Invoke(this);
        }
        //-------------------------------------------------------------------------------
        //                              Shop
        //-------------------------------------------------------------------------------
        protected override void StepProcess_Shop(StepBase step, Action callback)
        {           
            StartCoroutine(IE_ShopStart(callback));
        }

        private IEnumerator IE_ShopStart(Action callback)
        {            
            yield return new WaitForSeconds(0.1f);

            dellStepProcess = () => {
                callback.Invoke();
                Debug.Log("StepProcess_Shop");
            };

            _buttonShopOpen.gameObject.SetActive(true);
            _buttonNextTurn.gameObject.SetActive(true);

        }

        public void ButtonShowShop()
        {

            _buttonShopOpen.gameObject.SetActive(false);
            _buttonNextTurn.gameObject.SetActive(false);
            _shop.Show(this);
        }

        public void OnShopCloseButton()
        {
            _buttonShopOpen.gameObject.SetActive(true);
            _buttonNextTurn.gameObject.SetActive(true);
        }

        public void ButtonNextTurn()
        {
            _buttonShopOpen.gameObject.SetActive(false);
            _buttonNextTurn.gameObject.SetActive(false);
            dellStepProcess.Invoke();
            dellStepProcess = null;
        }

        public override bool TryToUseEnergy(int price)
        {
            if (price <= _energy)
            {
                _energy -= price;
                OnStatusCheng?.Invoke(this);
                return true;
            }
            return false;
        }

    }
}