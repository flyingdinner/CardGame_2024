using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    [Serializable]
    public class HPService
    {
        public event Action<HPService> OnHPChenge;
        public event Action<HPService> OnDead;

        public bool alive => _currentHP > 0 && !_dead;
        public int maxHP => _baseHP + _bonusHP;
        public int currentHP => _currentHP;

        [SerializeField] private int _baseHP;
        [SerializeField] private int _bonusHP;
        [SerializeField] private int _currentHP;
        [SerializeField] private bool _dead;

        public void AddDamage(int damage)
        {
            if (_dead) return;

            _currentHP -= damage;
            if (_currentHP <= 0)
            {
                _dead = true;
                OnDead?.Invoke(this);
            }

            _currentHP = Math.Clamp(_currentHP, 0, maxHP);
            OnHPChenge?.Invoke(this);

            Debug.Log(" + HPService : " + _currentHP + " / " + maxHP + " :: alive " + alive);
        }
    }
}