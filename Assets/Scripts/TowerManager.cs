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
    public bool shooting;
    public Material blood;

    public AudioSource bulletRicochet;
    public AudioSource visceralBulletImpact;
    public AudioSource rifleSilenced;

    private Transform lightTransform;
    private float exposedTime;
    private float exposedTimeLimit;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        exposedTime = 0;
        exposedTimeLimit = 40f;
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
            if ( exposedTime > 0 )
            {
                exposedTime -= Time.deltaTime;
            }
            else if ( exposedTime < 0 )
            {
                exposedTime = 0;
            }
        }
        else
        {
            searching = false;
        }
        if ( GameManager.Instance.State == GameManager.GameState.Shooting || GameManager.Instance.State == GameManager.GameState.Searching )
        {
            Debug.Log("Shooting decision");
            ShootingDecision();
            if ( ! searching )
            {
                shooting = true;
                Shooting();
            }
        }
        else
        {
            shooting = false;
        }

        inBorder = XROrignManager.Instance.inBorder;

        Color theColorToAdjust = blood.color;
        theColorToAdjust.a = exposedTime / exposedTimeLimit;
        blood.color = theColorToAdjust;
        // Debug.Log("theColorToAdjust.a: " + theColorToAdjust.a);

    }

    public void GameManagerOnGameStateChangedTower(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Shooting)
        {

        }
    }

    private void Shooting()
    {
        if( ! rifleSilenced.isPlaying )
        {
            rifleSilenced.Play();
        }
        if (!visceralBulletImpact.isPlaying)
        {
            visceralBulletImpact.Play();
        }
        if (!bulletRicochet.isPlaying)
        {
            bulletRicochet.Play();
        }

        Debug.Log("Inside Shooting function");

        if ( exposedTime < exposedTimeLimit && inBorder )
        {
            exposedTime += Time.deltaTime;
        }
        else if ( exposedTime >= exposedTimeLimit)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Lose);
        }
    }

    public void ShootingDecision()
    {
        Debug.Log("border in shooting decision: " + inBorder);
        if ( ! inBorder )
        {
            Debug.Log("Not in border");
            if (GameManager.Instance.State == GameManager.GameState.Shooting && GameManager.Instance.State != GameManager.GameState.Searching)
            {
                Debug.Log("update game state Searching");
                GameManager.Instance.UpdateGameState(GameManager.GameState.Searching);
                LightToggle(true);
                searching = true;
                shooting = false;
                rifleSilenced.Pause();
                visceralBulletImpact.Pause();
                bulletRicochet.Pause();
            }
        }
        else
        {
            if ( GameManager.Instance.State == GameManager.GameState.Idle || GameManager.Instance.State == GameManager.GameState.Searching )
            {
                Debug.Log("Trigger shooting from searching");
                shooting = true;
                GameManager.Instance.UpdateGameState(GameManager.GameState.Shooting);
            }
            searching = false;
            LightToggle(true);
        }
    }

    public void LightToggle( bool trigger )
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
