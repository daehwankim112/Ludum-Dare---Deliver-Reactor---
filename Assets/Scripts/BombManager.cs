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

    public IEnumerator Bombed()
    {
        if ( GameManager.Instance.State == GameManager.GameState.Bombed )
        {
            dropping.Play();
            Debug.Log("Dropping " + dropping.clip.length);
            yield return new WaitForSeconds(dropping.clip.length);
            explosion.Play();
            exploding = true;
            Debug.Log("Explodding " + explosion.clip.length);
            yield return new WaitForSeconds(explosion.clip.length);
            exploding = false;
            GameManager.Instance.UpdateGameState(GameManager.GameState.Idle);
        }
    }
}
