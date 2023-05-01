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
        if ( GameManager.Instance.State == GameManager.GameState.Shooting )
        {
            ShootingDecision();
            Shooting();
        }
    }

    private void Shooting()
    {
        Vector3 direction = lightTransform.forward;
        Debug.Log(direction.ToString());
        Ray theRay = new Ray(lightTransform.position, transform.TransformDirection(direction * Range));
        Debug.DrawLine(lightTransform.position, transform.TransformDirection(direction * Range));
    }

    public void ShootingDecision()
    {
        if ( ! inBorder )
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Searching);
            return;
        }
        else
        {
            Lights(true);
        }
    }

    private void Lights( bool trigger )
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
