using System.Collections.Generic;
using Bourg.Achetable.Tours;
using Enemies;
using UnityEngine;
// ReSharper disable All

namespace Assets.Scripts.Bourg.Achetable.Tours
{
    public class TourMage : Achetables
    {
        [Header("Auto")]
        public int AutoMagicDamages;
        public float AutoFireRate;
        public int AutoRange;
        public CircleCollider2D AutoCollider2D;


        [Header("Active")]
        public int ActiveRange;
        public int ActiveMagicDamages;
        public float ActiveRate;
        public bool IsReadyToAttack = true;
        public int AddForceRange;
        public int AddForcePower;
    
        
        [Header("Utilities")]
        public int SpawnScore;
        public AudioSource AudioSource;
        //TEMP
        public LineRenderer LineRenderer;
        public float LaserLiveTime;//

        
        [Header("PowerEffect")]
        public GameObject PowerEffect;
        public GameObject VisualizeEffect;
        public float VisualizeEffectSpeed;
        

        private float _autoResetTimer;
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
            
            OutLine.SetActive(false);
            VisualizeEffect.SetActive(false);
            _isSelected = false;
            if (AutoCollider2D.radius != AutoRange) AutoCollider2D.radius = AutoRange;
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
           // Auto();
            if (_isSelected)
            {
                OutLine.SetActive(true);
                //Visualize();
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
                    
                    enemiesInRange[0].TakeMagicDamages(AutoMagicDamages);
                    
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

                float Dist = Vector2.Distance(this.Position, origin);
                if (!(Dist <= ActiveRange)) origin = origin.normalized * ActiveRange;
                _activeResetTimer = ActiveRate;

                PowerEffect.transform.position = VisualizeEffect.transform.position;
                PowerEffect.GetComponent<PowerEffectComponent>().OnAwake();
                PowerEffect.SetActive(true);

                Collider2D[] affected = new Collider2D[50];
                
                Physics2D.OverlapCircle(VisualizeEffect.transform.position, ActiveRange, new ContactFilter2D().NoFilter(), affected);
                foreach (Collider2D col in affected)
                {
                    if (col == null) continue;
                    if (col.transform.CompareTag("Enemy"))
                    {
                        EnemyComponent enemy = col.GetComponent<EnemyComponent>();
                        enemy.TakeMagicDamages(ActiveMagicDamages);
                    }

                    if (col.transform.CompareTag("Tree"))
                    {
                        Destroy(col.gameObject);
                    }
                }

                //AddForce on attack
                Physics2D.OverlapCircle(origin, AddForceRange, new ContactFilter2D().NoFilter(), affected);
                foreach (Collider2D col in affected)
                {
                    if (col == null) continue;
                    if (col.transform.CompareTag("Enemy"))
                    {
                        EnemyComponent enemy = col.GetComponent<EnemyComponent>();
                        if (enemy.CanGetPushed)
                        {
                            col.GetComponent<Rigidbody2D>().AddForce((new Vector2(col.transform.position.x, col.transform.position.y) - origin)
                                .normalized * AddForcePower, ForceMode2D.Impulse);
                        }
                    }
                }
            }
            
            else
            {
                _activeResetTimer -= Time.deltaTime;
                IsReadyToAttack = false;
            }
        }
        
        
        //Visualize Active Power Before throw
        public  void Visualize(Vector2 pos)
        {
            VisualizeEffect.SetActive(true);
            _mousePosition = pos; 
                //Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //float Dist = Vector2.Distance(_mousePosition, transform.position);
            //if (!(Dist <= ActiveRange)) _mousePosition = _mousePosition.normalized * ActiveRange;
            //VisualizeEffect.transform.position = Vector2.Lerp(VisualizeEffect.transform.position, _mousePosition, VisualizeEffectSpeed * Time.deltaTime);
            VisualizeEffect.transform.position = pos;
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
        }
        public override void OnDeselect()
        {
            _isSelected = false;
        }

    }
}