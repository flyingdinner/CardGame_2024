using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class PlayerAnimator : MonoBehaviour
    {

        public void PlayDead()
        {
            transform.localScale = Vector3.one * 0.3f;
        }

        public void PlayAlive()
        {
            transform.localScale = Vector3.one;
        }


        void Start()
        {

        }

        void Update()
        {

        }
    }
}