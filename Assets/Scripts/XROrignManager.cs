using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XROrignManager : MonoBehaviour
{
    public static XROrignManager Instance;
    public bool started;
    public bool inBorder;

    public bool idleToSearching;

    void Awake()
    {
        Instance = this;
        started = false;
        idleToSearching = false;
        if (transform.position.x < -2.5f || transform.position.x > 2.2f)
        {
            inBorder = false;
            Debug.Log("inBorder: " + inBorder);
        }
        else
        {
            inBorder = true;
        }
    }

    public void GameManagerOnGameStateChangedXROrigin(GameManager.GameState state)
    {
        if ( state == GameManager.GameState.Start)
        {
            this.transform.localPosition = new Vector3(3.1f, 0, 0);
            started= true;
            Debug.Log("Bug! gamestate: " + GameManager.Instance.State);
            GameManager.Instance.UpdateGameState(GameManager.GameState.Idle);
        }
    }

    private void Update()
    {
        if ( transform.position.x < -2.5f || transform.position.x > 2.2f )
        {
            inBorder = false;
            Debug.Log("inBorder: " + inBorder);
        }
        else
        {
            inBorder = true;
            Debug.Log("inBorder: " + inBorder);
            if ( GameManager.Instance.State == GameManager.GameState.Idle && ! idleToSearching )
            {
                idleToSearching = true;
                GameManager.Instance.UpdateGameState(GameManager.GameState.Shooting);
            }
        }
    }
}
