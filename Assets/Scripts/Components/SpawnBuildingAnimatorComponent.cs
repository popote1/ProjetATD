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


        transform.localScale = Vector3.one * SizeFactore;
        transform.position += new Vector3(0, 0, -FallingHeight);
        
        transform.DOMove(_spawnPos, AnimationTime).SetEase(fallingCurve);
        transform.DOScale(Vector3.one, AnimationTime).SetEase(SizeCurve);

    }

    
}
