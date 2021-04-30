using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;

public class UIBigMenuCOmponent : UIElementComponent
{
    public bool StartDesactivated;
    public Vector3 DesactivateRotation;
    public List<GameObject> Buttons;
    [Header("Activation Parameteres")]
    public float MainActivationTime;
    public AnimationCurve MainActivationCurve = AnimationCurve.Linear(0,0,1,1);
    public float DelayBetweenButton;
    public float ButtonActivationTime;
    public AnimationCurve ButtonActivationCurve= AnimationCurve.Linear(0,0,1,1);
    [Header("Desactivation Parameteres")] 
    public float MainDesactivationTime;
    public AnimationCurve MainDesactivationCurve= AnimationCurve.Linear(0,0,1,1);
    public float ButtonDesactivationTime;
    public AnimationCurve ButtonDesactivationCurve= AnimationCurve.Linear(0,0,1,1);

    public void Start()
    {
        if (StartDesactivated)Desactivat();
    }

    public override void Activate()
    {
        transform.DORotate(Vector3.zero, MainActivationTime).SetEase(MainActivationCurve).SetUpdate(true);
        transform.DOScale(Vector3.one, MainActivationTime).SetEase(MainActivationCurve).SetUpdate(true);
        float delay = MainActivationTime;
        foreach (GameObject button in Buttons)
        {
            button.transform.DOPause();
            button.transform.localScale = new Vector3(1,0,1);
            button.transform.DOScaleY(1, ButtonActivationTime).SetEase(ButtonActivationCurve).SetDelay(delay).SetUpdate(true);
            delay += DelayBetweenButton;
        }
    }
    
    
    public override void Desactivat()
    {
        foreach (GameObject button in Buttons) {
            button.transform.DOPause();
            button.transform.DOScaleY(0, ButtonDesactivationTime).SetEase(ButtonDesactivationCurve).SetUpdate(true);
        }
        transform.DORotate(DesactivateRotation, MainDesactivationTime).SetEase(MainDesactivationCurve).SetUpdate(true);
        transform.DOScale(Vector3.zero, MainDesactivationTime).SetEase(MainDesactivationCurve).SetUpdate(true);
    }
}
