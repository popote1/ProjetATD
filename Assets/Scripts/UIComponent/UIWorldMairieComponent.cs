using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Bourg;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldMairieComponent : MonoBehaviour
{
    public Image ImgFillHealth;
    public Slider SliderHealth;
    public Gradient HPColor;

    public Batiment Batiment;
    
    public bool IsUpdateInpdate=true;

    public void UpdateUI()
    {
        if ((float) Batiment.CurrentHp / Batiment.Hp == 1) SliderHealth.gameObject.SetActive(false);
        else SliderHealth.gameObject.SetActive(true);

        SliderHealth.value = (float) Batiment.CurrentHp / Batiment.Hp;
        ImgFillHealth.color = HPColor.Evaluate((float) Batiment.CurrentHp / Batiment.Hp);
    }

    private void Update()
    {
        if (IsUpdateInpdate)UpdateUI();
    }
}
