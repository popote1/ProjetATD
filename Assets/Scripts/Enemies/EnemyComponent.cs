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
    
        private Transform _transform;
        private Rigidbody2D _rb2D;
        private CircleCollider2D _col;
        private SpriteRenderer _spriteRenderer;
        public float _attackTimer;
        private Animator _anim;
        private PlayGrid _playgrid;

        private bool canAttack;
        private bool isBoss;


        //Initialisation
        void Start()
        {
            gameObject.layer = LayerMask.NameToLayer(Enemy.Layer);

            _playgrid ??= GameObject.FindGameObjectWithTag("PlayGrid").GetComponent<PlayGrid>();
            _rb2D = GetComponent<Rigidbody2D>();
            _col = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
            CanGetPushed = Enemy.CanBePushed;
        
            _anim.runtimeAnimatorController = Enemy.AnimationsController;
            _spriteRenderer.sprite = Enemy.Sprite;
            _attackTimer = Enemy.AttackSpeed;
            transform.localScale = Enemy.Size;
            isBoss = Enemy.IsBoss;

            CurrentHp = Enemy.HP;
            canAttack = true;
        }

        //Replace w/ BetaUpdate
        void Update()
        {
            if (!isBoss)
            {
                Move();
            }
        }

        //EnemyMove
        private void Move()
        {
            _transform = transform;
            Vector2Int intPosition = new Vector2Int((int)_transform.position.x, (int)_transform.position.y);
        
            Vector2 moveVector= _playgrid.GetCell(intPosition).MoveVector;
            float drag = _playgrid.GetCell(intPosition).DragFactor;
        
            _rb2D.AddForce(moveVector * Enemy.Speed);
            _rb2D.drag = drag;
            _anim.SetBool("Walking", true);
        }

        //EnemyAttack
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Batiment"))
            {
                if (canAttack)
                {
                    _anim.SetBool("Walking", false);
                    _anim.SetBool("Attacking", true);
                    if (Enemy.IsMagic)
                    {
                        other.GetComponent<Batiment>().TakeMagicDamages(Enemy.Damages);
                    }
                    else
                    {
                        other.GetComponent<Batiment>().TakePhysicDamages(Enemy.Damages);
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
            if (CurrentHp <= 0)
            {
                GameObject deathInstance = Instantiate(Enemy.PrefabMorts, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    
        //MagicDamages
        public void TakeMagicDamages(int damages)
        {
            CurrentHp -= (damages - Enemy.MagicResistance);
            if (CurrentHp <= 0)
            {
                GameObject deathInstance = Instantiate(Enemy.PrefabMorts, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
