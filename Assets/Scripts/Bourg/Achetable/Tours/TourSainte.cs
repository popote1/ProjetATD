using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Bourg.Achetable.Tours
{
    public class TourSainte : Achetables
    {
        [Header("Auto")]
        public int AutoRange;
        public int AutoMagicDamages;
        public int AutoFireRate;
        public CircleCollider2D AutoCollider2D;

        
        [Header("Active")]
        public int ActiveRange;
        public int ActiveMagicDamage;
        public float ActiveRate;
        public int AddForcePower;
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

        
        [Header("PowerEffect")]
        public GameObject PowerEffect;
        

        private float _autoResetTimer;
        private float _passiveTimer;
        private float _activeResetTimer;
        private List<MoveActorV2> enemiesInRange = new List<MoveActorV2>();

        //Initialisation
        private void Start()
        {
            if (AutoCollider2D.radius != AutoRange) AutoCollider2D.radius = AutoRange;
            //TEMP
            LineRenderer.SetPosition(0,transform.position + transform.forward*-3);
            LineRenderer.enabled=false;
        }

        private void Update()
        {
            Auto();
        }

        
        //Auto Attack
        private void Auto()
        {
            if (_autoResetTimer >= AutoFireRate && enemiesInRange.Count > 0)
            {
                enemiesInRange.RemoveAll(o => o == null);
                if (enemiesInRange.Count > 0)
                {
                    for (int i = 0; i < enemiesInRange.Count; i++)
                    {
                        //TODO : All EnemiesInRange TakePhysicDamages

                        LineRenderer.enabled = true;
                        LineRenderer.SetPosition(1,enemiesInRange[i].transform.position);
                        _autoResetTimer = 0;
                        AudioSource.Play();
                        Destroy(enemiesInRange[0].gameObject);
                    }
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
                IsReadyToAttack = true;
                
                _activeResetTimer = ActiveRate;

                PowerEffect.transform.position = this.transform.position;
                PowerEffect.SetActive(true);
                
                Collider2D[] affected = new Collider2D[50];
                
                Physics2D.OverlapCircle(origin, ActiveRange, new ContactFilter2D().NoFilter(), affected);
                foreach (Collider2D col in affected)
                {
                    if (col == null) continue;
                    if (!col.transform.CompareTag("MoveActor")) continue;
                    MoveActorV2 enemy = col.GetComponent<MoveActorV2>();
                    //TODO : All EnemiesInRange TakePhysicDamages
                    //AddForce
                    if (enemy.CanBePushed)
                    {
                        enemy.GetComponent<Rigidbody2D>().AddForce(
                            (new Vector2(enemy.transform.position.x, enemy.transform.position.y)-origin).normalized * AddForcePower, ForceMode2D.Impulse);
                    }
                }
            }

            else
            {
                _activeResetTimer -= Time.deltaTime;
                IsReadyToAttack = false;
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
        
        
        //Outline activator
        public void OnSelect()
        {
            OutLine.SetActive(true);
        }
        public void OnDeselect()
        {
            OutLine.SetActive(false);
        }
    }
}