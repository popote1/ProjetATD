using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class AutoAttackProjectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve _flightCurve;
    
    public bool IsMagic;
    public int Damages;
    public float Speed;
    public Transform Target;
    public EnemyComponent EnnemyTarget;


    private float _traveled;
    private Vector3 _origin;

    private void Start()
    {
        _origin = transform.position;
    }
    private void Update()
    {
        if (Target == null)
        {
            ResetPosition();
            _traveled = 0;
            gameObject.SetActive(false);
            return;
        }
        _traveled += Time.deltaTime * Speed;
        float height = _flightCurve.Evaluate(_traveled);
        Vector2 originWithHeight = new Vector2(_origin.x, _origin.y + height);
        Vector2 targetWithHeight = new Vector2(Target.position.x, Target.position.y + height);
        transform.position = Vector2.Lerp(originWithHeight, targetWithHeight, _traveled);
        if (_traveled >= 1)
        {
            _traveled = 0;
            DoDamages(EnnemyTarget);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EnemyComponent enemy = col.gameObject.GetComponent<EnemyComponent>();
            DoDamages(enemy);
        }
    }*/
    
    private void DoDamages(EnemyComponent enemy)
    {
        if (IsMagic) enemy.TakeMagicDamages(Damages);
        else enemy.TakePhysicDamages(Damages);
        
        this.gameObject.SetActive(false);
        ResetPosition();
    }

    private void ResetPosition()
    {
        transform.position = _origin;
    }
    
}
