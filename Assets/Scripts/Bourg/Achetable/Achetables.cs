using UnityEngine;

namespace Assets.Scripts.Bourg.Achetable
{

    public class Achetables : Batiment
    {
        [Header("Achetable")]
        public int Prix;
        public GameObject OutLine;

        public virtual void OnSelect(){}
        public virtual void OnDeselect(){}

    }
}