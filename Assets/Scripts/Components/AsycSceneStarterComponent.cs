using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using UnityEngine;

public class AsycSceneStarterComponent : MonoBehaviour
{
    public GameManagerComponent GameManagerComponent;
    public SmoothTerrainLoading SmoothTerrainLoading;
    public InGameUIManagerComponent InGameUIManagerComponent;
    private GameObject _menuScripte; 
    
    private void Awake()
    {
        
        _menuScripte =GameObject.Find("MainMenuScripts");
        if (_menuScripte != null)
        {
            GameManagerComponent.Seed = _menuScripte.GetComponent<MainMenuManagerComponent>().Seed;
            _menuScripte.GetComponent<MainMenuManagerComponent>().SmoothTerrainLoading = SmoothTerrainLoading;
        }
        
    }

    private void Start()
    {
        GameManagerComponent.SetTerrain();
        SmoothTerrainLoading.GenerateSmoothMesh();
        GameManagerComponent.GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if (SmoothTerrainLoading.LoadingProgress == 1)
        {
            Time.timeScale = 0;
            //Destroy(_menuScripte);
            InGameUIManagerComponent.UISetStartDialogue();
            GameManagerComponent.CalculateFlowField();
            Destroy(gameObject);
        }
    }
}
