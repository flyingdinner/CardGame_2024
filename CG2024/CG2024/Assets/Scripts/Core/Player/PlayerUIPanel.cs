using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Cards
{
    public class PlayerUIPanel : MonoBehaviour
    {
        [SerializeField] private PlayerHuman _playerHuman;
        [SerializeField] private TextMeshProUGUI _tEnergy;
        [SerializeField] private DiceUIVisualizer[] diceUIVisualizers;

        private void OnEnable()
        {
            _playerHuman.OnStatusCheng += _playerHuman_OnStatusCheng;
        }

        private void OnDisable()
        {
            _playerHuman.OnStatusCheng -= _playerHuman_OnStatusCheng;
        }

        private void _playerHuman_OnStatusCheng(PlayerHuman player)
        {
            _tEnergy.text = _playerHuman.energy.ToString();
            int index = 0;

            foreach(DiceValue dv in player.currenBonusDices)
            {
                diceUIVisualizers[index].Show(dv, true);
                index++;
            }

            foreach (DiceValue dv in player.currenDices)
            {
                diceUIVisualizers[index].Show(dv, false);
                index++;
            }

            for (int i = index; i < diceUIVisualizers.Length; i++)
            {
                diceUIVisualizers[i].Hide();
            }
        }
    }
}