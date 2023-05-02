using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public static Win Instance;

    private Transform lights;

    private void Start()
    {
        Instance = this;
        lights = this.transform.GetChild(0).transform;
    }

    public void WinLight()
    {
        lights.gameObject.SetActive(true);
    }
}
