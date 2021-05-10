
using System;
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
        public float EffectLifeTime;
        public bool DestroyTree;
        public bool IsInstante;

        private float _timer;

        private float _minRate;
        
        private List<EnemyComponent> _enemies = new List<EnemyComponent>();

        public void OnAwake()
        {
            _minRate = 0;
            _timer = EffectLifeTime;
            Collider2D[] test =Physics2D.OverlapCircleAll(transform.position,transform.localScale.x);
            foreach (Collider2D col in test)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Tree")&&DestroyTree) Destroy(col.gameObject);
                else if (col.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                    if (IsMagic) col.GetComponent<EnemyComponent>().TakeMagicDamages(Damages); 
                    else col.GetComponent<EnemyComponent>().TakePhysicDamages(Damages);
                }
            }
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!IsInstante)
            {
                Debug.Log("collide with " + other.gameObject.name);
                if (other.gameObject.layer == LayerMask.NameToLayer("Tree")&&DestroyTree)
                {
                    Destroy(other.gameObject);
                }
                else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyComponent enemy = other.GetComponent<EnemyComponent>();

                    if (enemy == null) return;
                    if (!_enemies.Contains(enemy))
                    {
                        _enemies.Add(enemy);
                    }
                }
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsInstante)
            {
                if (other.GetComponent<EnemyComponent>() == null) return;
                _enemies.Remove(other.GetComponent<EnemyComponent>());
            }
        }


        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
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
