using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class RandomSpriteStarterComponent : MonoBehaviour
{
    public List<Sprite> Sprites;
    public SpriteRenderer SpriteRenderer;
    public Color DeathColor = Color.red;
    public float LifeTime=10;
    public AnimationCurve FadeCurve= AnimationCurve.Linear(0,0,1,1);
    
    private void Start()
    {
        SpriteRenderer.sprite = Sprites[Random.Range(0, Sprites.Count)];
        SpriteRenderer.color = DeathColor;
        SpriteRenderer.DOFade(0, LifeTime).SetEase(FadeCurve);
        Invoke("Destroy" , LifeTime);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
