using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArbaletAnimation : MonoBehaviour
{
    public AnimationCurve AnimationCurve;
    public float ReloadTime = 5f;

    public void DoFire()
    {
        transform.DOScaleZ(0, ReloadTime).SetEase(AnimationCurve);
    }
}
