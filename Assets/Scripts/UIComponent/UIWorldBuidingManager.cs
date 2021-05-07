using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Bourg;
using Assets.Scripts.Bourg.Achetable;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldBuidingManager : MonoBehaviour
{
   public Image ImgFillHealth;
   public Slider SliderHealth;
   public Gradient HPColor;
   public Button BPDestroy;

   public Batiment Batiment;

   public bool IsUpdateInpdate=true;

   public void Start()
   {
      if (Batiment is Achetables)
      {
         (Batiment as Achetables).OnSelected = SetOnButtons;

         (Batiment as Achetables).OnDeselected = SetOffButton;
      }
   }

   public void Update()
   {
      if ( IsUpdateInpdate)UpdateUI();
   }

   public void SetOnButtons()
   {
      BPDestroy.gameObject.SetActive(true);
      Debug.Log(" Set Up button");
   }
   public void SetOffButton()
   {
      BPDestroy.gameObject.SetActive(false);
   }

   public void UpdateUI()
   {
      if ((float)Batiment.CurrentHp / Batiment.Hp==1) SliderHealth.gameObject.SetActive(false);
      else SliderHealth.gameObject.SetActive(true);
      SliderHealth.value = (float)Batiment.CurrentHp / Batiment.Hp;
      ImgFillHealth.color = HPColor.Evaluate((float)Batiment.CurrentHp / Batiment.Hp);
   }
   
   


}
