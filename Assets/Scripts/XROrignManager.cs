using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XROrignManager : MonoBehaviour
{
    public static XROrignManager Instance;
    public bool started;
    public bool inBorder;
    public bool searching;

    void Awake()
    {
        Instance = this;
        started = false;
        searching = false;
    }

    public void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if ( state == GameManager.GameState.Start)
        {
            this.transform.localPosition = new Vector3(3.1f, 0, 0);
            started= true;
            GameManager.Instance.State = GameManager.GameState.Idle;
        }
    }

    private void Update()
    {
        if ( transform.position.x < -2.5f || transform.position.x > 2.2 )
        {
            inBorder = false;
        }
        else
        {
            inBorder = true;
            if ( GameManager.Instance.State == GameManager.GameState.Searching )
            {
                GameManager.Instance.State = GameManager.GameState.Shooting;
            }
        }
    }
}
