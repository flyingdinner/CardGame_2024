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

        private void OnEnable()
        {
            if (GridHolder.CanMoveToPoint(point))
            {

            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public override void OnClick()
        {
            if (GridHolder.CanMoveToPoint(point))
                _player.MoveTo(this);
        }
    }
}