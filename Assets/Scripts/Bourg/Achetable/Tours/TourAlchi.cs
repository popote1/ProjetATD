using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Bourg.Achetable.Tours
{
    public class TourAlchi : Achetables
    {
        [Header("Auto")]
        public int AutoPhysicDamages;
        public int AutoFireRate;
        public int AutoRange;
        public CircleCollider2D AutoCollider2D;


        [Header("Active")]
        public int ActiveRange;
        public float ActiveRate;
        public bool IsReadyToAttack = true;
        

        [Header("Passive")]
        public int PassiveHpIncome;
        public float PassiveRate;
        public float PassiveRange;
        

        [Header("Utilities")]
        public int SpawnScore;
        public GameObject OutLine;
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
        private List<MoveActorV2> enemiesInRange = new List<MoveActorV2>();

        //Initialisation
        private void Start()
        {
            VisualizeEffect.SetActive(false);
            OutLine.SetActive(false);
            _isSelected = false;
            if (AutoCollider2D.radius != AutoRange) AutoCollider2D.radius = AutoRange;
            //TEMP
            LineRenderer.SetPosition(0,transform.position + transform.forward*-3);
            LineRenderer.enabled=false; //
        }

        private void Update()
        {
            Auto();
            if (_isSelected)
            {
                OutLine.SetActive(true);
                Visualize();
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
                    //TODO : One Enemy TakePhysicDamages

                    LineRenderer.enabled = true;
                    LineRenderer.SetPosition(1,enemiesInRange[0].transform.position);
                    _autoResetTimer = 0;
                    AudioSource.Play();
                    Destroy(enemiesInRange[0].gameObject);
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
                
                PowerEffect.transform.position = origin;
                PowerEffect.SetActive(true);
                //TODO : Enemies TakePhysicDamages OnTriggerStay2D in AcidPowerEffect script
            }
            
            else
            {
                _activeResetTimer -= Time.deltaTime;
                IsReadyToAttack = false;
            }
        }
        
        
        //Visualize Active Power Before throw
        private void Visualize()
        {
            VisualizeEffect.SetActive(true);
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
                foreach (Batiment bat in Pm.Batiments)
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
            if (other.GetComponent<MoveActorV2>() == null) return;
            if (!enemiesInRange.Contains(other.GetComponent<MoveActorV2>()))
                enemiesInRange.Add( other.GetComponent<MoveActorV2>());
        }
        //Remove enemies out of range
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<MoveActorV2>() == null) return;
            if (enemiesInRange.Contains(other.GetComponent<MoveActorV2>()))
                enemiesInRange.Remove( other.GetComponent<MoveActorV2>());
        }
        
        
        //Tower activator
        public void OnSelect()
        {
            _isSelected = true;
        }
        public void OnDeselect()
        {
            _isSelected = false;
        }
    }
}