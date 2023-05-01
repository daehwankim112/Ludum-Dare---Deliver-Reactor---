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
            GameManager.Instance.UpdateGameState(GameManager.GameState.Idle);
            Debug.Log("Bug! gamestate: " + GameManager.Instance.State);
        }
    }

    private void Update()
    {
        if ( transform.position.x < -2.5f || transform.position.x > 2.2f )
        {
            inBorder = false;
            Debug.Log("inBorder: " + inBorder);
            if ( GameManager.Instance.State == GameManager.GameState.Shooting )
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Searching);
                searching = true;
            }
        }
        else
        {
            inBorder = true;
            if ( searching )
            {
                Debug.Log("Found! Start shooting");
                GameManager.Instance.UpdateGameState(GameManager.GameState.Shooting);
                searching = false;
            }
        }
    }
}
