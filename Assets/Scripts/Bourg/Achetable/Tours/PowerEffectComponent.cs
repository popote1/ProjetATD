
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

        public void OnAwake() {
            _minRate = 0;
            _timer = EffectLifeTime;
            _enemies.Clear();
            if (IsInstante) {
                Collider2D[] test = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x);
                foreach (Collider2D col in test) {
                    if (col.gameObject.layer == LayerMask.NameToLayer("Tree") && DestroyTree) Destroy(col.gameObject);
                    else if (col.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                        if (IsMagic) col.GetComponent<EnemyComponent>().TakeMagicDamages(Damages);
                        else col.GetComponent<EnemyComponent>().TakePhysicDamages(Damages);
                    }
                }
            }
        }
        private void OnTriggerStay2D(Collider2D other) {
            if (!IsInstante) {
                Debug.Log("collide with " + other.gameObject.name);
                if (other.gameObject.layer == LayerMask.NameToLayer("Tree")&&DestroyTree) {
                    Destroy(other.gameObject);
                }
                else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                    EnemyComponent enemy = other.GetComponent<EnemyComponent>();

                    if (enemy == null) return;
                    if (!_enemies.Contains(enemy)) {
                        _enemies.Add(enemy);
                    }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D other) {
            if (!IsInstante) {
                if (other.GetComponent<EnemyComponent>() == null) return;
                _enemies.Remove(other.GetComponent<EnemyComponent>());
            }
        }
        private void Update() {
            if (_timer > 0) {
                _timer -= Time.deltaTime;
                //DoDamages();
            }
            else {
                gameObject.SetActive(false);
            }
            
            _minRate += Time.deltaTime;
            if (_minRate > Rate&&!IsInstante) {
                _minRate = 0;
                DoDamages();
            }
        }
        private void DoDamages() {
            Debug.Log( "Do damage on "+ _enemies.Count+" enemies");
            foreach (EnemyComponent enemy in _enemies) {
                if (IsMagic) enemy.TakeMagicDamages(Damages);
                else enemy.TakePhysicDamages(Damages);
            }
        }
    }
}
