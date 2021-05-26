using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CursorAnimator : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public AnimationCurve AnimationCurve  = AnimationCurve.Linear(0,0,1,1);

    [Range(0, 1)]public float Volume; 
    public AudioClip CursorSound;
    private float taille=1;
    private DOTween test;

    public void DoAnimation()
    {
        SpriteRenderer.size = Vector2.one;
        AudioManager.PlaySfx(CursorSound , Volume);
       // DOTween.Pause(this); 
       // DOTween.To(() => taille, x => taille = x, 0.8f, 0.5f).SetEase(AnimationCurve).SetLoops(100, LoopType.Yoyo);
    }
    void Start() { DOTween.To(() => taille, x => taille = x, 0.8f, 0.5f).SetEase(AnimationCurve).SetLoops(1000, LoopType.Yoyo); }
    void Update() {SpriteRenderer.size = Vector2.one * taille; }
    
}
