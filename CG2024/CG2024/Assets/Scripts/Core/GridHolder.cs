using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


namespace Cards
{
    public class GridHolder : MonoBehaviour
    {
        public static GridHolder instance;
        [field: SerializeField] public Transform[] _cells {  get; private set; }
        [field: SerializeField] public Vector2[] movepoints;
        private void Awake()
        {
            instance = this;

            List<Vector2> v2list = new List<Vector2>();
            foreach (Transform tr in instance._cells)
            {
                v2list.Add(new Vector2( tr.position.x,tr.position.z));
            }
            movepoints = v2list.ToArray();
        }

        public static bool CanMoveToPoint(Vector3 point)
        {
            if(instance == null)
                return false;

            Debug.Log("--- " + point);
            Vector2 pointv2 = new Vector2(point.x, point.z); 
            

            foreach(Vector2 tr in instance.movepoints)
            {
                if(Vector2.Distance (pointv2, tr)<0.25f)
                    return true;
            }
            
            return false;
        }
    }
}