using System.Collections.Generic;
using Bourg.Achetable.Tours;
using Enemies;
using UnityEngine;
using Components;
// ReSharper disable All

namespace Assets.Scripts.Bourg.Achetable.Tours
{
    public class TourGarde : Tower
    {
        [Header("Auto")]
        public int AutoPhysicDamages;
        public float AutoFireRate;
        public int AutoRange;
        
        
        [Header("Active")]
        public int ActiveRange;
        public int ActivePhysicDamages;
        public float ActiveRate;
        public bool IsReadyToAttack = true;

        
        [Header("Utilities")]
        public int SpawnScore;
        public AudioSource AudioSource;
        public CircleCollider2D AutoCollider2D;
        //TEMP
        public LineRenderer LineRenderer;
        public float LaserLiveTime;//

        
        [Header("PowerEffect")]
        //public GameObject PowerEffect;
        //public GameObject VisualizerEffect;
        

        private float _autoResetTimer;
        private float _activeResetTimer;
        private bool _isSelected;
        private Vector2 _mousePosition;
        private List<EnemyComponent> enemiesInRange = new List<EnemyComponent>();
        private  PowerEffectComponent _powerEffectComponent;
        

        //Initialisation
        private void Start()
        {
            _camera=Camera.main;
            _powerEffectComponent = PowerEffect1.GetComponent<PowerEffectComponent>();
            SetPowerEffect();
            
            OutLine.SetActive(false);
            VisualizerEffect.SetActive(false);
            _isSelected = false;
            if (AutoCollider2D.radius != AutoRange) AutoCollider2D.radius = AutoRange;
            //TEMP
            LineRenderer.SetPosition(0,transform.position + transform.forward*-3);
            LineRenderer.enabled=false;//
        }
        
        private void SetPowerEffect()
        {
            _powerEffectComponent.Damages = ActivePhysicDamages;
            _powerEffectComponent.Rate = ActiveRate;
            _powerEffectComponent.IsMagic = false;
            //_powerEffectComponent.IsCurved = false;
        }
        
        private void Update()
        {
            Auto();
            if (_isSelected)
            {
                OutLine.SetActive(true);
                //Visualize();

            }
            else
            {
                OutLine.SetActive(false);
                VisualizerEffect.SetActive(false);
            }
            
            if (IsPowerAvtivated) {
                Visualize();
                if (Input.GetButtonUp("Fire1")&&!PlayerManagerComponent.CursorOnUI) {
                    Active();
                    IsPowerAvtivated = false;
                    IsUsingPower = false;
                }
            }

            if (ActiveTimer != ActiveCouldown)
            {
                ActiveTimer += Time.deltaTime;
                if (ActiveTimer > ActiveCouldown) ActiveTimer = ActiveCouldown;
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
        public override void Active()
        {
            if (_activeResetTimer <= 0)
            {            
                //TODO: Check in PM closest selected tower to shoot with
                IsReadyToAttack = true;

                //float Dist = Vector2.Distance(this.Position, origin);
                //if (!(Dist <= ActiveRange)) origin = origin.normalized*ActiveRange;
                //_activeResetTimer = ActiveRate;
                Vector2 pos = transform.position + (VisualizerEffect.transform.up.normalized * ActiveRange);
                PowerEffect1.transform.position=pos;
                PowerEffect1.GetComponent<PowerEffectComponent>().OnAwake();
                PowerEffect1.SetActive(true);
                OnDeselect();
                ActiveTimer = 0;
            }
            
            else
            {
                _activeResetTimer -= Time.deltaTime;
                IsReadyToAttack = false;
            }
        }

        
        //Visualize Active Power Before throw
        public override void Visualize()
        {
            VisualizerEffect.SetActive(true);
            _mousePosition = GetMousePos();
            Debug.Log(GetMousePos() + "  " + (Vector2) transform.position);
                //Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float Dist = Vector2.Distance(_mousePosition, transform.position);
            if (!(Dist <= ActiveRange)) _mousePosition = _mousePosition.normalized*ActiveRange;
                //VisualizerEffect.transform.LookAt(_mousePosition);
                VisualizerEffect.transform.up=( (Vector3)GetMousePos()-transform.position);
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
        
        
        //Outline activator
        public override void OnSelect()
        {
            _isSelected = true;
            base.OnSelect();
        }
        public override void OnDeselect()
        {
            _isSelected = false;
            base.OnDeselect();
        }
    }
}