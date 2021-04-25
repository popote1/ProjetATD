using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using UnityEngine;

public class AsycSceneStarterComponent : MonoBehaviour
{
    public GameManagerComponent GameManagerComponent;
    public SmoothTerrainLoading SmoothTerrainLoading;
    private void Awake()
    {
        
        GameObject Go =GameObject.Find("MainMenuScripts");
        if (Go != null)
        {
            GameManagerComponent.Seed = Go.GetComponent<MainMenuManagerComponent>().Seed;
            Go.GetComponent<MainMenuManagerComponent>().SmoothTerrainLoading = SmoothTerrainLoading;
        }
        
    }

    private void Start()
    {
        GameManagerComponent.SetTerrain();
        SmoothTerrainLoading.GenerateSmoothMesh();
        GameManagerComponent.GetComponent<MeshRenderer>().enabled = false;
    }
}
