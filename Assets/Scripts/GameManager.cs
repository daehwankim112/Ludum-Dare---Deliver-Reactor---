using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Shooting);        
    }

    private void Update()
    {
        Debug.Log("State: " + State);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Start:
                HandleStart();
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            case GameState.Shooting:
                HandleShooting();
                break;
            case GameState.Idle:
                StartCoroutine(HandleIdle());
                break;
            case GameState.Searching:
                HandleSearching();
                break;
            case GameState.Bombed:
                HandleBombed();
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleSearching()
    {
        Debug.Log("Seaching");
        for ( float timer = 4; timer > 0; timer -= Time.deltaTime )
        {
            if ( State != GameState.Searching )
            {
                return;
            }
            // Debug.Log("timer: " + timer);
        }
        UpdateGameState(GameState.Idle);
    }

    private void HandleShooting()
    {
        Debug.Log("HandleShooting triggered");
        OnGameStateChanged += TowerManager.Instance.GameManagerOnGameStateChangedTower;
    }

    private IEnumerator HandleIdle()
    {
        if ( State == GameState.Idle)   
        {
            float time = UnityEngine.Random.Range(5, 10);
            Debug.Log("Waiting time: " + time);
            yield return new WaitForSeconds(time);
            if ( State == GameState.Idle )
            {
                UpdateGameState(GameState.Bombed);
            }
        }
    }

    private void HandleBombed()
    {
        StartCoroutine(BombManager.Instance.Bombed());
    }

    private void HandleStart()
    {
        OnGameStateChanged += XROrignManager.Instance.GameManagerOnGameStateChangedXROrigin;
    }

    public enum GameState {
        Start,
        Shooting,
        Searching,
        Bombed,
        Idle,
        Win,
        Lose
    }
}