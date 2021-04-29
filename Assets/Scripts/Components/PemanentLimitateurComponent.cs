using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PemanentLimitateurComponent : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.Find(transform.name)!=gameObject)Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
