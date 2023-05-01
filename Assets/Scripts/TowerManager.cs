using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private bool exploding;
    [SerializeField] private bool inBorder;

    public static TowerManager Instance;

    public GameObject lightGameObject;
    public float Range;
    public bool searching;

    private Transform lightTransform;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.exploding = BombManager.Instance.exploding;
        inBorder = XROrignManager.Instance.inBorder;
        lightTransform = lightGameObject.transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.Searching)
        {
            searching = true;
        }
        else
        {
            searching = false;
        }
        if ( GameManager.Instance.State == GameManager.GameState.Shooting )
        {
            ShootingDecision();
            if ( ! searching )
            {
                Shooting();
            }
        }
    }

    public void GameManagerOnGameStateChangedTower(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Shooting)
        {

        }
    }

    private void Shooting()
    {
        Debug.Log("Inside Shooting function");
        Vector3 direction = lightTransform.forward;
        Debug.Log(direction.ToString());
        Ray theRay = new Ray(lightTransform.position, transform.TransformDirection(direction * Range));
        Debug.DrawLine(lightTransform.position, transform.TransformDirection(direction * Range));
    }

    public void ShootingDecision()
    {
        if ( ! inBorder )
        {
            if (GameManager.Instance.State == GameManager.GameState.Shooting && ! searching)
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Searching);
                searching = true;
            }
            return;
        }
        else
        {
            if ( GameManager.Instance.State == GameManager.GameState.Idle || GameManager.Instance.State == GameManager.GameState.Searching )
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Shooting);
            }
            searching = false;
            LightToggle(true);
        }
    }

    private void LightToggle( bool trigger )
    {
        if ( trigger )
        {
            if ( ! lightGameObject.activeSelf )
            {
                lightGameObject.SetActive(true);
            }
        }
        else
        {
            if ( lightGameObject.activeSelf )
            {
                lightGameObject.SetActive(false);
            }
        }
    }
}
