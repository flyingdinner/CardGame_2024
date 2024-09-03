using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private PlayerHuman _playerHuman;
        [SerializeField] private TurmMachine _turmMachine;
        [SerializeField] private PlayerService _playerService;

        [Header("-- UI Panels --")]
        [SerializeField] private GameObject _startGamePanel;
        [SerializeField] private GameObject _nextLevelPanel;
        [SerializeField] private GameObject _loserLevelPanel;
        [SerializeField] private GameObject _playerInGameUI;
 
        void Start()
        {
            _startGamePanel.SetActive(true);            
        }

        public void StartGame()
        {
            _startGamePanel.SetActive(false);
            StartCoroutine(Ie_StartGameLoop());      
        }

        private IEnumerator Ie_StartGameLoop()
        {
            _startGamePanel.SetActive(false);//TODO animation
            yield return new WaitForSeconds(1f);

            _turmMachine.StartTurmMachine();
            _turmMachine.OnPlyerTurnEnd += TurmMachine_OnPlyerTurnEnd;
            _playerInGameUI.SetActive(true);
        }

        private void TurmMachine_OnPlyerTurnEnd(Turn obj)
        {
            if (_playerHuman.IsDead())
            {
                EndGameLoop(false);
                return;
            }

            if (_playerService.AllAIEnemyIsDead())
            {
                EndGameLoop(true);
                return;
            }

            _turmMachine.StartNextTurn();

        }

        private void EndGameLoop(bool win)
        {
            _turmMachine.OnPlyerTurnEnd -= TurmMachine_OnPlyerTurnEnd;
            _playerInGameUI.SetActive(false);

            _loserLevelPanel.SetActive(!win);
            _nextLevelPanel.SetActive(win);
            _turmMachine.StopTurmMachine();

        }
    }
}