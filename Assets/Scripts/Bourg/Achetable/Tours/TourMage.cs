using System.Collections.Generic;
using Scripts.Actors;
using Scripts.Main;
using UnityEngine;

namespace Bourg.Achetable.Tours
{
    public class TourMage : Achetables
    {
        [Header("Auto")]
        public int AutoMagicDamages;
        public int AutoFireRate;
        public int AutoRange;
        
        [Header("Active")]
        public int ActiveRange;
        public int ActiveMagicDamages;
        public int ActiveDamagesZone;
        public float ActiveRate;
        public bool IsReadyToAttack = true;
        public int AddForceRange;
        public int AddForcePower;
        
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
        public GameObject VisualizeEffect;
        public float VisualizeEffectSpeed;
        
        public int SpawnScore;

        private float _autoResetTimer;
        private float _activeResetTimer;
        private bool _isSelected;
        private List<MoveActorV2> enemiesInRange = new List<MoveActorV2>();

        private void Start()
        {
            OutLine.SetActive(false);
            VisualizeEffect.SetActive(false);
            _isSelected = false;
            if (AutoCollider2D.radius != AutoRange) AutoCollider2D.radius = AutoRange;
            //TEMP
            LineRenderer.SetPosition(0,transform.position + transform.forward * -3);
            LineRenderer.enabled=false;
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

        //Auto
        private void Auto()
        {
            if (_autoResetTimer >= AutoFireRate && enemiesInRange.Count > 0)
            {
                enemiesInRange.RemoveAll(o => o == null);
                if (enemiesInRange.Count > 0)
                {
                    //TODO : Enemy TakeMagicDamage

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
                //TODO: Check in PM closest selected tower to shoot with
                IsReadyToAttack = true;

                float Dist = Vector2.Distance(this.Position, origin);
                if (!(Dist <= ActiveRange)) origin = origin.normalized * ActiveRange;
                _activeResetTimer = ActiveRate;

                PowerEffect.transform.position = origin;
                PowerEffect.SetActive(true);

                Collider2D[] affected = new Collider2D[50];

                //Active Attack
                Physics2D.OverlapCircle(origin, ActiveDamagesZone, new ContactFilter2D().NoFilter(), affected);
                foreach (Collider2D col in affected)
                {
                    if (col == null) continue;
                    if (col.transform.CompareTag("MoveActor"))
                    {
                        //TODO: One Enemy TakeMagicDamage
                    }

                    if (col.transform.CompareTag("Tree"))
                    {
                        Destroy(col.gameObject);
                    }
                }

                //Active Attack AddForce
                Physics2D.OverlapCircle(origin, AddForceRange, new ContactFilter2D().NoFilter(), affected);
                foreach (Collider2D col in affected)
                {
                    if (col == null) continue;
                    if (col.transform.CompareTag("MoveActor"))
                    {
                        col.GetComponent<Rigidbody2D>()
                            .AddForce(
                                (new Vector2(col.transform.position.x, col.transform.position.y) - origin)
                                .normalized * AddForcePower, ForceMode2D.Impulse);
                    }
                }
            }
            
            else
            {
                _activeResetTimer -= Time.deltaTime;
                IsReadyToAttack = false;
            }
        }
        
        //Visualize active
        private void Visualize()
        {
            VisualizeEffect.SetActive(true);
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float Dist = Vector2.Distance(_mousePosition, transform.position);
            if (!(Dist <= ActiveRange)) _mousePosition = _mousePosition.normalized * ActiveRange;
            VisualizeEffect.position = Vector2.Lerp(VisualizeEffect.position, _mousePosition, VisualizeEffectSpeed * Time.deltaTime);
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
            _isSelected = true;
        }
        public void OnDeselect()
        {
            _isSelected = false;
        }

    }
}