using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{

    public enum PFXType 
    { 
        shoot,
        hit,
        heal,
        dead,
    }

    [Serializable]
    public class PFXSpawnSettings
    {
        public PFXType type;
        public GameObject prefab;
        public float animationTime;
    }

    public class PFXPlayer : MonoBehaviour
    {
        public static PFXPlayer instanse;

        [SerializeField] private PFXSpawnSettings[] _spawnSettings;

        private void Awake()
        {
            instanse = this;
        }

        public void PlayPfx(PFXType type, Transform point)
        {
            foreach(PFXSpawnSettings s in _spawnSettings)
              if(s.type == type)
                {
                    GameObject pfxGO = Instantiate(s.prefab, point.position, Quaternion.identity);
                }    
        }

        public void PlayPfx(PFXType type, Vector3 point)
        {
            foreach (PFXSpawnSettings s in _spawnSettings)
                if (s.type == type)
                {
                    GameObject pfxGO = Instantiate(s.prefab, point, Quaternion.identity);
                }
        }
    }
}