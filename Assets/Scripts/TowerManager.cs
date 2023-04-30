using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private bool exploding;
    [SerializeField] private bool inBorder;

    public static TowerManager Instance;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.exploding = BombManager.Instance.exploding;
        inBorder = XROrignManager.Instance.inBorder;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shooting()
    {
        if ( ! inBorder )
        {
            GameManager.Instance.State = GameManager.GameState.Searching;
            return;
        }
        else
        {
            
        }
    }
}
