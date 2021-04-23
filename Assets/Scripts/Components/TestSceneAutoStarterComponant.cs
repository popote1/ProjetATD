using System.Collections;
using System.Collections.Generic;
using Components;
using UnityEngine;

public class TestSceneAutoStarterComponant : MonoBehaviour
{
    public GameManagerComponent GameManagerComponent;
    void Start()
    {
        GameManagerComponent.SetTerrain();
        PlayerManagerComponent.Gold += 10000;
        GameManagerComponent.HomeBuildingTimer = 6;
        GameManagerComponent.IsUsingHomeSysteme = true;
    }
}
