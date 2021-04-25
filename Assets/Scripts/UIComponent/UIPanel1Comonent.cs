using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public  class UIPanel1Comonent : UIElementComponent
{

    [Header(" OnActication")] 
    
    public float ActivationDuration;
    public AnimationCurve ActivationCurve;
    public bool AutoSetActivationpos;
    public Vector3 ActivatePose;

    [Header("OnDesactivation")] 
    public float DesactivationDuration;
    public AnimationCurve DesactivationCurve;
    public Vector3 DesactivatePose;
    private RectTransform pos;
    private void Start()
    {
        if (AutoSetActivationpos) ActivatePose = transform.position;
    }


    public override void Activate()
    {
       /// transform.position = DesactivatePose;
       // transform.DOMove(ActivatePose, ActivationDuration).SetEase(ActivationCurve);
        pos = GetComponent<RectTransform>();
        pos.DOPivot(new Vector2(0.5f,100), ActivationDuration);
    }

    public override void Desactivat()
    {
        //transform.position = ActivatePose;
        //transform.DOMove(DesactivatePose, DesactivationDuration).SetEase(DesactivationCurve);
        pos.DOPivot(new Vector2(0.5f,0.5f), ActivationDuration);
    }
}
