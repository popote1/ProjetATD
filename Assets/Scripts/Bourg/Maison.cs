using System;
using System.IO;
using System.Collections.Generic;
using Bourg.Achetable.Tours;
using UnityEngine;

namespace Bourg
{

    public class Maison : Batiment
    {
        [Header("Paramètres Maison")] public float GoldIncome;
        public float GoldRate;
        public List<TourMage> TourMages;
        public float DistanceToMage;
        public Vector2 Position;
        
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
                foreach (Batiment bat in GameManager.Batiments)
                {
                    if (bat is TourMage)
                    {
                        if ((Position - bat.Position).magnitude < DistanceToMage)
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