using System.Collections.Generic;
using Assets.Scripts.Bourg.Achetable.Tours;
using Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Bourg
{

    public class Maison : Batiment
    {
        [Header("Paramètres Maison")] public float GoldIncome;
        public float GoldRate;
        public List<TourMage> TourMages = new List<TourMage>();
        public float DistanceToMage;
        public Action OnGoldGeneration;
        
        private float _goldTimer;


        public override void OnDestroy()
        {
            foreach (var tour in TourMages)
            {
                tour.InRangeMaisons.Remove(this);
            }
            base.OnDestroy();
        }

        private void Start()
        {
            Collider2D[] affected = new Collider2D[50];
            Physics2D.OverlapCircle(transform.position, DistanceToMage, new ContactFilter2D().NoFilter(), affected);
            foreach (Collider2D col in affected)
            {
                if (col == null) continue;
                if (col.gameObject != null)
                {
                    if (col.gameObject.GetComponent<TourMage>() != null)
                    {
                        if (!TourMages.Contains(col.GetComponent<TourMage>()))
                        {
                            col.GetComponent<TourMage>().InRangeMaisons.Add(this);
                            TourMages.Add(col.GetComponent<TourMage>());
                        }
                    }
                }
            }
        }
        private void Update()
        {
            GenerateGold();
        }

        private void GenerateGold()
        {
            if (_goldTimer <= 0)
            {
                _goldTimer = GoldRate;
                OnGoldGeneration.Invoke();
                PlayerManagerComponent.Gold += Mathf.FloorToInt(GoldIncome * (1 + TourMages.Count));
            }
            else
            {
                _goldTimer -= Time.deltaTime;
            }
        }
        
    }
}