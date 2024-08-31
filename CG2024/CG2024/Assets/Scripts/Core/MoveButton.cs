using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class MoveButton : Button3D
    {
        public Vector3 point => transform.position;

        [SerializeField]
        private PlayerHuman _player;
        
        public override void OnClick()
        {
            _player.MoveTo(this);
        }
    }
}