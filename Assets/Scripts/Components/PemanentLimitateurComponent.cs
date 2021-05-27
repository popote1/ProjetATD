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

    [Header("AudioManager")] 
    public AudioManager AudioManagere;
    private void Awake()
    {

        if (AudioManagere != null)
        {
            if (GameObject.Find(transform.name) != gameObject)
            {
                GameObject.Find(transform.name).GetComponent<AudioManager>();
                AudioManagere.IsSetByInspector = false;
                Destroy(GameObject.Find(transform.name));
                return;
            }
        }
        //if (GameObject.Find(transform.name)!=gameObject)Destroy(GameObject.Find(transform.name));
        if (GameObject.Find(transform.name)!=gameObject)Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
