using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XROrignManager : MonoBehaviour
{

    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if ( state == GameManager.GameState.Start)
        {
            this.transform.localPosition = new Vector3(3.1f, 0, 0);

        }
    }

    private void Update()
    {
        
    }
}
