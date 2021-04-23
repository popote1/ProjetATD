using System;
using System.Collections;
using System.Collections.Generic;
using Components;
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

    public TMP_InputField InputFieldBorgName;
    public string Seed;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UIClickPlay()
    {
        PanelMainMenu.SetActive(false);
        PanelBourgName.SetActive(true);
    }

    public void UIReturnToMainMenu()
    {
        PanelCredit.SetActive(false);
        PanelOption.SetActive(false);
        PanelBourgName.SetActive(false);
        PanelMainMenu.SetActive(true);
    }

    public void UILaunchGame()
    {
        Debug.Log("Lancement du jeu");
        StartCoroutine(LoadingScene());

    }

    public void UIOption()
    {
        PanelMainMenu.SetActive(false);
        PanelOption.SetActive(true);
    }

    public void UICredits()
    {
        PanelMainMenu.SetActive(false);
        PanelCredit.SetActive(true);
    }

    public void UIQuit()
    {
        Application.Quit();
    }

    IEnumerator LoadingScene()
    {
        AsyncOperation loading =SceneManager.LoadSceneAsync(1);
        while (!loading.isDone)
        {
            Debug.Log(loading.progress);
            yield return null;
        }
        

    }
    
    
}
