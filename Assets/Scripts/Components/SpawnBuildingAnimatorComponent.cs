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

    [Range(0, 1)] public float Volume = 0.5f;
    public AudioClip Buildingsound;
    public float SoundDelay = 0.25f;

    private Vector3 _spawnPos;
   
    void Start()
    {
        _spawnPos = transform.position;
        
        transform.localScale = new Vector3(1,1,SizeFactore);
        transform.position += new Vector3(0, 0, -FallingHeight);
        
        transform.DOMove(_spawnPos, AnimationTime).SetEase(fallingCurve);
        transform.DOScaleZ(1, AnimationTime).SetEase(SizeCurve);
        Invoke("DoShake",0.28f);
        Invoke("PlaySound" , SoundDelay);
    }

    private void DoShake()
    {
        ShakeComponent.DoShake(1);
    }

    private void PlaySound()
    {
        AudioManager.PlaySfx(Buildingsound, Volume);
    }
    
    

    
}
