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

    private float gameTimer;
    private bool everChangedFromSearching;
    private bool everChangedFromSearchingActivate;

    void Awake()
    {
        Instance = this;
        gameTimer = 0;
    }

    void Start()
    {
        UpdateGameState(GameState.Start);
        everChangedFromSearching = false;
        everChangedFromSearchingActivate = false;
    }

    private void Update()
    {
        Debug.Log("State: " + State);
        gameTimer += Time.deltaTime;
        if ( ! everChangedFromSearching && State != GameState.Searching && everChangedFromSearchingActivate)
        {
            everChangedFromSearching = true;
        }
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
                StartCoroutine(HandleSearching());
                break;
            case GameState.Bombed:
                HandleBombed();
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private IEnumerator HandleSearching()
    {
        Debug.Log("Seaching");
        float timer = gameTimer;
        everChangedFromSearching = false;
        everChangedFromSearchingActivate = true;
        yield return new WaitForSeconds(4);
        if ( everChangedFromSearching )
        {
            Debug.Log("State is not Searching. State: " + State);
        }
        else
        {
            Debug.Log("Time is up");
            UpdateGameState(GameState.Idle);
        }
        everChangedFromSearching = false;
        everChangedFromSearchingActivate = false;
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
            TowerManager.Instance.LightToggle(false);
            float time = UnityEngine.Random.Range(5, 10);
            Debug.Log("Waiting time: " + time);
            yield return new WaitForSeconds(time);
            if ( State == GameState.Idle && ! XROrignManager.Instance.idleToSearching )
            {
                UpdateGameState(GameState.Bombed);
            }
            else if ( State == GameState.Idle && XROrignManager.Instance.idleToSearching )
            {
                XROrignManager.Instance.idleToSearching = false;
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