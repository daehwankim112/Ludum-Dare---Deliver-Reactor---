using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public AudioSource dropping;
    public AudioSource explosion;
    public static BombManager Instance;

    public bool exploding;

    private void Awake()
    {
        Instance = this;
        exploding = false;
    }

    public async void Bombed()
    {
        dropping.Play();
        Debug.Log("Dropping");
        await Task.Delay((int)(dropping.clip.length * 1000));
        explosion.Play();
        exploding = true;
        Debug.Log("Explodding");
        await Task.Delay((int)(explosion.clip.length * 1000));
        exploding = false;
        GameManager.Instance.UpdateGameState(GameManager.GameState.Idle);
    }
}
