using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UITutoPanelComponent : UIElementComponent
{
    [Header("Onglet Parameteres")]
    public List<UIPanelSimpletScaleComponent> BPOnglet;
    public List<UIPanelSimpletScaleComponent> PanelBP;

    public RectTransform InfoPanelBackGround;
    public float MaxY;
    public float MinY;

    public List<GameObject> InfoPanel;
    
    [Header("Activation Parameteres")]
    public float MainActivationTime;
    public AnimationCurve MainActivationCurve = AnimationCurve.Linear(0,0,1,1);
    [Header("Desactivation Parameteres")] 
    public float MainDesactivationTime;
    public AnimationCurve MainDesactivationCurve= AnimationCurve.Linear(0,0,1,1);
    public Vector3 DesactivateRotation;


    private int _ongletIndex = 1;
    private int _panelIndex = 1;
    

    void Start()
    {
        ChangeOnglet(0);
        ChangerInfo(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangerInfo(int index)
    {
        InfoPanel[_panelIndex].SetActive(false);
        _panelIndex = index;
        InfoPanelBackGround.DOAnchorMin(new Vector2(0.02f, MinY), 0.25f).SetUpdate(true).OnComplete(delegate { ChangeInfo2(); });
        //Invoke("ChangeInfo2", 0.25f);
    }

    private void ChangeInfo2()
    {
        InfoPanelBackGround.DOAnchorMin(new Vector2(0.02f, MaxY), 0.25f).OnComplete(delegate {  InfoPanel[_panelIndex].SetActive(true);}).SetUpdate(true);
    }
    public void ChangeOnglet(int index)
    {
        if (index != _ongletIndex)
        {
            _ongletIndex = index;
            for (int i = 0; i < BPOnglet.Count; i++) {
                
                if (index == i) {
                    BPOnglet[i].Desactivat();
                    PanelBP[i].Desactivat();
                }
                
                else {
                    BPOnglet[i].Activate();
                    PanelBP[i].Activate();
                }
            }
        }
    }
    
    public override void Activate() {
        transform.DORotate(Vector3.zero, MainActivationTime).SetEase(MainActivationCurve).SetUpdate(true);
        transform.DOScale(Vector3.one, MainActivationTime).SetEase(MainActivationCurve).SetUpdate(true);
    }
    public override void Desactivat() {
        transform.DORotate(DesactivateRotation, MainDesactivationTime).SetEase(MainDesactivationCurve).SetUpdate(true);
        transform.DOScale(Vector3.zero, MainDesactivationTime).SetEase(MainDesactivationCurve).SetUpdate(true);
    }
}
