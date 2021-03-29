using System.Collections;
using System.Collections.Generic;
using Scripts.Actors;
using UnityEngine;


namespace Bourg.Achetable.Tours
{
    public class TourGarde : Achetables
    {
        [Header("Auto")]
        public int AutoPhysicDamages;
        public int AutoFireRate;
        public int AutoRange;
        
        [Header("Active")]
        public int ActiveRange;
        public int ActivePhysicDamages;
        public int ActiveDamagesZone;
        public float ActiveRate;
        public bool IsReadyToAttack = true;

        [Header("Utilities")]
        public GameObject OutLine;
        public AudioSource AudioSource;
        public CircleCollider2D AutoCollider2D;
        //TEMP
        public LineRenderer LineRenderer;
        //TEMP
        public float LaserLiveTime;

        [Header("PowerEffect")]
        public GameObject PowerEffect;
        
        public int SpawnScore;

        private float _autoResetTimer;
        private float _activeResetTimer;
        private List<MoveActorV2> enemiesInRange = new List<MoveActorV2>();

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

        //Auto
        private void Auto()
        {
            if (_autoResetTimer >= AutoFireRate && enemiesInRange.Count > 0)
            {
                enemiesInRange.RemoveAll(o => o == null);
                if (enemiesInRange.Count > 0)
                {
                    //TODO : One Enemy TakePhysicDamage

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
        
        //Active
        public void Active(Vector2 origin)
        {
            if (_activeResetTimer <= 0)
            {                
                IsReadyToAttack = true;

                float Dist = Vector2.Distance(this.Position, origin);
                if (!(Dist <= ActiveRange)) origin = origin.normalized*ActiveRange;
                _activeResetTimer = ActiveRate;

                PowerEffect.transform.LookAt(origin);
                PowerEffect.SetActive(true);
                //TODO: Enemies TakePhysicDamages OnTriggerEnter of PowerEffectRange & Destroy trees in range
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