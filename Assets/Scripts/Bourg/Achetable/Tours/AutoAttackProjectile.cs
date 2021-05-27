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
    private Vector3 _lastPos;

    public void Setorigin()
    {
        _origin = transform.position;
    }
    private void Start()
    {
        
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
        //Vector3 originWithHeight = new Vector2(_origin.x, _origin.y ,-height);
        //Vector3 targetWithHeight = new Vector2(Target.position.x, Target.position.y + height);
        transform.position = Vector3.Lerp(_origin, Target.position, _traveled)-new Vector3(0,0,height);
        transform.forward = transform.position - _lastPos;
        _lastPos = transform.position;
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
        _lastPos = _origin;
    }
    
}
