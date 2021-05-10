using System.Collections.Generic;
using Assets.Scripts.Bourg;
using Assets.Scripts.Bourg.Achetable;
using Components;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldTowerComponent : MonoBehaviour
{
    public Image ImgFillHealth;
   public Slider SliderHealth;
   public Gradient HPColor;
   public Button BPActive;
   public Image ImgBPActive;
   public Tower Batiment;
   public List<Button> Buttons;
   
   [Header("UIAniamtion")] 
   public float OnselectedAnimationTime =0.2f;
   public AnimationCurve OnselectedAnimationCurve =AnimationCurve.Linear(0,0,1,1);
   private Vector3[] _onSelecetedButtonsPositions ;

   public float OnDeselectedAnimationTime=0.1f;
   public AnimationCurve OnDesceletedAnimationCurve = AnimationCurve.Linear(0,0,1,1);
   public Vector3 OnDeselectedButtonsPosition;
   
   public bool IsUpdateInpdate=true;

   public void Start()
   {
      Batiment.OnSelected = SetOnButtons;
      Batiment.OnDeselected = SetOffButton;
      
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
      //BPDestroy.gameObject.SetActive(true);
      
      OnselectedAniamtion();
   }
   public void SetOffButton()
   {
     // BPDestroy.gameObject.SetActive(false);
      OnDeselectedAniamtion();
   }

   public void UpdateUI()
   {
      if ((float)Batiment.CurrentHp / Batiment.Hp==1) SliderHealth.gameObject.SetActive(false);
      else SliderHealth.gameObject.SetActive(true);
      
      SliderHealth.value = (float)Batiment.CurrentHp / Batiment.Hp;
      ImgFillHealth.color = HPColor.Evaluate((float)Batiment.CurrentHp / Batiment.Hp);

      if (!Batiment.IsPowerAvtivated)
      {
         if (!Batiment.IsReadyToFire)
         {
            ImgBPActive.fillAmount = Batiment.ActiveTimer / Batiment.ActiveCouldown;
            BPActive.interactable = false;
         }
         else
         {
            ImgBPActive.fillAmount = 1;
            BPActive.interactable = true;
         }
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

   public void UIBPActivePower()
   {
      
      Debug.Log("Click sur ActivePower");
      for (int i = 1; i < Buttons.Count; i++)
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

   public void UIBPDestroy()
   {
      Debug.Log("Click sur Destroy");
      Destroy(Batiment.gameObject);
      PlayerManagerComponent.CursorOnUI = false;
   }

   public void UIPBCancel()
   {
      Debug.Log("Click sur CAncel");
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
         if (button ==BPActive&&!Batiment.IsReadyToFire) button.interactable = false;
      }
   }
}
