using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Bourg.Achetable
{

    public class Achetables : Batiment
    {
        [Header("Achetable")]
        public int Prix;
        [Range(0,100)]public float RetrunMoney = 75;
        public GameObject OutLine;
        public Action OnSelected ;
        public Action OnDeselected;

        [HideInInspector]public Camera _camera ;
        
        public virtual void OnSelect(){if(OnSelected!=null)OnSelected.Invoke();}
        public virtual void OnDeselect(){if(OnDeselected!=null)OnDeselected.Invoke();}

        public Vector2 GetMousePos()
        {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit)) return hit.point;
            return Vector2.zero;
        }
        
    }
}