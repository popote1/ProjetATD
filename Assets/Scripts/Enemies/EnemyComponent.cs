using System;
using System.Linq;
using Assets.Scripts.Bourg;
using Assets.Scripts.Bourg.Achetable;
using Assets.Scripts.Bourg.Achetable.Tours;
using Bourg.Achetable.Tours;
using PlaneC;
using UnityEngine;
using UnityEngine.Rendering;

namespace Enemies
{
    public class EnemyComponent : MonoBehaviour
    {
        public EnemySO Enemy;
        public int CurrentHp;
        public bool CanGetPushed;
        public  PlayGrid Playgrid;
        public float SpeedMultyplicatot=2;
        public float VelocityToOrrientation = 0.2f;

        public WaveSystemeV2Component WaveSystemeV2Component;
        public Transform _transform;
        public Rigidbody2D _rb2D;
        public CircleCollider2D _col;
        public SpriteRenderer _spriteRenderer;
        public float _attackTimer;
        public Animator _anim;
        public Batiment AttackTarget;
        

        private bool canAttack;
        private bool isBoss;
        private bool isNotSetted = true;


        //Initialisation
        void Start()
        {
            //gameObject.layer = LayerMask.NameToLayer(Enemy.Layer);

            Playgrid ??= GameObject.FindGameObjectWithTag("PlayGrid").GetComponent<PlayGrid>();
 /*           _rb2D = GetComponent<Rigidbody2D>();
            _col = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
*/
            canAttack = true;
        }

        //SetEnemyPrefab
        private void SetEnnemy()
        {
            if (Enemy != null)
            {
                CanGetPushed = Enemy.CanBePushed;
                //_anim.runtimeAnimatorController = Enemy.AnimationsController;
                _spriteRenderer.sprite = Enemy.Sprite;
                _attackTimer = Enemy.AttackSpeed;
                transform.localScale = Enemy.Size/1.5f;
                isBoss = Enemy.IsBoss;
                CurrentHp = Enemy.HP;
                isNotSetted = false;
                _rb2D.simulated = true;

            }
        }

        //Replace w/ BetaUpdate
        void Update()
        {
            Move();
            if (isNotSetted)
            {
                SetEnnemy();
            }
            if (AttackTarget!= null)AttaqueLoop();
        }

        //EnemyMove
        private void Move()
        {
            _transform = transform;
            Vector2Int intPosition = new Vector2Int((int)_transform.position.x, (int)_transform.position.y);
        
            Vector2 moveVector= Playgrid.GetCell(intPosition).MoveVector;
            float drag = Playgrid.GetCell(intPosition).DragFactor;
            _rb2D.AddForce(moveVector * Enemy.Speed*Time.deltaTime*SpeedMultyplicatot, ForceMode2D.Force);
            _rb2D.drag = drag;
            _anim.SetBool("Walking", true);
            if (_rb2D.velocity.magnitude>VelocityToOrrientation) transform.up = _rb2D.velocity.normalized;
        }

        //EnemyAttack
      /* private void OnCollisionStay2D(Collision2D collision)
        {
           // Debug.Log("Ennemy Collide");
            if (collision.gameObject.layer == LayerMask.NameToLayer("Batiment"))
            {
                Debug.Log("Ennemy Collide to a building");
                if (canAttack)
                {
                    Debug.Log("Ennemy Attack");
                    _anim.SetBool("Walking", false);
                    _anim.SetBool("Attacking", true);
                    if (Enemy.IsMagic)
                    {
                        collision.gameObject.GetComponent<Batiment>().TakeMagicDamages(Enemy.Damages);
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Batiment>().TakePhysicDamages(Enemy.Damages);
                    }
                    canAttack = false;
                }
                else {
                    _anim.SetBool("Attacking", false);
                    if (_attackTimer > 0) {
                        _attackTimer -= Time.deltaTime;
                    }
                    else {
                        _attackTimer = Enemy.AttackSpeed;
                        canAttack = true;
                    }
                }
            }
        }*/

      private void OnCollisionEnter2D(Collision2D other)
      {
          if (other.gameObject.GetComponent<Maison>())  AttackTarget = other.gameObject.GetComponent<Maison>(); 
          if (other.gameObject.GetComponent<Murs>())  AttackTarget = other.gameObject.GetComponent<Murs>();
          if (other.gameObject.GetComponent<TourAlchi>())  AttackTarget = other.gameObject.GetComponent<TourAlchi>();
          if (other.gameObject.GetComponent<TourGarde>())  AttackTarget = other.gameObject.GetComponent<TourGarde>();
          if (other.gameObject.GetComponent<TourMage>())  AttackTarget = other.gameObject.GetComponent<TourMage>();
          if (other.gameObject.GetComponent<TourSainte>())  AttackTarget = other.gameObject.GetComponent<TourSainte>();
          if (other.gameObject.GetComponent<Mairie>())  AttackTarget = other.gameObject.GetComponent<Mairie>();
      }

      private void AttaqueLoop()
      {
          if (_attackTimer >= Enemy.AttackSpeed)
          {
              if ((AttackTarget.transform.position - transform.position).magnitude > 2.5)
              {
                  AttackTarget = null;
                  _attackTimer = Enemy.AttackSpeed;
                  _anim.SetBool("Walking", true);
                  _anim.SetBool("Attacking", false);
                  Batiment newBat = GetOtherBuilding();
                  if (newBat != null) AttackTarget = newBat;
              }
              if (AttackTarget != null)
              {
                  transform.up = (Vector2)(AttackTarget.transform.position - transform.position).normalized;
                  _anim.SetBool("Walking", false);
                  _anim.SetBool("Attacking", true);
                  if (Enemy.IsMagic)AttackTarget.TakeMagicDamages(Enemy.Damages);
                  else AttackTarget.TakePhysicDamages(Enemy.Damages);
                  _attackTimer = 0;
              }
          }
          else if (_attackTimer <= Enemy.AttackSpeed/2)
          {
              _anim.SetBool("Walking", true);
              _anim.SetBool("Attacking", false);
          }
          else transform.up = (Vector2)(AttackTarget.transform.position - transform.position).normalized;
          _attackTimer +=Time.deltaTime;
      }

      private Batiment GetOtherBuilding()
      {
          ContactFilter2D co = new ContactFilter2D();
          co.layerMask = LayerMask.GetMask("Batiment");
          Collider2D[] cols = new Collider2D[50];
          Physics2D.OverlapCircle(transform.position, 2, co, cols);
          if (cols.Length > 0&&cols.Length <50)
          {
              Collider2D FirstOfDefault = cols.OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
                  .FirstOrDefault();
              if (FirstOfDefault.GetComponent<Maison>()) return FirstOfDefault.GetComponent<Maison>(); 
              if (FirstOfDefault.GetComponent<Murs>()) return FirstOfDefault.GetComponent<Murs>();
              if (FirstOfDefault.GetComponent<TourAlchi>()) return FirstOfDefault.GetComponent<TourAlchi>();
              if (FirstOfDefault.GetComponent<TourGarde>()) return FirstOfDefault.GetComponent<TourGarde>();
              if (FirstOfDefault.GetComponent<TourMage>()) return FirstOfDefault.GetComponent<TourMage>();
              if (FirstOfDefault.GetComponent<TourSainte>()) return FirstOfDefault.GetComponent<TourSainte>();
              if (FirstOfDefault.GetComponent<Mairie>()) return FirstOfDefault.GetComponent<Mairie>();
          }
          return null;
    }


      //PhysicDamages
        public void TakePhysicDamages(int damages)
        {
            CurrentHp -= (damages - Enemy.PhysicResistance);
            if (CurrentHp <= 0) Die();
            
        }
    
        //MagicDamages
        public void TakeMagicDamages(int damages)
        {
            CurrentHp -= (damages - Enemy.MagicResistance);
            if (CurrentHp <= 0) Die();
            
        }

        private void Die()
        {
            GameObject deathInstance = Instantiate(Enemy.PrefabMorts, transform.position, transform.rotation);
            deathInstance.transform.localScale = transform.localScale;
            if (WaveSystemeV2Component!=null)WaveSystemeV2Component.EnnemiDie(this);
            Destroy(gameObject);
        }
    }
}
