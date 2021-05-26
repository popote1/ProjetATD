using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpawnBuildingAnimatorComponent : MonoBehaviour
{
    public float AnimationTime;
    public float FallingHeight;
    public AnimationCurve fallingCurve;
    public float SizeFactore;
    public AnimationCurve SizeCurve;

    private Vector3 _spawnPos;
   
    void Start()
    {
        _spawnPos = transform.position;
        
        transform.localScale = new Vector3(1,1,SizeFactore);
        transform.position += new Vector3(0, 0, -FallingHeight);
        
        transform.DOMove(_spawnPos, AnimationTime).SetEase(fallingCurve);
        transform.DOScaleZ(1, AnimationTime).SetEase(SizeCurve);
        Invoke("DoShake",0.28f);
    }

    private void DoShake()
    {
        ShakeComponent.DoShake(1);
    }
    
    

    
}
