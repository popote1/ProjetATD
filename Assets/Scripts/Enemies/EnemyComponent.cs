using Assets.Scripts.Bourg;
using PlaneC;
using UnityEngine;

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
                _anim.runtimeAnimatorController = Enemy.AnimationsController;
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
            if (!isBoss)
            {
                Move();
            }

            if (isNotSetted)
            {
                SetEnnemy();
            }
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
        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("Ennemy Collide");
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
                else
                {
                    _anim.SetBool("Attacking", false);

                    if (_attackTimer > 0)
                    {
                        _attackTimer -= Time.deltaTime;
                    }
                    else
                    {
                        _attackTimer = Enemy.AttackSpeed;
                        canAttack = true;
                    }
                }
            }
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
