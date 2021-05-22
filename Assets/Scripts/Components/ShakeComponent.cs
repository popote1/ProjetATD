using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShakeComponent : MonoBehaviour
{

   [Header("Skake 1")]
   public float Duration1 = 1;
   public float Strenght1=1;
   public int Vibrato1 = 10;
   public float Randomness1 = 10;
   public bool FadeOut1 = false;
   
   [Header("Skake 2")]
   public float Duration2 = 1;
   public float Strenght2=1;
   public int Vibrato2 = 10;
   public float Randomness2 = 10;
   public bool FadeOut2 = false;
   
   [Header("Skake 3")]
   public float Duration3 = 1;
   public float Strenght3=1;
   public int Vibrato3 = 10;
   public float Randomness3 = 10;
   public bool FadeOut3 = false;

   
   private static float _duration1 = 1;
   private static float _strenght1=1;
   private static int _vibrato1 = 10;
   private static float _randomness1 = 10;
   private static bool _fadeOut1 = false;
   
   private static float _duration2 = 1;
   private static float _strenght2=1;
   private static int _vibrato2 = 10;
   private static float _randomness2 = 10;
   private static bool _fadeOut2 = false;
   
   private static float _duration3 = 1;
   private static float _strenght3=1;
   private static int _vibrato3 = 10;
   private static float _randomness3 = 10;
   private static bool _fadeOut3 = false;
   
   private static Transform _camera;

   private void Start()
   {
      _camera = transform;

      _duration1 = Duration1;
      _strenght1 = Strenght1;
      _vibrato1 = Vibrato1;
      _randomness1 = Randomness1;
      _fadeOut1 = FadeOut1;
      
      _duration2 = Duration2;
      _strenght2 = Strenght2;
      _vibrato2 = Vibrato2;
      _randomness2 = Randomness2;
      _fadeOut2 = FadeOut2;
      
      _duration3 = Duration3;
      _strenght3 = Strenght3;
      _vibrato3 = Vibrato3;
      _randomness3 = Randomness3;
      _fadeOut3 = FadeOut3;
   }


   public static void DoShake(int value)
   {
      if (value ==1) _camera.DOShakePosition(_duration1, _strenght1, _vibrato1, _randomness1, _fadeOut1);
      else if (value ==2)_camera.DOShakePosition(_duration2, _strenght2, _vibrato2, _randomness2, _fadeOut2);
      else if (value ==3)_camera.DOShakePosition(_duration3, _strenght3, _vibrato3, _randomness3, _fadeOut3);
   }
}
