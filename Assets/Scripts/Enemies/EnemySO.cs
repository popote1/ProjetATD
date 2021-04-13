using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("Variables")]
    public float Speed;
    public int HP;
    public int Damages;
    public float AttackSpeed;
    public Vector3 Size;
    public bool IsBoss;
    public int MagicResistance;
    public int PhysicResistance;
    public GameObject PrefabMorts;
    public string Layer;
    public bool CanBePushed;
    public bool IsMagic;

    [Header("Graphics")]
    public Sprite Sprite;
    public AnimatorController AnimationsController;

}
