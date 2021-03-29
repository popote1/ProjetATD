using System.Collections.Generic;
using Assets.Scripts.Bourg.Achetable.Tours;
using Components;
using UnityEngine;

namespace Assets.Scripts.Bourg
{

    public class Maison : Batiment
    {
        [Header("Paramètres Maison")] public float GoldIncome;
        public float GoldRate;
        public List<TourMage> TourMages;
        public float DistanceToMage;
        
        private float _goldTimer;
        private int _nombreDeTourInRange;

        private void Update()
        {
            GenerateGold();
        }

        private void GenerateGold()
        {
            if (_goldTimer <= 0)
            {
                _goldTimer = GoldRate;
                _nombreDeTourInRange = 0;
                foreach (Batiment bat in Pm.Batiments)
                {
                    if (bat is TourMage)
                    {
                        if ((this.Position - bat.Position).magnitude < DistanceToMage)
                        {
                            _nombreDeTourInRange++;
                        }
                    }
                }
                Pm.Gold += Mathf.FloorToInt(GoldIncome * (1 + _nombreDeTourInRange));
            }
            else
            {
                _goldTimer -= Time.deltaTime;
            }
        }
    }
}