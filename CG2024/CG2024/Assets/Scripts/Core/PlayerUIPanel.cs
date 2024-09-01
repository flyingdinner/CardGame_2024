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

        private void OnEnable()
        {
            _playerHuman.OnStatusCheng += _playerHuman_OnStatusCheng;
        }

        private void OnDisable()
        {
            _playerHuman.OnStatusCheng -= _playerHuman_OnStatusCheng;
        }

        private void _playerHuman_OnStatusCheng(PlayerHuman obj)
        {
            _tEnergy.text = _playerHuman.energy.ToString();
        }
    }
}