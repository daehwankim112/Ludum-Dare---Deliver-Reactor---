using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    public AudioSource slowDown;

    public Material black;

    private float gameTimer;
    private float startLoadingTime;
    private float tempTimer;
    private bool everChangedFromSearching;
    private bool everChangedFromSearchingActivate;
    private bool gameStart;

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
        startLoadingTime = 3f;
    }

    private void Update()
    {
        Debug.Log("State: " + State);
        gameTimer += Time.deltaTime;
        if ( ! everChangedFromSearching && State != GameState.Searching && everChangedFromSearchingActivate)
        {
            everChangedFromSearching = true;
        }
        if (gameStart)
        {
            tempTimer = gameTimer;
            gameStart = false;
        }
        if ( gameTimer <= tempTimer + startLoadingTime )
        {
            Color theColorToAdjust = black.color;
            theColorToAdjust.a = (tempTimer + startLoadingTime - gameTimer) / startLoadingTime;
            black.color = theColorToAdjust;
            if ( theColorToAdjust.a < 0.02f )
            {
                theColorToAdjust.a = 0;
                black.color = theColorToAdjust;
            }
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
                HandleWin();
                break;
            case GameState.Lose:
                HandleLose();
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

    private void HandleWin()
    {
        Win.Instance.WinLight();
        MedBoxManager.Instance.WinDisableBox(false);
    }

    private void HandleLose()
    {
        slowDown.Play();
        XROrignManager.Instance.Lose();
        TowerManager.Instance.Lose();
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
        gameStart = true;
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