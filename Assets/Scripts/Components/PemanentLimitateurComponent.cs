using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PemanentLimitateurComponent : MonoBehaviour
{
    
    [Header("Loading Screen")] 
    public Canvas CanvasLoading;
    public CanvasGroup CanvasGroupLoading;
    public Slider SliderLoading;
    public TMP_Text TxtLoadingValue;
    private void Awake()
    {
        if (GameObject.Find(transform.name)!=gameObject)Destroy(GameObject.Find(transform.name));
        DontDestroyOnLoad(gameObject);
    }
}
