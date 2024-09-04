using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatusVisualizer : MonoBehaviour
{
    [SerializeField] private PlayerHuman _player;
    [SerializeField] private Transform _attackRangeCircle;
    [SerializeField] private Transform _attackAoECircle;

    private void OnEnable()
    {
        _player.OnStatusCheng += Player_OnStatusCheng;    
    }

    private void OnDisable()
    {
        _player.OnStatusCheng-= Player_OnStatusCheng;
    }

    private void Player_OnStatusCheng(PlayerHuman obj)
    {
        if (_player.inActionState && _player.GetDiceCountByType(DiceValue.action)>0)
        {
            _attackRangeCircle.gameObject.SetActive(true);

            Vector3 rangeScale = new Vector3(_player.attackRange, 1, _player.attackRange);
            _attackRangeCircle.localScale = rangeScale;


            //_attackAoECircle.gameObject.SetActive(_player.);

        }
        else
        {
            _attackRangeCircle.gameObject.SetActive(false);
            _attackAoECircle.gameObject.SetActive(false);
        }
    
    }
}
