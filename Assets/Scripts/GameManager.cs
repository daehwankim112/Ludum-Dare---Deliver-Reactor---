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
        UpdateGameState(GameState.Bombed);        
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
                HandleIdle();
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
        XROrignManager.Instance.searching = true;
        for ( float timer = 4; timer > 0; timer -= Time.deltaTime )
        {
            if ( State != GameState.Searching )
            {
                timer = 4;
            }
        }
        State = GameState.Idle;
    }

    private void HandleShooting()
    {
        TowerManager.Instance.Shooting();
    }

    private async void HandleIdle()
    {
        if ( State == GameState.Idle)   
        {
            float time = UnityEngine.Random.Range(5, 10);
            Debug.Log("Waiting time: " + time);
            await Task.Delay((int)(time * 1000));
            UpdateGameState(GameState.Bombed);
        }
    }

    private void HandleBombed()
    {
        BombManager.Instance.Bombed();
    }

    private void HandleStart()
    {
        OnGameStateChanged += XROrignManager.Instance.GameManagerOnGameStateChanged;
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