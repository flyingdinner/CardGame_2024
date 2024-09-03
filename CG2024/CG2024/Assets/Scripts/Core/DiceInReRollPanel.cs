using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    public class DiceInReRollPanel : Button3D
    {
        public event Action<DiceInReRollPanel> OnTryReRollDice;

        [field: SerializeField] public int index;
        [field: SerializeField] public DiceValue currentValue;

        public static Vector3 DiceValueToEulerAngles(DiceValue value)
        {
            switch (value)
            {
                case DiceValue.bonusDamage:
                    return Vector3.zero;

                case DiceValue.heart:
                    return new Vector3(-90, 0, 180);

                case DiceValue.energy:
                    return new Vector3(-90, 0, 270);

                case DiceValue.action:
                    return new Vector3(-90, 0, 0);

                case DiceValue.fail:
                    return new Vector3(-90, 0, 90);

                case DiceValue.move:
                    return new Vector3(180, 0, 90);
            }
            return Vector3.zero;
        }

        public void Initialize(DiceValue dv, int id)
        {
            currentValue = dv;
            index = id;

            RollInPosition(currentValue);
        }

        public override void OnClick()
        {
            OnTryReRollDice?.Invoke(this);
        }

        private void RollInPosition(DiceValue diceValue)
        {
            StartCoroutine(Ie_Rollin(DiceValueToEulerAngles(diceValue)));
        }

        private IEnumerator Ie_Rollin(Vector3 euler)
        {

            Vector3 startPosition = transform.localEulerAngles;
            Vector3 targetPoint = euler;
            float timeElapsed = 0f;
            float duration = 0.4f;

            while (timeElapsed < duration)
            {
                transform.localEulerAngles = Vector3.Lerp(startPosition, targetPoint, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null; // чекаємо наступного кадру
            }

            transform.localEulerAngles = euler;
            yield return new WaitForSeconds(0.1f);

        }
    }
}