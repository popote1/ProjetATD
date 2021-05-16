using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Bourg.Achetable;
using Components;
using UnityEngine;

public abstract class Tower :Achetables
{
    [Header("Tower")]
    public static bool IsUsingPower ;
    public bool IsPowerAvtivated;
    
    
    public GameObject PowerEffect1;
    public GameObject VisualizerEffect;

    public float ActiveCouldown;
    public float ActiveTimer;

    public bool IsReadyToFire {
        get {
            if (ActiveCouldown == ActiveTimer) return true;
            return false;
        }
    }

    public void UIActivePower()
    {
        IsPowerAvtivated = true;
        IsUsingPower = true;
    }

    public void UICancelPower()
    {
        IsPowerAvtivated = false;
        IsUsingPower = false;
        PowerEffect1.SetActive(false);
        VisualizerEffect.SetActive(false);
    }
    public abstract void Visualize();
    public abstract void Active();


    public void Update()
    {
        if (IsPowerAvtivated) {
            Visualize();
            if (Input.GetButtonUp("Fire1")&&!PlayerManagerComponent.CursorOnUI) {
                Active();
                IsPowerAvtivated = false;
                IsUsingPower = false;
            }
        }
    }
}
