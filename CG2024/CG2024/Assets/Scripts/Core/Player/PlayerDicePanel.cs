using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    [Serializable]
    public class DiceHoldPosition
    {
        public Transform diceTr => dice.transform;

        public Transform point;
        public Transform hide;
        public DiceInReRollPanel dice;

        public void Initialize(DiceValue value, int id)
        {
            dice.gameObject.SetActive(true);
            dice.Initialize(value, id);           
        }

        public void SetInHidePosition()
        {
            dice.transform.position = hide.position;
        }
    }

    public class PlayerDicePanel : MonoBehaviour
    {
        [SerializeField] private DiceHoldPosition[] _dicePoints;
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private PlayerHuman _playerBase;
        [SerializeField] private int rerollCounter = 0;
        [SerializeField] private GameObject _buttonOk;
 
        private void OnEnable()
        {
            _mainPanel.SetActive(false);
            _buttonOk.SetActive(false);
        }

        public void OnOkButton()
        {
            _playerBase.OnButtonDiceOk();
            _buttonOk.SetActive(false);
        }

        public void ShowMainPanel(bool on)
        {
            _mainPanel.SetActive(on);
        }

        public void Initialize(PlayerHuman player)
        {
            ShowMainPanel(true);
            _playerBase = player;
            rerollCounter = _playerBase.rerollCounts;

            for (int i = 0; i < player.currenDices.Count && i < _dicePoints.Length; i++)
            {
                _dicePoints[i].Initialize(player.currenDices[i], i);

                StartCoroutine(Ie_MoveDiceToStartPosition(_dicePoints[i]));
            }
        }

        private IEnumerator Ie_MoveDiceToStartPosition(DiceHoldPosition dice)
        {
            _buttonOk.SetActive(false);
            dice.dice.OnTryReRollDice -= Dice_OnTryReRollDice;
            dice.SetInHidePosition();

            yield return new WaitForSeconds(0.05f);
            Vector3 startPosition = dice.hide.position;
            Vector3 targetPoint = dice.point.position;
            float timeElapsed = 0f;
            float duration = 0.4f;

            while (timeElapsed < duration)
            {
                dice.diceTr.position = Vector3.Lerp(startPosition, targetPoint, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null; // чекаємо наступного кадру
            }

            // У кінці гарантовано встановлюємо цільову позицію
            dice.diceTr.position = targetPoint;
            yield return new WaitForSeconds(0.1f);

            dice.dice.OnTryReRollDice += Dice_OnTryReRollDice;
            _buttonOk.SetActive(true);
        }

        private void Dice_OnTryReRollDice(DiceInReRollPanel dice)
        {
            if (rerollCounter > 0)
            {
                rerollCounter--;
                
                dice.Initialize(_playerBase.RerollDice(dice.index), dice.index);
                StartCoroutine(Ie_MoveDiceToStartPosition(_dicePoints[dice.index]));
            }
        }
    }
}