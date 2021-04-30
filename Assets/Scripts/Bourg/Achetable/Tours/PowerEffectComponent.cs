
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Bourg.Achetable.Tours
{
    public class PowerEffectComponent : MonoBehaviour
    {
        public bool IsMagic;
        public int Damages;
        public float Rate;
        public float FxTimer;

        private float _minRate;
        
        private List<EnemyComponent> _enemies = new List<EnemyComponent>();

        private void OnAwake()
        {
            _minRate = 0;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            EnemyComponent enemy = other.GetComponent<EnemyComponent>();

            if (enemy == null) return;
            if (!_enemies.Contains(enemy))
            {
                _enemies.Add(enemy);
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<EnemyComponent>() == null) return;
            _enemies.Remove(other.GetComponent<EnemyComponent>());
        }


        private void Update()
        {
            if (FxTimer > 0)
            {
                FxTimer -= Time.deltaTime;
                DoDamages();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void DoDamages()
        {
            if (_minRate < Rate)
            {
                if (_enemies == null) return;
                if (!IsMagic)
                {
                    foreach (EnemyComponent enemy in _enemies)
                    {
                        enemy.TakePhysicDamages(Damages);
                    }
                }
                else
                {
                    foreach (EnemyComponent enemy in _enemies)
                    {
                        enemy.TakeMagicDamages(Damages);
                    }
                }

                _minRate += Time.deltaTime;
            }
            else
            {
                _minRate = 0f;
            }
        }
    }
}
