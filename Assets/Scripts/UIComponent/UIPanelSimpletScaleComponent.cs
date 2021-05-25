using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIPanelSimpletScaleComponent : UIElementComponent
{

    public float AnimationTime = 0.3f;
    public Vector3 ActiveScale = Vector3.one;
    public Vector3 DesactiveScale  = Vector3.zero;

    public override void Activate() {
        transform.DOScale(DesactiveScale, AnimationTime);
    }

    public override void Desactivat() {
        transform.DOScale(ActiveScale, AnimationTime);
    }
}
