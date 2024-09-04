using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cards
{
    public class GridHolder : MonoBehaviour
    {
        public static GridHolder instance;

        [field: SerializeField] private Transform[] _cells;

        private void Awake()
        {
            instance = this;
        }

        public bool CanMoveToPoint(Vector3 point)
        {

            return true;
        }
    }
}