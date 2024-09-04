using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cards
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private PlayerHuman _playerHuman;
        [SerializeField] private TurmMachine _turmMachine;
        [SerializeField] private PlayerService _playerService;
        [SerializeField] private LevelLoader _loader;

        [Header("-- UI Panels --")]
        [SerializeField] private GameObject _startGamePanel;
        [SerializeField] private GameObject _nextLevelPanel;
        [SerializeField] private GameObject _loserLevelPanel;
        [SerializeField] private GameObject _playerInGameUI;

        private int _currentLevel = 0;

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
            _loader.LoadLevel(_currentLevel);


            _startGamePanel.SetActive(false);//TODO animation

            yield return new WaitForSeconds(0.5f);

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
            if (win)
            {
                _currentLevel++;
            }
            else
            {
                _currentLevel = 0;
            }

            _turmMachine.OnPlyerTurnEnd -= TurmMachine_OnPlyerTurnEnd;
            _playerInGameUI.SetActive(false);

            _loserLevelPanel.SetActive(!win);
            _nextLevelPanel.SetActive(win);
            _turmMachine.StopTurmMachine();

        }

        public void StartNextLevel()
        {
            if (_currentLevel > 0)
            {
                StartGame();
                _loserLevelPanel.SetActive(false);
                _nextLevelPanel.SetActive(false);
            }
            else
            {
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
        }
    }
}