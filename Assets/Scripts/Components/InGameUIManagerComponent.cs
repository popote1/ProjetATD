using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InGameUIManagerComponent : MonoBehaviour
{
    public GameObject PanekPauseMenu;


    public void UIClickPause()
    {
        PanekPauseMenu.SetActive(true);
    }

    public void UIOnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void UIOnReturn()
    {
        PanekPauseMenu.SetActive(false);
    }

    public void UIOnQuite()
    {
        Application.Quit();
    }
}
