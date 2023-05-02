using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedBoxManager : MonoBehaviour
{
    public static MedBoxManager Instance;

    public GameObject[] listOfMedBoxes;
    public GameObject box;

    private bool allMedBoxesMoved;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        box.SetActive(true);
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        allMedBoxesMoved = true;
        foreach (GameObject obj in listOfMedBoxes ) {
            Debug.Log("box: " + box.transform.position);
            Debug.Log("medbox: " + obj.name + ", " + obj.transform.position);
            if ( obj.transform.position.x <= box.transform.position.x + 1f && obj.transform.position.x >= box.transform.position.x - 1f
                && obj.transform.position.z <= box.transform.position.z + 1f && obj.transform.position.z >= box.transform.position.z - 1f)
            {
                Debug.Log("Medbox " + obj.gameObject.name + " is in the box");
                allMedBoxesMoved = allMedBoxesMoved && true;
            }
            else
            {
                allMedBoxesMoved = false;
            }
        }
        Debug.Log("allMedBoxesMoved: " + allMedBoxesMoved);
        if ( allMedBoxesMoved )
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
        }
    }

    public void WinDisableBox(bool toggle)
    {
        gameObject.SetActive(toggle);
        box.SetActive(toggle);
    }
}
