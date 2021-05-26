using  System.Collections.Generic;
using Components;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InGameUIManagerComponent : MonoBehaviour
{
    public GameManagerComponent GameManagerComponent;
    [Header("PauseMenu")]
    public GameObject PanelPauseMenu;
    public UIElementComponent PauseMenu;
    [Header("WinMenu")] 
    public GameObject CanvaseWin;
    public float WinColorPanelAparitionTime;
    public CanvasGroup PanelWinColor;
    public UIElementComponent UIWin;
    [Header("LoseMenu")]
    public GameObject CanvaseLose;
    public float LoseBlackPanelAparitionTime=5f;
    public CanvasGroup PanelLoseBlack;
    public float LosePanelAparitionTime=1f;
    public CanvasGroup PanelLose;
     [Header("Black Screen Trasition")] 
    public float FadeTime;
    public GameObject CanvasBlackFade;
    public CanvasGroup PanelBlackFade;
    [Header("StartDialogue")] 
    public GameObject CanvasDialogueBackGourndColor;
    public UIElementComponent PanelDialogue;
    [Header("Bot Buttons")]
    public UIElementComponent BPPower;
    public UIElementComponent BPDestroy;
    [Header("Wave settings")] 
    public Slider SliderWave;
    public Slider SliderTimerWave;
    public TMP_Text TxtWaveInfo;
    public List<string> WaveTest;

    public void UIClickPause()
    {
        PanelPauseMenu.SetActive(true);
        PauseMenu.Activate();
        GameManagerComponent.SetPause();
    }

    public void UISetStartDialogue()
    {
        CanvasDialogueBackGourndColor.SetActive(true);
        PanelDialogue.Activate();
    }

    public void UISetStart()
    {
        GameManagerComponent.SetStartGame();
        CanvasDialogueBackGourndColor.SetActive(false);
        PanelDialogue.Desactivat();
    }
    
    public void UIOnMainMenu()
    {
        Time.timeScale = 1;
        CanvasBlackFade.SetActive(true);
        PanelBlackFade.DOFade(1, FadeTime).SetUpdate(true);
        Invoke("LoadMainMenu" , FadeTime);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void UIOnReturn()
    {
        GameManagerComponent.ResetPause();
        PanelPauseMenu.SetActive(false);
        PauseMenu.Desactivat();
    }

    public void UIOnQuite()
    {
        Application.Quit();
    }

    public void DoWin()
    {
        CanvaseWin.SetActive(true);
        UIWin.transform.eulerAngles = new Vector3(0,0,130);
        UIWin.transform.localScale = Vector3.zero;
        PanelWinColor.DOFade(1, WinColorPanelAparitionTime).SetUpdate(true);
        UIWin.Activate();
    }

    public void DoLose()
    {
        CanvaseLose.SetActive(true);
        PanelLose.alpha = 0;
        PanelLoseBlack.alpha = 0;
        PanelLoseBlack.DOFade(1, LoseBlackPanelAparitionTime).SetUpdate(true);
        PanelLose.DOFade(1, LosePanelAparitionTime).SetUpdate(true);
    }

    public void SetWaveSlider(float value)
    {
        SliderWave.value = value;
    }

    public void SetWaveTimerSlider(float value)
    {
        SliderTimerWave.value = value;
    }

    public void SetWaveInfoText(int value)
    {
        if (WaveTest[value]==null)TxtWaveInfo.text = "Erreu d'index de wave";
        TxtWaveInfo.text = WaveTest[value];
    }

    public void SetOnPowerButton()
    {
        BPPower.Activate();
    }

    public void SetOffPowerButton()
    {
       BPPower.Desactivat(); 
    }
}
