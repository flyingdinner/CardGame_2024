using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cards
{
    public class AIDiceVisualiser : MonoBehaviour
    {
        [field: SerializeField] public DiceUISettings[] settings { get; private set; }

        [SerializeField] private SpriteRenderer[] _diceSprites;
        [SerializeField] private PlayerBase _playerBase;

        private void OnEnable()
        {
            DiceReset();
        }

        private void DiceReset()
        {
            Hide();

            for (int i = 0; i < _playerBase.currenAllDices.Count; i++)
            {
                _diceSprites[i].gameObject.SetActive(true);
                _diceSprites[i].sprite = GetSpriteByDiceValue(_playerBase.currenAllDices[i]);
            }
        }

        private void Hide()
        {
            foreach (var dice in _diceSprites) { 
            dice.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            Hide();
        }

        public Sprite GetSpriteByDiceValue(DiceValue value)
        {
            foreach (DiceUISettings s in settings)
            {
                if (s.value == value)
                    return s.sprite;
            }
            return null;
        }
    }
}