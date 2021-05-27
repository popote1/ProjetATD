using System.Collections.Generic;
using Bourg.Achetable.Tours;
using Enemies;
using UnityEngine;
using Components;
// ReSharper disable All

namespace Assets.Scripts.Bourg.Achetable.Tours
{
    public class TourMage : Tower
    {
        [Header("Auto")]
        public int AutoMagicDamages;
        public float AutoFireRate;
        public int AutoRange;
        public CircleCollider2D AutoCollider2D;
        public AutoAttackProjectile Projectile;


        [Header("Active")]
        public int ActiveRange;
        public int ActiveMagicDamages;
        public float ActiveRate;
        public float ActiveSize = 1;
        public bool IsReadyToAttack = true;
        public int AddForceRange;
        public int AddForcePower;

        [Header("Pasif")]
        public float PassifRange = 5;
        public List<Maison> InRangeMaisons=new List<Maison>();
        
        [Header("Utilities")]
        public int SpawnScore;
        public AudioSource AudioSource;
        //TEMP
        public LineRenderer LineRenderer;
        public float LaserLiveTime;//

        
        [Header("PowerEffect")]
        //public GameObject PowerEffect;
        //public GameObject VisualizeEffect;
        public float VisualizeEffectSpeed;
        

        private float _autoResetTimer;
        private float _activeResetTimer;
        private bool _isSelected;
        private Vector2 _mousePosition;
        private List<EnemyComponent> enemiesInRange = new List<EnemyComponent>();
        private PowerEffectComponent _powerEffectComponent;

        //Initialisation
        private void Awake()
        {
            Projectile.Setorigin();
        }
        private void Start()
        {
            _camera = Camera.main;
            _powerEffectComponent = PowerEffect1.GetComponent<PowerEffectComponent>();
            SetPowerEffect();
            
            OutLine.SetActive(false);
            VisualizerEffect.SetActive(false);
            if (AutoCollider2D.radius != AutoRange) AutoCollider2D.radius = AutoRange;
           /* Collider2D[] affected = new Collider2D[50];
            Physics2D.OverlapCircle(transform.position, PassifRange, new ContactFilter2D().NoFilter(), affected);
            foreach (Collider2D col in affected)
            {
                if (col == null) continue;
                if (col.gameObject.CompareTag("Maison"))
                {
                    InRangeMaisons.Add(col.GetComponent<Maison>());
                    col.GetComponent<Maison>().TourMages.Add(this);
                }
            }*/

            //TEMP
            LineRenderer.SetPosition(0,transform.position + transform.forward * -3);
            LineRenderer.enabled=false;//
        }

        private void SetPowerEffect()
        {
            _powerEffectComponent.Damages = ActiveMagicDamages;
            _powerEffectComponent.Rate = ActiveRate;
            _powerEffectComponent.IsMagic = true;
        }
        
        private void Update()
        {
            Auto();
           // Auto();
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
            Debug.Log(enemiesInRange.Count + " ennemy dans la zone");
            if (_autoResetTimer >= AutoFireRate && enemiesInRange.Count > 0)
            {
                Debug.Log("peux faire des autoAttque");
                enemiesInRange.RemoveAll(o => o == null);
                if (enemiesInRange.Count > 0&&!Projectile.gameObject.activeSelf)
                {
                    Projectile.Damages = AutoMagicDamages;
                    Projectile.IsMagic = true;
                    Projectile.Target = enemiesInRange[0].transform;
                    Projectile.EnnemyTarget = enemiesInRange[0];
                    Projectile.gameObject.SetActive(true);
                    /*LineRenderer.enabled = true;
                    LineRenderer.SetPosition(1,enemiesInRange[0].transform.position);
                    
                    enemiesInRange[0].TakePhysicDamages(AutoPhysicDamages);
                    */
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
           
            //TODO: Check in PM closest selected tower to shoot with
            IsReadyToAttack = true;
    
            //float Dist = Vector2.Distance(this.Position, GetMousePos());
            //if (!(Dist <= ActiveRange)) origin = origin.normalized * ActiveRange;
            //_activeResetTimer = ActiveRate;
    
            PowerEffect1.transform.position = VisualizerEffect.transform.position;
            PowerEffect1.transform.localScale = Vector3.one*ActiveSize;
            PowerEffect1.GetComponent<PowerEffectComponent>().OnAwake();
            PowerEffect1.SetActive(true);
            ShakeComponent.DoShake(2);
    /*
            Collider2D[] affected = new Collider2D[50];
            
            Physics2D.OverlapCircle(VisualizerEffect.transform.position, ActiveRange, new ContactFilter2D().NoFilter(), affected);
            foreach (Collider2D col in affected) {
                if (col == null) continue;
                if (col.transform.CompareTag("Enemy")) {
                    EnemyComponent enemy = col.GetComponent<EnemyComponent>();
                    enemy.TakeMagicDamages(ActiveMagicDamages);
                }
                if (col.transform.CompareTag("Tree")) {
                    Destroy(col.gameObject);
                }
            }
    
            //AddForce on attack
            Physics2D.OverlapCircle(VisualizerEffect.transform.position, AddForceRange, new ContactFilter2D().NoFilter(), affected);
            foreach (Collider2D col in affected)
            {
                if (col == null) continue;
                if (col.transform.CompareTag("Enemy")) {
                    EnemyComponent enemy = col.GetComponent<EnemyComponent>();
                    if (enemy.CanGetPushed) {
                        col.GetComponent<Rigidbody2D>().AddForce((new Vector2(col.transform.position.x, col.transform.position.y) - (Vector2)(VisualizerEffect.transform.position))
                            .normalized * AddForcePower, ForceMode2D.Impulse);
                    }
                }
            }
*/
            ActiveTimer = 0;
            OnDeselect();
            
        }
        
        
        //Visualize Active Power Before throw
        public override void Visualize()
        {
            VisualizerEffect.SetActive(true);
            _mousePosition = GetMousePos();
            Vector3 pos =  _mousePosition-(Vector2)transform.position ;
            VisualizerEffect.transform.localScale = Vector3.one*ActiveSize;
            if (pos.magnitude > ActiveRange)
            {
                VisualizerEffect.transform.position = pos.normalized * ActiveRange + transform.position+new Vector3(0,0,-0.5f);
            }
            else{VisualizerEffect.transform.position = (Vector3)_mousePosition+new Vector3(0,0,-0.5f);}
            //float Dist = Vector2.Distance(_mousePosition, transform.position);
            //if (!(Dist <= ActiveRange)) _mousePosition = _mousePosition.normalized * ActiveRange;
            //VisualizeEffect.transform.position = Vector2.Lerp(VisualizeEffect.transform.position, _mousePosition, VisualizeEffectSpeed * Time.deltaTime);
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

        public override void OnDestroy()
        {
            foreach (var maison in InRangeMaisons)
            {
                maison.TourMages.Remove(this);
            }
            base.OnDestroy();
        }
    }
}