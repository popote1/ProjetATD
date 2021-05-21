using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    public List<PlanParalax> Plans;

    public float MoveFactor;
    public Vector2 MousPosNormaliz;
    private Vector3[] _originalPos;

    public void Start()
    {
        _originalPos = new Vector3[Plans.Count];

        for (int i = 0; i < Plans.Count; i++) {
            _originalPos [i] = Plans[i].ObjectTransform.localPosition;
        }
    }

    public void Update()
    {
        MousPosNormaliz =new Vector2(
            (Input.mousePosition.x/Screen.width-0.5f)*2f,
            (Input.mousePosition.y/Screen.height-0.5f)*2f
            );
        for (int i = 0; i < Plans.Count; i++)
        {
            Plans[i].ObjectTransform.localPosition =
                (Vector2)_originalPos[i] + Vector2.one * Plans[i].ParalaxFactor * MoveFactor * MousPosNormaliz;
        }
        
    }
}
[Serializable]
public struct PlanParalax
{
    public Transform ObjectTransform;
    [Range(-1, 1)] public float ParalaxFactor ;
    public PlanParalax(Transform objectTransform, float paralaxFactor) : this()
    {
        ObjectTransform = objectTransform;
        ParalaxFactor = paralaxFactor;
    }
    
}
