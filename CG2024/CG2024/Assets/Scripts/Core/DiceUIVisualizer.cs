using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Cards
{
    [Serializable]
    public class DiceUISettings
    {
        public Sprite sprite;
        public DiceValue value;

    }

    public class DiceUIVisualizer : MonoBehaviour
    {
        [field: SerializeField] public DiceUISettings[] settings { get; private set; }
        [field: SerializeField] public Image _imageFrame;
        [field: SerializeField] public Image _imageMain;

        void Start()
        {
            Hide();
        }

        public void Hide()
        {
            _imageFrame.enabled = false;
            _imageMain.enabled = false;
        }

        public void Show(DiceValue value, bool isBonus)
        {
            _imageMain.enabled = true;
            _imageFrame.enabled = isBonus;

            _imageMain.sprite = GetSpriteByDiceValue(value);
        }

        public Sprite GetSpriteByDiceValue(DiceValue value)
        {
            foreach(DiceUISettings s in settings)
            {
                if (s.value == value)
                    return s.sprite;
            }
            return null;
        }

    }
}