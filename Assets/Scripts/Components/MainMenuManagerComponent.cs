using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManagerComponent : MonoBehaviour
{
    public GameObject PanelMainMenu;
    public GameObject PanelBourgName;
    public GameObject PanelOption;
    public GameObject PanelCredit;
    public UIElementComponent MainMenu;
    public UIElementComponent BourgName;
    public UIElementComponent Option;
    public UIElementComponent Credit;

    public TMP_InputField InputFieldBorgName;
    public string Seed;

    [Header("Loading Screen")] 
    public Canvas CanvasLoading;
    public CanvasGroup CanvasGroupLoading;
    public Slider SliderLoading;
    public TMP_Text TxtLoadingValue;
    public float FadeTime;

    [HideInInspector]public SmoothTerrainLoading SmoothTerrainLoading;
    

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(CanvasLoading);
        Option.Desactivat();
        BourgName.Desactivat();
        Credit.Desactivat();
    }

    public void UIClickPlay()
    {
        MainMenu.Desactivat();
        //BourgName.Activate();
       // PanelMainMenu.SetActive(false);
        BourgName.Activate();
    }

    public void UIReturnToMainMenu()
    {
        PanelCredit.SetActive(false);
        PanelOption.SetActive(false);
        PanelBourgName.SetActive(false);
        PanelMainMenu.SetActive(true);
    }

    public void UIBourgReturn()
    {
        BourgName.Desactivat();
        //BourgName.Desactivat();
        MainMenu.Activate();
       // UIReturnToMainMenu();
    }

    public void UILaunchGame()
    {
        Debug.Log("Lancement du jeu");
        StartCoroutine(LoadingScene());
    }
    

    public void UIOption()
    {
        Option.Activate();
        MainMenu.Desactivat();
       // PanelMainMenu.SetActive(false);
       // PanelOption.SetActive(true);
    }

    public void UIOptionReturn()
    {
        MainMenu.Activate();
        Option.Desactivat();
    }

    public void UICredits()
    {
        Credit.Activate();
        MainMenu.Desactivat();
       // PanelMainMenu.SetActive(false);
       // PanelCredit.SetActive(true);
    }

    public void UICreaditsReturn()
    {
        Credit.Desactivat();
        MainMenu.Activate();
    }

    public void UIQuit()
    {
        Application.Quit();
    }

    IEnumerator LoadingScene()
    {
        float loadingValur = 0;
        bool terrainReady = false;
        DOTweenModuleUI.DOFade(CanvasGroupLoading, 1, FadeTime);
        while (CanvasGroupLoading.alpha<1f)
        {
            yield return null;
        }
        
        AsyncOperation loading =SceneManager.LoadSceneAsync(1);
        while (!loading.isDone||!terrainReady)
        {
            if (SmoothTerrainLoading == null) loadingValur = loading.progress / 5f;
            else
            {
                if (SmoothTerrainLoading.LoadingProgress == 1)
                {
                    Debug.Log("Terrain ready"+ SmoothTerrainLoading.LoadingProgress);
                    terrainReady = true;
                }
                loadingValur = (loading.progress + SmoothTerrainLoading.LoadingProgress * 4) / 5f;
            }
            SliderLoading.value = loadingValur;
            TxtLoadingValue.text = Mathf.FloorToInt(loadingValur *100)+ "%";
            yield return null;
        }
        DOTweenModuleUI.DOFade(CanvasGroupLoading, 0, FadeTime).SetUpdate(true).OnComplete(SelfDestroy);

    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
    
    
}
