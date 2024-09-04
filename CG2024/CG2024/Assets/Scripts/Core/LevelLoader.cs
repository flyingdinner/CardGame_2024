using System;
using UnityEngine;

namespace Cards
{
    public class LevelLoader : MonoBehaviour
    {
        public event Action<LevelGO> OnLevelLoaded;

        [field: SerializeField] public LevelGO currentLevel { get; private set; }

        [SerializeField] private LevelCollectionSO collection;
        [SerializeField] private PlayerService _playerService;

        public void LoadLevel(int index)
        {
            if (currentLevel != null)
            {
                RemoveOldLevel();
            }

            GameObject go = Instantiate(collection.levelsGO[index]);
            LevelGO lgo = go.GetComponent<LevelGO>();
            currentLevel = lgo;

            _playerService.OnLevelLoaded(lgo);

            OnLevelLoaded?.Invoke(lgo);
        }

        private void RemoveOldLevel()
        {

        }
    }
}