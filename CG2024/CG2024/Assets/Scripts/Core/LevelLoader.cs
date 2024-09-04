using System;
using UnityEngine;

namespace Cards
{
    public class LevelLoader : MonoBehaviour
    {
        public event Action<LevelGO> OnLevelLoaded;

        [SerializeField] private LevelCollectionSO collection;
        [field: SerializeField] public LevelGO currentLevel { get; private set; }

        public void LoadLevel(int index)
        {
            if (currentLevel != null)
            {
                RemoveOldLevel();
            }

            GameObject go = Instantiate(collection.levelsGO[index]);
            LevelGO lgo = go.GetComponent<LevelGO>();
            currentLevel = lgo;

            OnLevelLoaded?.Invoke(lgo);
        }

        private void RemoveOldLevel()
        {

        }
    }
}