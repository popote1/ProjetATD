using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Bourg;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIWorldHousesComponent : MonoBehaviour
{
    public Image ImgFillHealth;
    public Slider SliderHealth;
    public Gradient HPColor;

    public Maison House;
    
    public bool IsUpdateInpdate=true;

    [Header("Animation")]
    public TMP_Text TxtGoldText;
    public CanvasGroup CanvasGroupGoldText;

    public float AnimationDuration;
    public Vector3 StatPos;
    public Vector3 EndPos;
    public Vector3 EndSize;
    public AnimationCurve FadeCurve;
    
    public 
    // Start is called before the first frame update
    void Start()
    {
        House.OnGoldGeneration += GoldIncomeAniamtion;
    }

    // Update is called once per frame
    void Update()
    {
        if ( IsUpdateInpdate)UpdateUI();
    }

    public void UpdateUI()
    {
        if ((float) House.CurrentHp / House.Hp == 1) SliderHealth.gameObject.SetActive(false);
        else SliderHealth.gameObject.SetActive(true);

        SliderHealth.value = (float) House.CurrentHp / House.Hp;
        ImgFillHealth.color = HPColor.Evaluate((float) House.CurrentHp / House.Hp);
    }

    public void GoldIncomeAniamtion()
    {
        TxtGoldText.text = "+"+ Mathf.FloorToInt(House.GoldIncome * (1 + House.TowerInRange));
        TxtGoldText.transform.localPosition = StatPos;
        TxtGoldText.transform.localScale = Vector3.one;
        CanvasGroupGoldText.alpha = 0;
        TxtGoldText.transform.DOLocalMove(EndPos, AnimationDuration);
        TxtGoldText.transform.DOScale(EndSize, AnimationDuration);
        CanvasGroupGoldText.DOFade(1, AnimationDuration).SetEase(FadeCurve);

    }
}
