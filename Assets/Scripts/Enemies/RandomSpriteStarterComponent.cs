using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class RandomSpriteStarterComponent : MonoBehaviour
{
    public List<Sprite> Sprites;
    public SpriteRenderer SpriteRenderer;

    private void Start()
    {
        SpriteRenderer.sprite = Sprites[Random.Range(0, Sprites.Count)];
    }
}
