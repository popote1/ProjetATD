using System.Collections.Generic;
using Assets.Scripts.Bourg;
using Assets.Scripts.Bourg.Achetable;
using Components;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldWallComponent : MonoBehaviour
{
   public Image ImgFillHealth;
   public Slider SliderHealth;
   public Gradient HPColor;
   public Button BPDestroy;
   public Batiment Batiment;
   public Transform Canavas;
   public List<Button> Buttons;
   
   [Header("UIAniamtion")] 
   public float OnselectedAnimationTime;
   public AnimationCurve OnselectedAnimationCurve =AnimationCurve.Linear(0,0,1,1);
   private Vector3[] _onSelecetedButtonsPositions ;

   public float OnDeselectedAnimationTime;
   public AnimationCurve OnDesceletedAnimationCurve = AnimationCurve.Linear(0,0,1,1);
   public Vector3 OnDeselectedButtonsPosition;
   
   public bool IsUpdateInpdate=true;

   public void Start()
   {
      if (Batiment is Achetables)
      {
         (Batiment as Achetables).OnSelected = SetOnButtons;
         (Batiment as Achetables).OnDeselected = SetOffButton;
         
      }

      _onSelecetedButtonsPositions = new Vector3[Buttons.Count];
      for (int i = 0; i < Buttons.Count; i++)
      {
         _onSelecetedButtonsPositions[i] = Buttons[i].transform.position;
      }
      
      OnDeselectedAniamtion();
   }

   public void Update()
   {
      if ( IsUpdateInpdate)UpdateUI();
   }

   public void SetOnButtons()
   {
      BPDestroy.gameObject.SetActive(true);
      Debug.Log(" Set Up button");
      OnselectedAniamtion();
   }
   public void SetOffButton()
   {
      BPDestroy.gameObject.SetActive(false);
      OnDeselectedAniamtion();
   }

   public void UpdateUI()
   {
      if ((float)Batiment.CurrentHp / Batiment.Hp==1) SliderHealth.gameObject.SetActive(false);
      else SliderHealth.gameObject.SetActive(true);
      SliderHealth.value = (float)Batiment.CurrentHp / Batiment.Hp;
      ImgFillHealth.color = HPColor.Evaluate((float)Batiment.CurrentHp / Batiment.Hp);
      if (transform.rotation.z != 0)
      {
         Canavas.localEulerAngles = new Vector3(0,0,-90);
      }
      else 
      {
         Canavas.rotation =Quaternion.identity;
      }
   }

   public void SetCursorOnUITrue()
   {
      PlayerManagerComponent.CursorOnUI = true;
   }

   public void SetCursorOnUIFalse()
   {
      PlayerManagerComponent.CursorOnUI = false;
   }

   public void UIBPDestroy()
   {
      Destroy(Batiment.gameObject);
      PlayerManagerComponent.CursorOnUI = false;
   }

   public void UIPBCancel()
   {
      (Batiment as Achetables) .OnDeselect();
   }

   private void OnselectedAniamtion()
   {
      for (int i = 0; i < Buttons.Count; i++)
      {
         Buttons[i].DOPause();
         Buttons[i].transform.position = Batiment.transform.position;
                                         //OnDeselectedButtonsPosition;
         Buttons[i].gameObject.SetActive(true);
         Buttons[i].transform.DOMove(_onSelecetedButtonsPositions[i], OnselectedAnimationTime);
         Buttons[i].transform.DOScale(Vector3.one, OnselectedAnimationTime).SetEase(OnselectedAnimationCurve);
      }
      Invoke("SetButtonActive",OnselectedAnimationTime );
   }

   private void OnDeselectedAniamtion()
   {
      for (int i = 0; i < Buttons.Count; i++)
      {
         Buttons[i].interactable = false;
         Buttons[i].DOPause();
         Buttons[i].transform.DOMove( Batiment.transform.position, OnDeselectedAnimationTime);
         Buttons[i].transform.DOScale(Vector3.zero,  OnDeselectedAnimationTime).OnComplete(delegate
         {
            Buttons[i].gameObject.SetActive(false);
         });
      }
   }

   private void SetButtonActive()
   {
      foreach (var button in Buttons)
      {
         button.interactable = true;
      }
   }
}
