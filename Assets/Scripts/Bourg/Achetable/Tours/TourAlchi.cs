using System.Collections.Generic;
using Bourg.Achetable.Tours;
using Components;
using Enemies;
using UnityEngine;

namespace Assets.Scripts.Bourg.Achetable.Tours
{
    public class TourAlchi : Achetables
    {
        [Header("Auto")]
        public int AutoPhysicDamages;
        public int AutoFireRate;
        public float AutoRange;
        public CircleCollider2D AutoCollider2D;


        [Header("Active")]
        public int ActiveRange;
        public int ActivePhysicDamages;
        public float ActiveRate;
        public bool IsReadyToAttack = true;
        

        [Header("Passive")]
        public int PassiveHpIncome;
        public float PassiveRate;
        public int PassiveRange;
        

        [Header("Utilities")]
        public int SpawnScore;
        public AudioSource AudioSource;
        //TEMP
        public LineRenderer LineRenderer;
        public float LaserLiveTime;//

        
        [Header("PowerEffects")]
        public GameObject PowerEffect;
        public GameObject VisualizeEffect;
        public float VisualizeEffectSpeed;
        
        
        private float _autoResetTimer;
        private float _passiveTimer;
        private float _activeResetTimer;
        private bool _isSelected;
        
        private Vector2 _mousePosition;
        
        private List<EnemyComponent> enemiesInRange = new List<EnemyComponent>();
        private PowerEffectComponent _powerEffectComponent;

        //Initialisation
        private void Start()
        {
            _powerEffectComponent = PowerEffect.GetComponent<PowerEffectComponent>();
            SetPowerEffect();
            
            VisualizeEffect.SetActive(false);
            OutLine.SetActive(false);
            
            _isSelected = false;
            if (AutoCollider2D.radius != AutoRange) AutoCollider2D.radius = AutoRange;
            //TEMP
            LineRenderer.SetPosition(0,transform.position + transform.forward*-3);
            LineRenderer.enabled=false; //
        }

        private void SetPowerEffect()
        {
            _powerEffectComponent.Damages = ActivePhysicDamages;
            _powerEffectComponent.Rate = ActiveRate;
            _powerEffectComponent.IsMagic = false;
            _powerEffectComponent.IsCurved = true;
        }

        private void Update()
        {
            Auto();
            if (_isSelected)
            {
                OutLine.SetActive(true);
               // Visualize();
            }
            else
            {
                OutLine.SetActive(false);
                VisualizeEffect.SetActive(false);
            }
        }

        
        //Auto Attack
        private void Auto()
        {
            if (_autoResetTimer >= AutoFireRate && enemiesInRange.Count > 0)
            {
                enemiesInRange.RemoveAll(o => o == null);
                if (enemiesInRange.Count > 0)
                {
                    LineRenderer.enabled = true;
                    LineRenderer.SetPosition(1,enemiesInRange[0].transform.position);
                    
                    enemiesInRange[0].TakePhysicDamages(AutoPhysicDamages);
                    
                    _autoResetTimer = 0;
                    AudioSource.Play();
                }
            }

            if (_autoResetTimer > LaserLiveTime && LineRenderer.enabled)
            {
                LineRenderer.enabled = false;
            }

            if (_autoResetTimer < AutoFireRate)
            {
                _autoResetTimer += Time.deltaTime;
            }
        }
        
        //Active Power
        public void Active(Vector2 origin)
        {
            if (_activeResetTimer <= 0)
            {
                //TODO: Check in PM closest selected tower to shoot with
                IsReadyToAttack = true;
                PowerEffect.transform.position = this.transform.position;
                _powerEffectComponent.Target = VisualizeEffect.transform.position;
                PowerEffect.SetActive(true);
            }
            
            else
            {
                _activeResetTimer -= Time.deltaTime;
                IsReadyToAttack = false;
            }
        }
        
        
        //Visualize Active Power Before throw
       public void Visualize( Vector2 target)
        {
            VisualizeEffect.SetActive(true);
            _mousePosition = target; 
                //Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float Dist = Vector2.Distance(_mousePosition, transform.position);
            if (!(Dist <= ActiveRange)) _mousePosition = _mousePosition.normalized * ActiveRange;
            VisualizeEffect.transform.position = Vector2.Lerp(VisualizeEffect.transform.position, _mousePosition, VisualizeEffectSpeed * Time.deltaTime);
        }
        

        //Passive Power
        private void Passive()
        {
            if (_passiveTimer <= 0)
            {
                _passiveTimer = PassiveRate;
                foreach (Batiment bat in PlayerManagerComponent.Batiments)
                {
                    if (!((Position - bat.Position).magnitude < PassiveRange)) continue;
                    if (bat.CurrentHp < bat.Hp)
                    {
                        bat.Hp += PassiveHpIncome;
                    }
                }
            }
            else
            {
                _passiveTimer -= Time.deltaTime;
            }
        }
        
        
        //Add enemies in range
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<EnemyComponent>() == null) return;
            if (!enemiesInRange.Contains(other.GetComponent<EnemyComponent>()))
                enemiesInRange.Add( other.GetComponent<EnemyComponent>());
        }
        //Remove enemies out of range
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<EnemyComponent>() == null) return;
            if (enemiesInRange.Contains(other.GetComponent<EnemyComponent>()))
                enemiesInRange.Remove( other.GetComponent<EnemyComponent>());
        }
        

        //Tower activator
        public override void OnSelect()
        {
            _isSelected = true;
        }
        public override void OnDeselect()
        {
            _isSelected = false;
        }
    }
}